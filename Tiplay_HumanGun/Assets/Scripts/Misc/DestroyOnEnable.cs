using UnityEngine;
public class DestroyOnEnable : MonoBehaviour {

    private void OnEnable() {
        Destroy(this.gameObject,2f);
    }

}