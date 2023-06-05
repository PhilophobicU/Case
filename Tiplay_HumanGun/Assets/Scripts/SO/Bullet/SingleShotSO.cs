using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "Bullets/SingleShot")]
public class SingleShotSO : GunShootingPatternSO {
    public GameObject bullet;
    public GameObject crashParticle;
    public override void GetShootingStyle(Vector3 startPos,Vector3 endPos , Vector3 crashSpawnPoint ) {
        GameObject go = Instantiate(bullet, startPos, Quaternion.identity);
        
        go.transform.DOMove(endPos, 40f)
            .SetSpeedBased()
            .OnComplete(() => {
                Destroy(go);
                Instantiate(crashParticle, crashSpawnPoint, Quaternion.Euler(-180, 0, 0));
            });
    }
}
