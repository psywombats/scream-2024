using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class TitleComponent : MonoBehaviour, IInputListener
{
    [SerializeField] private GameObject laptopPivot;
    [SerializeField] private Vector3 pivotRot = new Vector3(10, 0, 0);
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private GameObject cameraToMove;
    [SerializeField] private GameObject transRef;
    [SerializeField] private RotationComponent rotter;
    [SerializeField] private CanvasGroup rawGroup;
    [SerializeField] private PlayerController avatar;
    [SerializeField] private Camera avCam;
    [SerializeField] private GameObject playerListener;
    [SerializeField] private GameObject playerEnv;
    [SerializeField] private GameObject outriderListener;
    [SerializeField] private CanvasGroup slowFlash;
    [SerializeField] private PitOpenerComponent opener;
    [SerializeField] private CanvasGroup finalFade;
    [Space]
    [SerializeField] private GameObject datesOnGroup;
    [SerializeField] private GameObject datesOffGroup;
    [SerializeField] private Text dateText;
    [SerializeField] private GameObject datesCursorGroup;
    [SerializeField] private GameObject datesNoCursorGroup;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    private bool selectingDate;
    private bool mainCursorOnDates;
    private int dateIndex;
    private bool begun;

    public void Start()
    {
        InputManager.Instance.PushListener(this);
        StartCoroutine(CoUtils.RunTween(laptopPivot.transform.DORotate(pivotRot, 1.2f)));
        //StartCoroutine(SlowFlashRoutine());
    }

    private IEnumerator SlowFlashRoutine()
    {
        slowFlash.alpha = 0f;
        while (!begun)
        {
            yield return CoUtils.RunTween(slowFlash.DOFade(1f, .8f));
            yield return CoUtils.Wait(1f);
            yield return CoUtils.RunTween(slowFlash.DOFade(0f, .8f));
            yield return CoUtils.Wait(1f);
        }
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType)
    {
        switch (command)
        {
            case InputManager.Command.Primary:
                if (eventType != InputManager.Event.Up)
                {
                    return true;
                }
                if (selectingDate)
                {
                    LaunchBookmark();
                }
                else if (mainCursorOnDates)
                {
                    ShowDates(true);
                }
                else
                {
                    Advance();
                }
                break;
            case InputManager.Command.Right:
            case InputManager.Command.Down:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (eventType != InputManager.Event.Down)
                {
                    return true;
                }
                if (selectingDate)
                {
                    IncrDate(1);
                }
                else
                {
                    ToggleMainCursor();
                }
                break;
            case InputManager.Command.Left:
            case InputManager.Command.Up:
                if (eventType != InputManager.Event.Down)
                {
                    return true;
                }
                if (selectingDate)
                {
                    IncrDate(-1);
                }
                else
                {
                    ToggleMainCursor();
                }
                break;
            case InputManager.Command.Secondary:
            case InputManager.Command.Menu:
                ShowDates(false);
                break;
        }
        return true;
    }

    public void Update()
    {
        if (!begun)
        {
            avCam.transform.rotation = rotter.transform.rotation;
        }
    }

    private void Advance()
    {
        AudioManager.Instance.PlaySFX("menu/select", null, AudioManager.Bank.UI);
        InputManager.Instance.RemoveListener(this);
        Global.Instance.StartCoroutine(StartGameRoutine("PalMap", "start", OrthoDir.North));
    }

    public IEnumerator StartGameRoutine(string map, string target, OrthoDir dir)
    {
        avatar.body.useGravity = true;
        Global.Instance.Data.SetStringVariable("date", "Tuesday 10/1");
        Global.Instance.Data.SetStringVariable("time", "10:00 AM");

        Global.Instance.Data.SetSwitch("abseiling_disabled", true);
        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(DOTween.To(() => rotter.rot, val => rotter.rot = val, Vector3.zero, 2.5f)),
            CoUtils.RunTween(uiGroup.DOFade(0f, 2.5f)),
            AudioManager.Instance.FadeOutRoutine(2.5f));
        yield return CoUtils.Wait(.8f);
        avCam.enabled = true;

        outriderListener.SetActive(false);
        playerListener.SetActive(true);
        playerEnv.SetActive(true);

        begun = true;
        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(cameraToMove.transform.DOMove(transRef.transform.position, .5f)),
            CoUtils.RunTween(cameraToMove.transform.DORotate(transRef.transform.eulerAngles, .5f)));
        yield return CoUtils.RunTween(rawGroup.DOFade(0f, .7f));
        avatar.enabled = true;

        yield return CoUtils.Wait(12f);
        opener.enabled = true;
        StartCoroutine(CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(DOTween.To(() => opener.r, val => opener.r = val, 32f, 3f)),
            CoUtils.RunTween(DOTween.To(() => opener.rate, val => opener.rate = val, 3f, 6f))));
        yield return CoUtils.Wait(2.5f);
        avatar.PauseInput();
        avCam.transform.DORotate(new Vector3(-70f, avCam.transform.eulerAngles.y, 0f), .5f);
        yield return CoUtils.Wait(2f);
        yield return CoUtils.RunTween(finalFade.DOFade(1f, 2f));

        yield return SceneManager.LoadSceneAsync("Map3D", LoadSceneMode.Single);
        yield return Global.Instance.Maps.TeleportRoutine(map, target, dir);
        avatar.UnpauseInput();

        Global.Instance.Data.SetSwitch("abseiling_disabled", false);
    }

    private void ShowDates(bool show)
    {
        AudioManager.Instance.PlaySFX("menu/select", null, AudioManager.Bank.UI);
        datesOnGroup.SetActive(show);
        datesOffGroup.SetActive(!show);
        selectingDate = show;
    }

    private void IncrDate(int delta)
    {
        AudioManager.Instance.PlaySFX("menu/config_slider", null, AudioManager.Bank.UI);
        dateIndex += delta;
        if (dateIndex >= SwitchSets.Sets.Length)
        {
            dateIndex = 0;
        }
        if (dateIndex < 0)
        {
            dateIndex = SwitchSets.Sets.Length - 1;
        }
        dateText.text = SwitchSets.BookmarkDates[dateIndex];
        leftArrow.SetActive(dateIndex != 0);
        rightArrow.SetActive(dateIndex < SwitchSets.Sets.Length - 1);
    }

    private void LaunchBookmark()
    {
        AudioManager.Instance.StopENV();
        AudioManager.Instance.PlaySFX("menu/select", null, AudioManager.Bank.UI);
        InputManager.Instance.RemoveListener(this);
        Global.Instance.StartCoroutine(ResumeGameRoutine());
    }

    private void ToggleMainCursor()
    {
        AudioManager.Instance.PlaySFX("menu/up_down", null, AudioManager.Bank.UI);
        mainCursorOnDates = !mainCursorOnDates;
        datesCursorGroup.SetActive(mainCursorOnDates);
        datesNoCursorGroup.SetActive(!mainCursorOnDates);
    }

    private IEnumerator ResumeGameRoutine()
    {
        for (var i = 0; i <= dateIndex; i += 1)
        {
            foreach (var s in SwitchSets.Sets[i])
            {
                Global.Instance.Data.SetSwitch(s, true);
            }
        }
        Global.Instance.Data.SetStringVariable("date", SwitchSets.BookmarkDates[dateIndex]);
        Global.Instance.Data.SetStringVariable("time", SwitchSets.BookmarkTimes[dateIndex]);
        yield return CoUtils.RunTween(finalFade.DOFade(1f, 1f));

        var map = SwitchSets.MapNames[dateIndex];
        yield return SceneManager.LoadSceneAsync("Map3D", LoadSceneMode.Single);
        yield return Global.Instance.Maps.TeleportRoutine(map, "start");
        yield return CoUtils.RunTween(MapOverlayUI.Instance.fader.DOFade(0f, 1f));
    }
}
