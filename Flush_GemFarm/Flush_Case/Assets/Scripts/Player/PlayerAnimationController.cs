using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private const string Speed = "speed";

    private Animator _animator;
    private PlayerController _playerController;

    void Awake() {
        _animator = GetComponent<Animator>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update() {
            _animator.SetFloat(Speed, _playerController.direction.magnitude);
    }
}