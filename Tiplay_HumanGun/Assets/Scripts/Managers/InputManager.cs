using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private float playerRunningSpeed;
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Vector2 minMaxBorder;
    private float _xPos;
    private Vector3 _dragStartPos, _endPos, _newPos;
    
    

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.IsGameStarted()) return;


        if (GameManager.Instance.IsGameStarted()) {
            objectToMove.position += playerRunningSpeed * Time.deltaTime * Vector3.forward;
        }
        if (Input.GetMouseButtonDown(0)) {

            _dragStartPos = Input.mousePosition;
            _xPos = objectToMove.transform.localPosition.x;
        }

        if (Input.GetMouseButton(0)) {
            _endPos = Input.mousePosition;
            _newPos.x = ((_endPos.x - _dragStartPos.x) / (Screen.width / 10f)) + _xPos;
            _newPos.x = Mathf.Clamp(_newPos.x, minMaxBorder.x, minMaxBorder.y);
            objectToMove.transform.localPosition = new Vector3(_newPos.x, objectToMove.transform.localPosition.y, objectToMove.transform.localPosition.z);
        }
    }
}
