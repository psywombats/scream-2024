using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EndGameComponent : MonoBehaviour
{
    [SerializeField] private float fadeDuration = .7f;
    [SerializeField] private float closeDuration = .5f;
    [SerializeField] private CanvasGroup rawGroup;
    [SerializeField] private Camera cameraToMove;
    [SerializeField] private Camera transRef;
    [SerializeField] private GameObject laptopPivot;
    [SerializeField] private Vector3 pivotRot = new Vector3(-90, 0, 0);

    public void OnEnable()
    {
        EndGame();
    }

    private void EndGame()
    {
        Global.Instance.StartCoroutine(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine()
    {
        yield return CoUtils.RunParallel(Global.Instance,
            AudioManager.Instance.FadeOutRoutine(fadeDuration),
            CoUtils.RunTween(rawGroup.DOFade(0f, fadeDuration)));

        yield return CoUtils.RunParallel(Global.Instance,
            CoUtils.RunTween(laptopPivot.transform.DORotate(pivotRot, 1.2f)),
            CoUtils.RunTween(cameraToMove.transform.DOMove(transRef.transform.position, closeDuration)),
            CoUtils.RunTween(cameraToMove.transform.DORotate(transRef.transform.eulerAngles, closeDuration)));
    }
}
