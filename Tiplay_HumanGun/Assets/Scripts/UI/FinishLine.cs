using UnityEngine;
public class FinishLine : MonoBehaviour {

    [SerializeField] private ParticleSystem confetti;
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PlayerController playerController)) {
            confetti.Play();
        }
    }

}