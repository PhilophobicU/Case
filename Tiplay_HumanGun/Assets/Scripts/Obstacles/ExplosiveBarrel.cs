using Unity.Mathematics;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour,IShootable
{
    [SerializeField]
    private int value;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private GameObject[] effectedObjects;
    private void Start() {
        value = 1;
    }
    public bool Targetable() {
        return value >= 1;
    }

    public Transform ReturnTransfrom() {
        return transform;
    }

    public void Hit(int damage) {
        value -= damage;
        ParticleSystem p = Instantiate(particleSystem,transform.position,quaternion.identity);
        if (value <= 0) {
            foreach (GameObject g in effectedObjects) {
                Destroy(g);
            }
            Destroy(this.gameObject);
        }
        Destroy(p.gameObject, 5f);
    }
}
