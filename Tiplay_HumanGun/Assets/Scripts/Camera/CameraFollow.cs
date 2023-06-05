using System;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour {
    public static CameraFollow Instance;
    private Camera _camera;
    [SerializeField] private Transform followObject;
    [SerializeField] private float followSpeed;
    public CameraAnglePresets cameraSelection;
    [SerializeField] private CameraTypes[] cameraType;

    private void Awake() {
        Instance = this;
        
        if(_camera == null) {
            _camera = GetComponent<Camera>();
        }
        if(followObject == null) {
            followObject = FindObjectOfType<PlayerController>().transform.GetChild(0).transform;
        }
    }
    private void OnValidate() {
        if(followObject == null ) return;
        //transform.position = PresetPos((int)cameraSelection);
       // transform.rotation = Quaternion.Euler(PresetRotation((int)cameraSelection));
    }

    Vector3 PresetPos(int PresetType) {
        return new Vector3(cameraType[PresetType].position.x + followObject.position.x, cameraType[PresetType].position.y, followObject.position.z + cameraType[PresetType].position.z);
    }

    Vector3 PresetRotation(int PresetType) {
        return new Vector3(cameraType[PresetType].eulerRotations.x + followObject.rotation.x, cameraType[PresetType].eulerRotations.y, followObject.rotation.z + cameraType[PresetType].eulerRotations.z);
    }
    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, PresetPos((int)cameraSelection), Time.deltaTime * followSpeed);
        // transform.rotation = Quaternion.Euler(PresetRotation((int)cameraSelection));
    }

    public void LerpCameraPositionAndRotation(int previousPreset, int nextPreset,float duration) {
        Vector3 before = PresetPos(previousPreset);
        Vector3 next = PresetPos(nextPreset);
        Vector3 result = before - next;
        DOVirtual.Vector3(transform.localPosition, transform.localPosition -result, duration, v => {
            transform.localPosition = v;
        }).SetEase(Ease.InSine);
        
        Vector3 beforeRot = PresetRotation(previousPreset);
        Vector3 nextRot = PresetRotation(nextPreset);
        Vector3 resultRot = beforeRot - nextRot;
        DOVirtual.Vector3(transform.eulerAngles, transform.eulerAngles -resultRot, duration, v => {
            transform.localRotation =  Quaternion.Euler(v.x, v.y, v.z);
        }).SetEase(Ease.InSine);
    }
    private void LerpFov(float FirstState,float SecondState,float duration) {
        if (Math.Abs(FirstState - SecondState) < 0.1) return;
        DOVirtual.Float(FirstState, SecondState, duration, v => {
            _camera.fieldOfView = v;
        }).SetEase(Ease.InSine);
    }
}
[Serializable]
public struct CameraTypes {
    public Vector3 position;
    public Vector3 eulerRotations;
}
public enum CameraAnglePresets {
    BeforeStart,
    Playing,
    End
}