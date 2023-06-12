using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Joystick input;
    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform playerBag;
    public Vector3 direction => new Vector3(input.Direction.x, 0, input.Direction.y);
    

    private void Update() {
        if (input.Direction != Vector2.zero) {
            float speedAdjustment = Time.deltaTime * playerSpeed;
            transform.position += direction * speedAdjustment;
            transform.forward = direction;
        }
    }
}