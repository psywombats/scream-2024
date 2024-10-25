using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PortraitComponent : MonoBehaviour
{

    private static readonly float fadeTime = 0.5f;
    private static readonly float highlightTime = 0.3f;
    private static readonly float inactiveAlpha = 0.5f;

    public NVLComponent nvl;
    public Image sprite;
    public Image altSprite;
    public Sprite trans;
    public bool moveSibling;
    public bool crater;

    public SpeakerData Speaker { get; private set; }
    public bool IsHighlighted { get; private set; }

    public void Clear()
    {
        Speaker = null;
        sprite.sprite = trans;
        altSprite.sprite = trans;
        IsHighlighted = false;
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string expr = null)
    {
        if (Speaker != null)
        {
            yield return ExitRoutine();
        }
        Speaker = speaker;
        sprite.sprite = GetSpriteForExpr(speaker, expr);
        sprite.sprite = speaker.sprite;

        sprite.SetNativeSize();
        sprite.color = new Color(1, 1, 1, 0);
        IsHighlighted = true;
        if (speaker.moonSprite != null && crater)
        {
            yield return CoUtils.RunParallel(this,
                CoUtils.RunTween(sprite.DOFade(1.0f, fadeTime)),
                MooncraterRoutine(fadeTime));
        }
        else
        {
            yield return CoUtils.RunTween(sprite.DOFade(1.0f, fadeTime));
        }

        IsHighlighted = true;
    }

    public IEnumerator MooncraterRoutine(float fadeTime)
    {
        altSprite.color = new Color(1, 1, 1, 0);
        altSprite.sprite = Speaker.moonSprite;
        altSprite.SetNativeSize();
        yield return CoUtils.RunTween(altSprite.DOFade(1f, fadeTime / 2f));
        yield return CoUtils.RunTween(altSprite.DOFade(0f, fadeTime / 2f));
    }

    private Sprite GetSpriteForExpr(SpeakerData speaker, string expr)
    {
        if (string.IsNullOrEmpty(expr))
        {
            return speaker.sprite;
        }
        Sprite found = null;
        foreach (var sub in speaker.exprs)
        {
            if (sub.key == expr)
            {
                found = sub.sprite;
                break;
            }
        }
        if (found == null)
        {
            found = speaker.sprite;
            Debug.LogWarning("Couldn't find expression " + expr + " for " + speaker.Key);
        }
        return found;
    }

    public IEnumerator ExitRoutine()
    {
        if (Speaker != null)
        {
            if (Speaker.moonSprite != null && crater)
            {
                yield return CoUtils.RunParallel(this,
                    CoUtils.RunTween(sprite.DOFade(0f, fadeTime)),
                    MooncraterRoutine(fadeTime));
            }
            else
            {
                yield return CoUtils.RunTween(sprite.DOFade(0f, fadeTime));
            }
            Clear();
        }
        if (moveSibling)
        {
            transform.SetAsFirstSibling();
        }
    }

    public IEnumerator HighlightRoutine()
    {
        if (sprite.color.r == 1.0f)
        {
            yield break;
        }
        var tween = sprite.DOColor(new Color(1, 1, 1, 1), highlightTime);
        if (moveSibling)
        {
            transform.SetAsLastSibling();
        }
        yield return CoUtils.RunTween(tween);

        IsHighlighted = true;
    }

    public IEnumerator ExpressRoutine(string expr)
    {
        if (!IsHighlighted)
        {
            yield return nvl.SetHighlightRoutine(Speaker);
        }
        altSprite.color = new Color(1, 1, 1, 0);
        altSprite.sprite = GetSpriteForExpr(Speaker, expr);
        altSprite.SetNativeSize();
        yield return CoUtils.RunParallel(this,
            //CoUtils.RunTween(sprite.DOColor(new Color(inactiveAlpha, inactiveAlpha, inactiveAlpha, 1f), highlightTime)),
            CoUtils.RunTween(altSprite.DOFade(1f, highlightTime)));
        sprite.sprite = altSprite.sprite;
        sprite.color = Color.white;
        altSprite.color = Color.clear;
        altSprite.sprite = null;
    }

    public IEnumerator UnhighlightRoutine()
    {
        if (Speaker == null || sprite.color.r == inactiveAlpha)
        {
            yield break;
        }
        var tween = sprite.DOColor(new Color(inactiveAlpha, inactiveAlpha, inactiveAlpha, 1), highlightTime);
        yield return CoUtils.RunTween(tween);
        if (moveSibling)
        {
            transform.SetAsFirstSibling();
        }

        IsHighlighted = false;
    }
}
