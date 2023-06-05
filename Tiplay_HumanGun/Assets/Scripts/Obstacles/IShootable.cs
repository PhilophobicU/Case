using UnityEngine;
public interface IShootable {
    public bool Targetable();

    public Transform ReturnTransfrom();

    public void Hit(int damage);
}
