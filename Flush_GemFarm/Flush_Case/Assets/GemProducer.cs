using DG.Tweening;
using UnityEngine;

public class GemProducer : MonoBehaviour {
    [SerializeField] private Transform gemSpawnPoint;
    public Vector3 gemTransform => gemSpawnPoint.position;
    public bool emptyGemTile => _currentGem == null;
    private Gem _currentGem;


    public void SetGem(Gem inputGem) {
        ClearGem();
        _currentGem = inputGem;
        Gem gem = Instantiate(_currentGem, gemSpawnPoint.position, Quaternion.identity,gemSpawnPoint);
        gem.SetProducer(this);
    }
    public void ClearGem() {
        _currentGem = null;
    }
    

}
