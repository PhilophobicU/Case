using UnityEngine;
[CreateAssetMenu()]
public abstract class GunShootingPatternSO : ScriptableObject
{
    public abstract void GetShootingStyle(Vector3 startPos,Vector3 endPos , Vector3 crashSpawnPoint);
}
