using System;
using DG.Tweening;
using UnityEngine;

public class CollectController : MonoBehaviour {
    [SerializeField] private Transform playerBag;
    public event Action<Gem> OnSell;

    

    public void Sell(Transform t) {
        if(playerBag.childCount == 0) return;
        Gem gem = playerBag.GetChild(playerBag.childCount - 1).GetComponent<Gem>();
        StatManager.Instance.IncreaseStat(gem.GemType);
        OnSell?.Invoke(gem);
        gem.transform.parent = t;
        gem.transform.DOLocalJump(Vector3.zero, 3, 1, 1);
        gem.transform.DOScale(0, 2f);
        Destroy(gem.gameObject,2f);
    }
}

//6P  8G 2Y