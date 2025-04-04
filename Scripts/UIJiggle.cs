using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJiggle : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float rotStrength;
    [SerializeField] private int rotRandomness;
    [SerializeField] private Vector3 endValue;
    [SerializeField] private float jumpPower;

    public void Start()
    {
        transform.DOShakeRotation(duration, rotStrength, rotRandomness).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuart);
        transform.DOJump(transform.position, jumpPower, 1, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
