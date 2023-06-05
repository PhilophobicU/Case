using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BestScoreTable : MonoBehaviour {

    private int _bestScore;
    private float _zAmount;
    private TMP_Text _yourBestTxt;
    private void Awake() {
        _yourBestTxt = GetComponentInChildren<TextMeshPro>();
        _bestScore = PlayerPrefs.GetInt("BEST_SCORE", 10);
        _zAmount = PlayerPrefs.GetFloat("TABLE_Z", 61.67f);
        _yourBestTxt.text = $"YOUR BEST {_bestScore}M";
        transform.position = new Vector3(transform.position.x, transform.position.y, _zAmount);
    }
    void Start() {
        GameManager.Instance.OnGameEndWithSuccess += GameManagerOnGameEndWithSuccessBestScore;
    }

    private void GameManagerOnGameEndWithSuccessBestScore(object sender, EventArgs e) {
        Vector3 boardPos = new Vector3(transform.position.x, transform.position.y, 5 + PlayerController.Instance.transform.position.z);
        if(PlayerController.Instance.GetProgressData() > _bestScore) {
            transform.DOMove(boardPos, 1f);
            _bestScore = PlayerController.Instance.GetProgressData();
            PlayerPrefs.SetFloat("TABLE_Z", boardPos.z);
            PlayerPrefs.SetInt("BEST_SCORE",_bestScore);
        }
        _yourBestTxt.text = $"YOUR BEST {_bestScore}M";
    }


}