﻿using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(MapEvent))]
public class CharaEvent : MonoBehaviour
{
    private const float TargetHeight = 2.2f;
    private const float HighlightSpeed = 2f;

    [SerializeField] public DollComponent doll;
    [SerializeField] private string speakerTag;
    [SerializeField] private bool phaseIn;
    [SerializeField] private float phaserDelay = .8f;
    [SerializeField] public OrthoDir dir = OrthoDir.South;

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private bool highlightNow;

    public void Start()
    {
        UpdateRenderer();

        if (phaseIn)
        {
            StartCoroutine(PhaseInRoutine());
        }
        SetFacing(dir);
        Event.enableChild = doll.gameObject;
        doll.gameObject.SetActive(Event.IsSwitchEnabled);
    }

    private IEnumerator PhaseInRoutine()
    {
        doll.offsetter.transform.localScale = new Vector3(0, 0, 1);
        yield return CoUtils.Wait(phaserDelay);
        doll.offsetter.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutBounce).Play();
        doll.offsetter.transform.DOScaleX(1f, 1.5f).SetEase(Ease.OutCubic).Play();
    }

    public void Update()
    {
        var a = doll.highlightRenderer.color.a;
        var targetA = highlightNow ? 1f : 0f;
        var delta = HighlightSpeed * Time.deltaTime;
        if (a < targetA) delta *= -1;
        a += delta;
        if (delta < 0 && a < targetA) a = targetA;
        if (delta > 0 && a > targetA) a = targetA;

        doll.highlightRenderer.color = new Color(
            doll.highlightRenderer.color.r,
            doll.highlightRenderer.color.g,
            doll.highlightRenderer.color.b,
            a);

        if (Global.Instance.Avatar != null && !Global.Instance.Avatar.IsPaused)
        {
            highlightNow = false;
        }
    }

    public void SetFacing(OrthoDir dir)
    {
        if (doll != null)
        {
            doll.offsetter.transform.localEulerAngles = new Vector3(0, dir.Rot3D(), 0);
        }
    }

    public void OnValidate()
    {
        if (!Application.isPlaying)
        {
            UpdateRenderer();
            SetFacing(dir);
        }
    }

    public void Interact() => Event.Interact();

    public void UpdateRenderer()
    {
        if (speakerTag == null)
        {
            return;
        }
        var speaker = IndexDatabase.Instance.Speakers.GetDataOrNull(speakerTag);
        if (speaker != null)
        {
            var factor = TargetHeight / speaker.sprite.rect.height * speaker.sprite.pixelsPerUnit;
            foreach (var renderer in doll.renderers)
            {
                renderer.transform.localScale = new Vector3(factor, factor, factor);
                renderer.sprite = speaker.sprite;
            }
            doll.highlightRenderer.transform.localScale = new Vector3(factor, factor, factor);
            //doll.highlightRenderer.sprite = speaker.glow;
        }
    }

    public void HandleRay()
    {
        highlightNow = true;
    }
}
