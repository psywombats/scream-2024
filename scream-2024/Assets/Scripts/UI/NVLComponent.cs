using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NVLComponent : MonoBehaviour
{
    private static float bgTime = 0.5f;

    public PortraitComponent slotA;
    public PortraitComponent slotB;
    public PortraitComponent slotC;
    public PortraitComponent slotD;
    public PortraitComponent slotE;

    public GameObject backerArea;
    public ExpanderComponent backer;
    //public LineAutotyper text;
    public TextAutotyper text;
    public Text nameText;
    public CanvasGroup fader;
    public CanvasGroup background;

    public bool IsShown { get; private set; }

    public Dictionary<SpeakerData, string> speakerNames = new Dictionary<SpeakerData, string>();

    public void Wipe()
    {
        text.Clear();
        nameText.text = "";
    }

    public IEnumerator ShowRoutine(bool hideBackers = false)
    {
        IsShown = true;
        backer.Hide();
        fader.alpha = 0.0f;
        background.alpha = 0f;
        foreach (var portrait in GetPortraits())
        {
            portrait.Clear();
        }

        Global.Instance.Audio.PlaySFX("in_game/popups", null, AudioManager.Bank.UI);
        StartCoroutine(CoUtils.RunTween(MapOverlayUI.Instance.info.DOFade(0, bgTime)));

        if (!hideBackers)
        {
            backerArea.gameObject.SetActive(true);
            StartCoroutine(CoUtils.RunTween(background.DOFade(1, bgTime)));
        }
        else
        {
            backerArea.gameObject.SetActive(false);
        }
        yield return backer.ShowRoutine();
        text.Clear();
        Wipe();
    }

    public IEnumerator HideRoutine()
    {
        var routines = new List<IEnumerator>();
        foreach (var portrait in GetPortraits())
        {
            if (portrait.Speaker != null)
            {
                routines.Add(portrait.ExitRoutine());
            }
        }
        yield return CoUtils.RunParallel(routines.ToArray(), this);
        routines.Clear();
        routines.Add(backer.HideRoutine());
        routines.Add(CoUtils.RunTween(fader.DOFade(0.0f, backer.duration)));
        routines.Add(CoUtils.RunTween(background.DOFade(0, bgTime)));
        routines.Add(CoUtils.RunTween(MapOverlayUI.Instance.info.DOFade(1, bgTime)));
        yield return CoUtils.RunParallel(routines.ToArray(), this);
        Wipe();
        IsShown = false;
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string slot, string expr = null)
    {
        var portrait = GetPortrait(slot);
        yield return portrait.EnterRoutine(speaker, expr);
    }

    public IEnumerator ExitRoutine(SpeakerData speaker)
    {
        foreach (var portrait in GetPortraits())
        {
            if (portrait.Speaker == speaker)
            {
                yield return portrait.ExitRoutine();
            }
        }
    }

    public IEnumerator SpeakRoutine(SpeakerData speaker, string message)
    {
        Wipe();
        var name = speaker.displayName;

        if (!IsShown)
        {
            yield return ShowRoutine(hideBackers: true);
        }

        if (speaker != null)
        {
            yield return SetHighlightRoutine(speaker);
        }

        string toType = message;
        nameText.text = name;
        //yield return text.WriteLineRoutine(toType);
        yield return text.TypeRoutine(toType, waitForConfirm: false);
        yield return InputManager.Instance.ConfirmRoutine();
        Global.Instance.Audio.PlaySFX("in_game/popups", null, AudioManager.Bank.UI);
    }

    public IEnumerator SetHighlightRoutine(SpeakerData speaker)
    {
        var portrait = GetPortrait(speaker);
        var routines = new List<IEnumerator>();
        if (portrait != null && !portrait.IsHighlighted)
        {
            routines.Add(portrait.HighlightRoutine());
        }
        foreach (var other in GetPortraits())
        {
            if (other.Speaker != null && other.Speaker != speaker && other.IsHighlighted)
            {
                routines.Add(other.UnhighlightRoutine());
            }
        }
        yield return CoUtils.RunParallel(routines.ToArray(), this);
    }

    public PortraitComponent GetHighlightedPortrait()
    {
        return GetPortraits().Where(p => p.IsHighlighted).First();
    }

    private PortraitComponent GetPortrait(string slot)
    {
        PortraitComponent portrait = null;
        switch (slot.ToLower())
        {
            case "a": portrait = slotA; break;
            case "b": portrait = slotB; break;
            case "c": portrait = slotC; break;
            case "d": portrait = slotD; break;
            case "e": portrait = slotE; break;
        }
        return portrait;
    }

    public PortraitComponent GetPortrait(SpeakerData speaker)
    {
        if (slotA.Speaker == speaker) return slotA;
        if (slotB.Speaker == speaker) return slotB;
        if (slotC.Speaker == speaker) return slotC;
        if (slotD.Speaker == speaker) return slotD;
        if (slotE.Speaker == speaker) return slotE;
        return null;
    }

    private List<PortraitComponent> GetPortraits()
    {
        return new List<PortraitComponent>() {
            slotA,
            slotB,
            slotC,
            slotD,
            slotE,
        };
    }
}
