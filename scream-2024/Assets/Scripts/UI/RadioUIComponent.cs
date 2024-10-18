using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadioUIComponent : MonoBehaviour
{
    [SerializeField] private ExpanderComponent expander;
    [SerializeField] private Text nameText;
    [SerializeField] private LineAutotyper textTyper;
    [SerializeField] private Image portrait;

    private bool active;

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

        if (!active)
        {
            yield return expander.ShowRoutine();
            active = true;
        }
        yield return textTyper.WriteLineRoutine(text);
    }

    public IEnumerator HideRoutine()
    {
        yield return expander.HideRoutine();
        active = false;
    }
}
