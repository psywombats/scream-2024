﻿using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntertitleComponent : MonoBehaviour
{
    [SerializeField] private GameObject enableChild;
    [SerializeField] private CanvasGroup renderGroup;
    [SerializeField] private CanvasGroup fader;
    [Space]
    [SerializeField] private Text big;
    [SerializeField] private Text small;
    [Space]
    [SerializeField] private float t1 = 1f;
    [SerializeField] private float t2 = 4f;

    public IEnumerator DoIntertitleCommand(string s1, string s2)
    {
        big.text = s1;
        small.text = s2;

        yield return CoUtils.RunTween(fader.DOFade(1f, t1));
        enableChild.SetActive(true);
        yield return CoUtils.RunTween(renderGroup.DOFade(1f, t1));
        yield return CoUtils.Wait(t2);
        yield return CoUtils.RunTween(renderGroup.DOFade(0f, t1));
        enableChild.SetActive(false);
        yield return CoUtils.RunTween(fader.DOFade(1f, t1));
    }
}