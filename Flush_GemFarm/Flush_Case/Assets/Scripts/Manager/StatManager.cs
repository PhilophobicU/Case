using System;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public static StatManager Instance;
    public GemSO[] stats;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void IncreaseStat(GemSO type) {

        for (int i = 0; i < stats.Length; i++) {
            if(type.gemName ==  stats[i].gemName) {
                stats[i].collectedAmount++;
                break;
            }
        }
    }
    public void ResetData() {
        foreach (GemSO inst in stats) {
            inst.collectedAmount = 0;
        }
        for (int i = 0; i < stats.Length; i++) {
            PlayerPrefs.SetInt(stats[i].gemName,0);
        }
        PlayerPrefs.SetInt("TOTAL_GOLD",0);
    }
    
    

}