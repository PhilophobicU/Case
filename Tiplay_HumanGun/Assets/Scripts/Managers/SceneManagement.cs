using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour {

    public static SceneManagement Instance;
    private int[] _levels = { 2, 3, 4 };
    private int _levelIndex;
    private void Awake() {
        Instance = this;
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (IsTutorial()) return;
        _levelIndex = PlayerPrefs.GetInt("LEVEL_INDEX", 0);
        if (currentBuildIndex != _levels[_levelIndex]) {
            SceneManager.LoadScene(_levels[_levelIndex]);
        }
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene() {
        bool isTutorial = SceneManager.GetActiveScene().buildIndex == 1;
        if (isTutorial) {
            SceneManager.LoadScene(_levels[0]);
            return;
        }
        int nextLevel = (_levelIndex + 1) % 3;
        PlayerPrefs.SetInt("LEVEL_INDEX", nextLevel);
        SceneManager.LoadScene(_levels[nextLevel]);
    }
    public bool IsTutorial() {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        bool isCurrentLevelTutorial = currentBuildIndex == 1;
        return isCurrentLevelTutorial;
    }

}