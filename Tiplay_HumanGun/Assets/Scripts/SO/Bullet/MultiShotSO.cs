using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "Bullets/MultiShot")]
public class MultiShotSO : GunShootingPatternSO {
    public GameObject bullet;
    public int bulletCount;
    public GameObject crashParticle;
    public Vector3[] rotationOffsets = new Vector3[4];
    public override void GetShootingStyle(Vector3 startPos,Vector3 endPos , Vector3 crashSpawnPoint) {
        for (int i = 0; i < bulletCount; i++) {
            GameObject go = Instantiate(bullet, startPos, Quaternion.identity);
            go.transform.DOMove(endPos + new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),0), 40f)
                .SetSpeedBased()
                .OnComplete(() => {
                    Destroy(go);
                    Instantiate(crashParticle, crashSpawnPoint, Quaternion.Euler(-180, 0, 0));
                });
        }
    }
}
