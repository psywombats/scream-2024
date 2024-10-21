﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TextAutotyper : MonoBehaviour, IInputListener
{

    [SerializeField] public Text textbox;
    [SerializeField] public float charsPerSecond = 120f;
    [SerializeField] protected GameObject advanceArrow;
    [SerializeField] protected bool speedUpWhenHurried;

    public bool mode2 = false;

    public int LinesTyped { get; private set; } = 0;

    protected int typingStartIndex = 0;
    protected bool hurried;
    protected bool confirmed;

    public virtual void Clear()
    {
        textbox.text = "";
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType)
    {
        switch (eventType)
        {
            case InputManager.Event.Hold:
                if (command == InputManager.Command.Primary)
                {
                    hurried = true;
                }
                break;
            case InputManager.Event.Down:
                if (command == InputManager.Command.Primary)
                {
                    confirmed = true;
                }
                break;
        }
        return true;
    }

    //public void StartGlitch()
    //{
    //    StartCoroutine(MapOverlayUI.Instance.adv.GetHighlightedPortrait().JoltRoutine());
    //    StartCoroutine(AudioManager.Instance.JumpscareRoutine());
    //}

    public IEnumerator TypeRoutine(string text, bool waitForConfirm = true)
    {
        Debug.Log("!!! type routine begins");
        hurried = false;
        confirmed = false;
        float elapsed = 0.0f;
        float total = (text.Length - typingStartIndex) / charsPerSecond;
        textbox.GetComponent<CanvasGroup>().alpha = 1.0f;

        var containsGlitch = text.IndexOf("____") >= 0;
        var glitchesDelayed = 0;
        var glitchStartedAt = -1;

        while (elapsed <= total)
        {
            Debug.Log("!!! while elapsed < total");
            elapsed += Time.deltaTime;
            int charsToShow = Mathf.FloorToInt(elapsed * charsPerSecond) + typingStartIndex;
            int cutoff = charsToShow > text.Length ? text.Length : charsToShow;
            textbox.text = text.Substring(0, cutoff);

            var uCount = 0;
            Debug.Log("!!! foreach c in text");
            foreach (var c in textbox.text)
            {
                if (c == '_')
                {
                    uCount += 1;
                }
            }
            Debug.Log("!!! glitch search");
            if (glitchesDelayed < uCount)
            {
                var tryg = textbox.text.Length;
                while (tryg > 0 && textbox.text[tryg - 1] == '_')
                {
                    tryg -= 1;
                }
                if (tryg > glitchStartedAt)
                {
                    glitchStartedAt = tryg;
                    //StartGlitch();
                }
                yield return CoUtils.Wait(.175f);
                glitchesDelayed = uCount;
            }

            if (!mode2)
            {
                textbox.text += "<color=#aa000000>";
                textbox.text += text.Substring(cutoff);
                textbox.text += "</color>";
            }
            yield return null;

            elapsed += Time.deltaTime;
            if (hurried)
            {
                hurried = false;
                if (speedUpWhenHurried)
                {
                    elapsed += Time.deltaTime * 4;
                }
            }
            if (confirmed && !containsGlitch)
            {
                confirmed = false;
                if (!speedUpWhenHurried)
                {
                    break;
                }
            }
        }
        textbox.text = text;

        Debug.Log("!!! confirm wait check");
        if (waitForConfirm)
        {
            confirmed = false;
            if (advanceArrow != null) advanceArrow.SetActive(true);
            while (!confirmed)
            {
                yield return null;
            }
            if (advanceArrow != null) advanceArrow.SetActive(false);
        }
    }
}
