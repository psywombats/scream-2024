using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadioUIComponent : MonoBehaviour
{
    public enum RadioQual
    {
        good,
        okay,
        bad,
    }

    [SerializeField] private ExpanderComponent expander;
    [SerializeField] private Text nameText;
    [SerializeField] private TextAutotyper textTyper;
    [SerializeField] private Image portraitGood;
    [SerializeField] private Image portraitOkay;
    [SerializeField] private Image portraitBad;
    
    public bool IsShown { get; private set; }

    public void Start()
    {
        expander.Hide();
    }

    public IEnumerator SpeakRoutine(string speakerKey, string text, RadioQual qual = RadioQual.good)
    {
        textTyper.Clear();
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerKey);
        nameText.text = speaker.displayName;

        Image portrait = null;
        switch (qual)
        {
            case RadioQual.bad:
                portrait = portraitBad;
                break;
            case RadioQual.good:
                portrait = portraitGood;
                break;
            case RadioQual.okay:
                portrait = portraitOkay;
                break;
        }

        portraitBad.gameObject.SetActive(false);
        portraitGood.gameObject.SetActive(false);
        portraitOkay.gameObject.SetActive(false);

        portrait.gameObject.SetActive(true);
        portrait.sprite = speaker.radioSprite;

        if (!IsShown)
        {
            if (qual == RadioQual.bad)
            {
                Global.Instance.Audio.PlaySFX("in_game/radio_broken", null, AudioManager.Bank.UI);
            }
            else
            {
                Global.Instance.Audio.PlaySFX("in_game/radio_on", null, AudioManager.Bank.UI);
            }
            yield return expander.ShowRoutine();
            IsShown = true;
        }
        yield return null;
        yield return textTyper.TypeRoutine(text, true);
    }

    public IEnumerator HideRoutine()
    {
        Global.Instance.Audio.PlaySFX("in_game/radio_off", null, AudioManager.Bank.UI);
        yield return expander.HideRoutine();
        textTyper.Clear();
        IsShown = false;
    }
}
