using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutinComponent : MonoBehaviour
{
    [SerializeField] private GameObject mover;
    [SerializeField] private Image cutter;
    [SerializeField] private CanvasGroup group;
    [Space]
    [SerializeField] private float nvlDuration = .4f;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float moveOff = 64;

    public IEnumerator DoCutin(string cutinKey)
    {
        var cutin = IndexDatabase.Instance.Cutins.GetData(cutinKey);
        cutter.sprite = cutin.sprite;
        cutter.SetNativeSize();
        var nvl = MapOverlayUI.Instance.nvl;
        var nvlActive = nvl.IsShown;

        if (nvlActive)
        {
            yield return CoUtils.RunTween(nvl.GetComponent<CanvasGroup>().DOFade(0f, nvlDuration));
            nvl.Wipe();
            yield return CoUtils.Wait(.5f);
        }
        var spot = mover.transform.localPosition;

        yield return CoUtils.RunParallel(this,
            CoUtils.RunTween(group.DOFade(1f, transitionDuration)),
            CoUtils.RunTween(mover.transform.DOLocalMoveX(mover.transform.localPosition.x - moveOff, transitionDuration).From()));
        
        yield return InputManager.Instance.ConfirmRoutine();

        yield return CoUtils.RunParallel(this,
            CoUtils.RunTween(group.DOFade(0f, transitionDuration)),
            CoUtils.RunTween(mover.transform.DOLocalMoveX(mover.transform.localPosition.x + moveOff, transitionDuration)));

        mover.transform.localPosition = spot;
        if (nvlActive)
        {
            yield return CoUtils.Wait(.5f);
            yield return CoUtils.RunTween(nvl.GetComponent<CanvasGroup>().DOFade(1f, nvlDuration));
            yield return CoUtils.Wait(.5f);
        }
    }
}
