using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Red, Blue, Green }
    public CollectibleType type;
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
