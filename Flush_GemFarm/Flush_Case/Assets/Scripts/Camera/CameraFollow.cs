using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform followedObject;

    private void Update() {
        FollowCamera();
    }

    private void OnValidate() {
        FollowCamera();
    }
    private void FollowCamera() {
        Vector3 nextPos = followOffset + followedObject.position;
        transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * followSpeed);
    }

}