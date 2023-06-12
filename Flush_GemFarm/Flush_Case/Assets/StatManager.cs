using System;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public static StatManager Instance;
    public CollectedStats[] stats;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void IncreaseStat(GemSO type) {

        for (int i = 0; i < stats.Length; i++) {
            if(type.gemName ==  stats[i].GemType.gemName) {
                stats[i].collectedAmount++;
                break;
            }
        }
    }

}

[Serializable]
public struct CollectedStats {
    public GemSO GemType;
    public int collectedAmount;
}