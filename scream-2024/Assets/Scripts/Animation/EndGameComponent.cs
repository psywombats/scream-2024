using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameComponent : MonoBehaviour
{
    [SerializeField] private float fadeDuration = .7f;
    [SerializeField] private float closeDuration = .5f;
    [SerializeField] private float closeDuration2 = 1.5f;
    [SerializeField] private CanvasGroup rawGroup;
    [SerializeField] private Camera cameraToMove;
    [SerializeField] private Camera transRef;
    [SerializeField] private GameObject laptopPivot;
    [SerializeField] private Vector3 pivotRot = new Vector3(-90, 0, 0);
    [SerializeField] private Light pointLight;
    [SerializeField] private Light lapLight;
    [Space]
    [SerializeField] private RenderTexture lapTex;
    [SerializeField] private RenderTexture transTex;
    [SerializeField] private GameObject outrider1;
    [SerializeField] private GameObject outrider2;
    [SerializeField] private CanvasGroup rawGroup2;
    [SerializeField] private Camera cameraToMove2;
    [SerializeField] private Camera transRef2;
    [SerializeField] private GameObject laptopPivot2;
    [SerializeField] private Light pointLight2;
    [SerializeField] private Light lapLight2;
    [Space]
    [SerializeField] private TextSpawner spawner;
    [SerializeField] private CanvasGroup texterGroup;
    [SerializeField] private CanvasGroup stingerGroup;
    [SerializeField] private Text stingerA;
    [SerializeField] private Text stingerB;
    [SerializeField] private Text credits1;
    [SerializeField] private Text credits2;
    [SerializeField] private Text credits3;
    [SerializeField] private Text credits4;
    [SerializeField] private Text credits5;
    [SerializeField] private Text credits6;
    [SerializeField] private Text credits7;

    public void OnEnable()
    {
        EndGame();
    }

    private void EndGame()
    {
        Global.Instance.Maps.ActiveMap.gameObject.SetActive(false);
        Global.Instance.StartCoroutine(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine()
    {
        Global.Instance.Audio.PlayBGM("drone");
        yield return CoUtils.RunParallel(Global.Instance,
            AudioManager.Instance.FadeOutRoutine(fadeDuration),
            CoUtils.RunTween(rawGroup.DOFade(0f, fadeDuration)));

        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(laptopPivot.transform.DORotate(pivotRot, closeDuration)),
            CoUtils.RunTween(pointLight.DOIntensity(0f, closeDuration)),
            CoUtils.RunTween(lapLight.DOIntensity(0f, closeDuration)),
            CoUtils.RunTween(cameraToMove.transform.DOMove(transRef.transform.position, closeDuration)),
            CoUtils.RunTween(cameraToMove.transform.DORotate(transRef.transform.eulerAngles, closeDuration)));
    }

    public IEnumerator EndGame2Routine()
    {
        Global.Instance.Audio.PlayBGM("ending_music");
        cameraToMove.enabled = false;
        cameraToMove.targetTexture = lapTex;
        cameraToMove.Render();
        cameraToMove.targetTexture = transTex;
        cameraToMove.Render();

        outrider1.SetActive(false);
        outrider2.SetActive(true);

        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(rawGroup2.DOFade(0f, fadeDuration)));

        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(laptopPivot2.transform.DORotate(pivotRot, closeDuration2)),
            CoUtils.RunTween(pointLight2.DOIntensity(0f, closeDuration2)),
            CoUtils.RunTween(lapLight2.DOIntensity(0f, closeDuration2)),
            CoUtils.RunTween(cameraToMove2.transform.DOMove(transRef2.transform.position, closeDuration2)),
            CoUtils.RunTween(cameraToMove2.transform.DORotate(transRef2.transform.eulerAngles, closeDuration2)));

        yield return CoUtils.Wait(1.5f);
        yield return spawner.Spawn();

        yield return CoUtils.Wait(2f);
        yield return CoUtils.RunTween(stingerA.DOFade(1f, .5f));
        yield return CoUtils.Wait(2f);
        yield return CoUtils.RunTween(stingerB.DOFade(1f, .5f));
        yield return CoUtils.Wait(4f);
        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(texterGroup.DOFade(0f, 2)),
            CoUtils.RunTween(stingerGroup.DOFade(0f, 2)));
        yield return CoUtils.Wait(2f);

        yield return CoUtils.RunTween(credits1.DOFade(1f, .5f));
        yield return CoUtils.Wait(.5f);
        yield return CoUtils.RunTween(credits2.DOFade(1f, .5f));
        yield return CoUtils.Wait(.2f);
        yield return CoUtils.RunTween(credits3.DOFade(1f, .5f));
        yield return CoUtils.Wait(.2f);
        yield return CoUtils.RunTween(credits4.DOFade(1f, .5f));
        yield return CoUtils.Wait(.2f);
        yield return CoUtils.RunTween(credits5.DOFade(1f, .5f));
        yield return CoUtils.Wait(1.5f);
        yield return CoUtils.RunTween(credits6.DOFade(1f, .5f));
        yield return CoUtils.Wait(1.5f);
        yield return CoUtils.RunTween(credits7.DOFade(1f, 1.5f));

        yield return InputManager.Instance.ConfirmRoutine();
        yield return SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
    }
}
