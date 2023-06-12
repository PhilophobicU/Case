using System;
using DG.Tweening;
using UnityEngine;

public class Gem : MonoBehaviour {

    [SerializeField] private GemSO gemType;
    private GemProducer _producer;
    public GemSO GemType => gemType;
    private bool _collected;
    private bool _collectible => transform.localScale.x > .25f;
    private BoxCollider _collider;
    private Tween r;
    private void Awake() {
        _collider = GetComponent<BoxCollider>();
    }

    public void SetProducer(GemProducer t) {
        transform.localScale = Vector3.zero;
        r = transform.DOScale(1, 5f);
        _producer = t;
    }
    private void Update() {
        if (!_collected &&_collectible && !_collider.enabled)
            _collider.enabled = true;

    }
    private void OnTriggerEnter(Collider other) {
        // StatManager.Instance.IncreaseStat(gemType);
        if (other.CompareTag("Player")) {
            _collected = true;
            _collider.enabled = false;
            r.Kill();
            Transform bagTransform = other.transform.GetChild(1);
            transform.parent = bagTransform;

            transform.DOLocalJump(new Vector3(0,(bagTransform.childCount-1)*.2f,0), 5, 1, 1);
            
            // jump on player
            _producer.ClearGem();
        }
    }
}