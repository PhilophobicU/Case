using DG.Tweening;
using UnityEngine;

public class GemProducer : MonoBehaviour {
    [SerializeField] private Transform gemSpawnPoint;
    public Vector3 gemTransform => gemSpawnPoint.position;
    public bool emptyGemTile => _currentGem == null;
    private GemSO _currentGem;


    public void SetGem(GemSO inputGem) {
        ClearGem();
        _currentGem = inputGem;
        Gem gem = Instantiate(_currentGem.gemPrefab, gemTransform, Quaternion.identity,gemSpawnPoint);
        gem.SetProducer(this);
    }
    public void ClearGem() {
        _currentGem = null;
    }
    

}
