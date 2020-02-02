using UnityEngine;

public class Log : MonoBehaviour, IItem {
    public void Use() {
        // TODO: Sound SE

        // TODO: Particle effect

        gameObject.SetActive(false);
    }

    public void Consume() {
        Destroy(gameObject);
    }
}
