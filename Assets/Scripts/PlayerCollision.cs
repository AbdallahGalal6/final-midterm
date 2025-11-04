using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerCollision : MonoBehaviour
{
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to collectibles
        if (!other.CompareTag("Collectible"))
            return;

        // Get the Collectible component
        Collectible collectible = other.GetComponent<Collectible>();
        if (collectible == null)
            return;

        // Check if the collected color matches the current target
        bool correct = collectible.type == GameManager.Instance.currentTarget;

        if (correct)
        {
            GameManager.Instance.UpdateScore(true);
            if (correctSFX != null)
                audioSource.PlayOneShot(correctSFX);
            Debug.Log("✅ Correct color collected!");
        }
        else
        {
            GameManager.Instance.UpdateScore(false);
            if (wrongSFX != null)
                audioSource.PlayOneShot(wrongSFX);
            Debug.Log("❌ Wrong color collected!");
        }

        // Remove the collected object
        Destroy(other.gameObject);
    }
}
