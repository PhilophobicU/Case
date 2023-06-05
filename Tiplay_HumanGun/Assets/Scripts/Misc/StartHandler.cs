using UnityEngine;
using UnityEngine.SceneManagement;
public class StartHandler : MonoBehaviour {

    private int _isTutorialPlayed;
    private void Awake() {
        _isTutorialPlayed = _isTutorialPlayed = PlayerPrefs.GetInt("TUTORIAL", 0);
    }
    private void Start() {
        SceneManager.LoadScene(_isTutorialPlayed == 0 ? 1:2);
    }
    

}