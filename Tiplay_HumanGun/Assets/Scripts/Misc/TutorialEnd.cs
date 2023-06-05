using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PlayerPrefs.SetInt("TUTORIAL", 1);
    }
}
