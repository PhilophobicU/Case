using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu()]
public class GunPatternsSO : ScriptableObject {
    public int weaponDamage;
    public float weaponCooldown;
    public int[] specifyRecoilSticksmans;
    public int dampenAmount;
    public List<StickmanPosRot> stickmans;
    public GunShootingPatternSO fireStyle;
    [Serializable]
    public struct StickmanPosRot {
        public Vector3 localPositions;
        public Vector3 eulerAngles;
        public Material color;
        public AnimationType animation;
    }
}

public enum AnimationType {
    Stand,
    Bend,
    LegHalfBend,
    BendKnee,
    LeftArmUp,
    LeftArmUpBend,
    HeadBow,
    RightArmUp,
    RightArmUpBend,
    HeadBowBack,
    LeanForward,
    HalfBendWithHead
}