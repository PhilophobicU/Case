using System;
using UnityEngine;

public class SellManager : MonoBehaviour {

    [SerializeField] private float timeBetweenSell;
    [SerializeField] private CollectController player;
    private float time;
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")) {
            time += Time.deltaTime;
            if(time > timeBetweenSell) {
                time -= timeBetweenSell;
                player.Sell(transform);
            }
        }
    }


}
