using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    [SerializeField] private Animator animator;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    [SerializeField] private LayerMask progressLayer;
    [SerializeField] private LayerMask stickmanLayer;
    public int allowedTransformCount;
    private void Awake() {
        Instance = this;
    }

    public bool IsLevelLimitReached() {
        return GunTransfromController.Instance.StickmanListCount()  < allowedTransformCount; 
    }

    public int DifferenceBetweenRunnersAndAllowed() {
        return allowedTransformCount - GunTransfromController.Instance.StickmanListCount();
    }

    private void Update() {
        PlayerCollectRaycast();
    }

    public void RunAnimation(bool state) {
        animator.SetBool(IsRunning, state);
    }

    private void PlayerCollectRaycast() {
        float maxDistance = 1f;
        Ray ray = new Ray(transform.position + (Vector3.up / 4), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance)) {
            if (hit.collider.TryGetComponent(out CollectibleBots collectibleBots)) {
                if(!IsLevelLimitReached() && collectibleBots.gameObject.layer != 8) {
                    UIManager.Instance.CollectMoneyWorldToUI(collectibleBots.transform,collectibleBots.convertedMoneyAmount);
                    collectibleBots.gameObject.layer = 2;
                    Destroy(collectibleBots.gameObject);
                    return;
                }
                if(IsLevelLimitReached()) {
                    bool firstTime = GunTransfromController.Instance.StickmanListCount() < 2;
                    if (firstTime) {
                        GunTransfromController.Instance.AddToRunnersGroup(collectibleBots.transform);
                        GunTransfromController.Instance.AddToRunnersGroup(transform.GetChild(0));
                        RunningAnimationOff();
                        return;
                    }
                    GunTransfromController.Instance.AddToRunnersGroup(collectibleBots.transform);
                }
            }
            if(hit.collider.TryGetComponent(out Gate gate)) {
                gate.Interact();
            }
        }
    }
    public void RunningAnimationOff() {

        animator.SetBool(IsRunning, false);
    }

    public Vector3 CurrentPos() {
        return transform.position + (Vector3.up / 4);
    }
    
    public bool RecoilCheck() {
        return transform.childCount > 1;
    }
    public bool HasChild(int i) {
        return i < transform.childCount;
    }
    public int GetProgressData() {
        Ray ray = new Ray(transform.position+ transform.up,Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 3,progressLayer)) {
            if (hit.collider.TryGetComponent(out Progress progress)) {

                return progress.meter;
            }
        }
        return 1;
    }
}