using UnityEngine;

[CreateAssetMenu]
public class GemSO : ScriptableObject {

    public string gemName;
    public int startingSellValue;
    public Sprite gemIcon;
    public Gem gemPrefab;
    public int collectedAmount;

}