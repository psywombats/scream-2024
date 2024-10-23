using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    [SerializeField] private int count1;
    [SerializeField] private int count2;
    [SerializeField] private float slowDelay;
    [SerializeField] private float fastDelay;
    [SerializeField] private Vector2 slowBounds;
    [SerializeField] private Vector2 fastBounds;
    [SerializeField] private List<GameObject> objs;

    public void Start()
    {
        //StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        yield return Spawn(count1, slowBounds, slowDelay, true);
        yield return CoUtils.Wait(1);
        yield return Spawn(count2, fastBounds, fastDelay, false);
    }

    private IEnumerator Spawn(int count, Vector2 bounds, float delay, bool shake)
    {
        for (var i = 0; i < count; i += 1)
        {
            var toSpawn = objs[i % objs.Count];
            var clone = Instantiate(toSpawn);
            clone.transform.SetParent(transform);
            var rect = clone.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.localEulerAngles = Vector3.zero;
            rect.anchoredPosition = new Vector2(
                Random.Range(-bounds.x, bounds.x),
                Random.Range(-bounds.y, bounds.y));
            rect.transform.localPosition = new Vector3(rect.transform.localPosition.x, rect.transform.localPosition.y, 0);
            if (shake) StartCoroutine(CoUtils.RunTween(rect.transform.DOShakePosition(.2f)));
            yield return CoUtils.Wait(delay);
        }
    }
}