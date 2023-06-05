using System;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    private void Awake() {
        Instance = this;
    }
    public static CameraHandler Instance { get; set; }

    [SerializeField] private GameObject[] Cameras;
    public void Switch(CameraType type) {
        
        for (int i = 0; i < 3 ; i++) {
            Cameras[i].SetActive((int)type == i);
        }
    }
}

public enum CameraType {
    Start,Playing,End
}