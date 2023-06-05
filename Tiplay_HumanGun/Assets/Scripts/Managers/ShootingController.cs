using UnityEngine;

public class ShootingController : MonoBehaviour {

    private Transform _focus;
    private float _cooldown;
    [SerializeField] private LayerMask obstacleLayer;

    private void Update() {
        if (GameManager.Instance.IsGameEnded()) return;
        ShootingRay();
        FocusShootingObject();
    }
    private void FocusShootingObject() {
        if (_focus != null) {
            Vector3 diff = _focus.transform.position - transform.position;
            var lookAt = Quaternion.LookRotation(diff);
            transform.eulerAngles = new Vector3(0, ClampAngle(lookAt.eulerAngles.y, -5, 5), 0);
        }
    }

    private float ClampAngle(float val, float min, float max) {
        if (val is > 0 and < 180) {
            if (val > max) {
                _focus = null;
                return max;
            }
            return val;
        }
        if (val < 360 + min) {
            _focus = null;
            return 360 + min;
        }
        return 360 + val;
    }

    private void ShootingRay() {
        if (!GunTransfromController.Instance.Shootable()) return;
        float maxDistance = 6f;
        WeaponCooldown();
        Ray ray = new Ray(transform.position + (Vector3.up / 4), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, obstacleLayer)) {
            if (hit.collider.TryGetComponent(out IShootable shootable)) {
                if (_focus == null) _focus = shootable.ReturnTransfrom();
                if (shootable.Targetable()) StartShootingProcess(shootable, hit);
            }
        }
        else {
            _focus = null;
            transform.rotation = Quaternion.identity;
        }
    }
    private void StartShootingProcess(IShootable shootable, RaycastHit hit) {

        if (_cooldown <= 0) {
            _cooldown += GunTransfromController.Instance.GetWeaponCooldown();
            GunFire(hit);
            shootable.Hit(GunTransfromController.Instance.GetDamageAmount());
            GunTransfromController.Instance.Recoil();
        }
    }
    private void GunFire(RaycastHit hit) {
        GunTransfromController.Instance.GetList().fireStyle
            .GetShootingStyle(PlayerController.Instance.CurrentPos(),
                _focus.transform.position, hit.point);
    }
    private void WeaponCooldown() {

        if (_cooldown > 0) {
            _cooldown -= Time.deltaTime;
        }
    }


}