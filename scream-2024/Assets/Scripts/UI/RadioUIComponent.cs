using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadioUIComponent : MonoBehaviour
{
    [SerializeField] private ExpanderComponent expander;
    [SerializeField] private Text nameText;
    [SerializeField] private LineAutotyper textTyper;
    [SerializeField] private Image portrait;

    public bool IsShown { get; private set; }

    public void Start()
    {
        expander.Hide();
    }

    public IEnumerator SpeakRoutine(string speakerKey, string text)
    {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerKey);
        nameText.text = speaker.displayName;
        portrait.sprite = speaker.sprite;
        portrait.SetNativeSize();

        if (!IsShown)
        {
            yield return expander.ShowRoutine();
            IsShown = true;
        }
        yield return null;
        yield return textTyper.WriteLineRoutine(text);
        yield return Global.Instance.Input.ConfirmRoutine();
    }

    public IEnumerator HideRoutine()
    {
        yield return expander.HideRoutine();
        IsShown = false;
    }
}
