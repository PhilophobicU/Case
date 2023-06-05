using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class GunTransfromController : MonoBehaviour {

    public static GunTransfromController Instance;

    private int _pistolBaseDamage;
    private int _smgBaseDamage;
    private int _shotgunBaseDamage;
    [SerializeField] private Material defaultMaterial;
    private void Start() {
        GameManager.Instance.OnGameEndWithSuccess += GameManagerOnGameEndWithSuccessDestroyAll;
    }
    public bool Shootable() {
       return _runners.Count > 0;
    }

    private void GameManagerOnGameEndWithSuccessDestroyAll(object sender, EventArgs e) {
        var lastMembers = LastMembers(_runners.Count -1);
        foreach (Transform t in lastMembers) {
            t.parent = null;
            RemoveFromRunnersGroup(lastMembers);
            t.GetComponent<CollectibleBots>().FallFromPlayer();
        }
        BackToNormal();
    }

    private void Awake() {
        Instance = this;
        _runners = new List<Transform>();
    }

    [SerializeField] private List<Transform> _runners;
    [SerializeField] private List<GunPatternsSO> gunPatternsSO;
    private bool _shootable;
    public void Recoil() {

        var gun = GetList();
        if (_shootable) {
            Debug.LogError("Recoil Call more then complete time");
        }
        if (!_shootable) {
            _shootable = true;
            float recoilDuration = 0.1f;
            float recoilDampen = gun.dampenAmount;
            if (PlayerController.Instance.RecoilCheck()) {
                for (int i = 0; i < gun.specifyRecoilSticksmans.Length; i++) {
                    if (PlayerController.Instance.HasChild(gun.specifyRecoilSticksmans[i])) {
                        _runners[gun.specifyRecoilSticksmans[i]].GetComponent<CollectibleBots>().Recoil(recoilDampen, recoilDuration);
                    }
                    else {
                        break;
                    }
                }
            }
            float recoilTurnAmount = -10;
            PlayerController.Instance.transform.DOLocalRotate(new Vector3(recoilTurnAmount, 0, 0), .05f).OnComplete(() => { PlayerController.Instance.transform.DOLocalRotate(Vector3.zero, .01f); });
        }
        _shootable = false;
    }

    public void ReOrderList(GunPatternsSO list) {
        for (int i = 0; i < _runners.Count; i++) {
            _runners[i].GetComponent<Animator>().SetTrigger(Animation_Type(list, i));
            _runners[i].transform.DOLocalMove(list.stickmans[i].localPositions, .75f);
            _runners[i].transform.DOLocalRotate(list.stickmans[i].eulerAngles, .4f).OnComplete(() => { _runners[i].transform.DOScale(1.25f, .4f).OnComplete(() => { _runners[i].transform.DOScale(1f, .4f); }); });
            _runners[i].GetChild(1).GetComponent<SkinnedMeshRenderer>().material = list.stickmans[i].color;
        }
    }

    private void AddToList(GunPatternsSO list, int i) {
        _runners[i].GetComponent<Animator>().SetTrigger(Animation_Type(list, i));
        _runners[i].transform.DOLocalJump(list.stickmans[i].localPositions, .25f, 1, .4f);
        _runners[i].transform.DOLocalRotate(list.stickmans[i].eulerAngles, .4f).OnComplete(() => { _runners[i].transform.DOScale(1.25f, .4f).OnComplete(() => { _runners[i].transform.DOScale(1f, .4f); }); });
        _runners[i].GetChild(1).GetComponent<SkinnedMeshRenderer>().material = new Material(list.stickmans[i].color);
    }

    public int StickmanListCount() {
        return _runners.Count;
    }

    public void AddToRunnersGroup(Transform stickmanTransform) {
        var list = GetList();
        if (!_runners.Contains(stickmanTransform)) {
            stickmanTransform.gameObject.layer = 8;
            _runners.Add(stickmanTransform);
            stickmanTransform.parent = PlayerController.Instance.transform;
            AddToList(GetList(), _runners.Count - 1);
            if (list != GetList()) {
                ReOrderList(GetList());
            }
        }
    }
    
    public GunPatternsSO GetList() {
        if (_runners.Count <= gunPatternsSO[0].stickmans.Count) return gunPatternsSO[0];
        if (_runners.Count > gunPatternsSO[0].stickmans.Count && _runners.Count <= gunPatternsSO[1].stickmans.Count) return gunPatternsSO[1];
        return gunPatternsSO[2];
    }

    public Transform[] LastMembers(int howManyFallFromPlayer) {
        var list = new Transform[howManyFallFromPlayer];
        var index = 0;
        for (int i = _runners.Count - 1; i >= _runners.Count - howManyFallFromPlayer; i--) {
            list[index] = _runners[i].transform;
            index++;
        }
        return list;
    }
    
    
    public void RemoveFromRunnersGroup(Transform[] stickmanTransforms) {
        foreach (Transform t in stickmanTransforms) {
            if (_runners.Contains(t)) {
                _runners.Remove(t);
            }
        }
        ReOrderList(GetList());
    }

    public float GetWeaponCooldown() {
        return GetList().weaponCooldown;
    }

    private string[] _animationStrings = {
        "stand", "bend", "leghalfbend", "bendknee", "leftup", "leftbendup", "bow", "rightup", "rightbendup", "bowback", "leanforward", "halfbendwhead"
    };

    private string Animation_Type(GunPatternsSO type, int i) {
        return _animationStrings[(int)type.stickmans[i].animation];
    }

    public int GetDamageAmount() {
        return GetList().weaponDamage;
    }
    private void BackToNormal() {
        _runners[0].GetComponent<Animator>().SetTrigger("idle");
        _runners[0].transform.rotation = Quaternion.Euler(0,180,0);
        _runners[0].GetChild(1).GetComponent<SkinnedMeshRenderer>().material = defaultMaterial;
    }
}