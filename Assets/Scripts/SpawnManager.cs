using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public Material redMat, blueMat, greenMat;
    public float spawnInterval = 1.2f;
    public int maxActive = 20;
    public Vector2 spawnX = new Vector2(-9f, 9f);
    public Vector2 spawnZ = new Vector2(-9f, 9f);
    public float spawnY = 1f;
    public float lifetime = 15f; // collectibles auto-destroy after this time

    private List<GameObject> active = new List<GameObject>();
    private Material[] mats;

    void Start()
    {
        mats = new Material[] { redMat, blueMat, greenMat };
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn if under maxActive
            if (active.Count < maxActive)
                SpawnOne();
        }
    }

    void SpawnOne()
    {
        float x = Random.Range(spawnX.x, spawnX.y);
        float z = Random.Range(spawnZ.x, spawnZ.y);
        Vector3 pos = new Vector3(x, spawnY, z);

        GameObject go = Instantiate(collectiblePrefab, pos, Quaternion.identity);

        // Assign Collectible type
        Collectible col = go.GetComponent<Collectible>();
        if (col != null)
        {
            int idx = Random.Range(0, mats.Length);
            col.type = (Collectible.CollectibleType)idx;
        }

        // Assign material to Renderer
        Renderer rend = go.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = mats[(int)col.type];
        }
        else
        {
            // Try to find renderer in children
            rend = go.GetComponentInChildren<Renderer>();
            if (rend != null) rend.material = mats[(int)col.type];
        }

        active.Add(go);

        // Destroy after lifetime and remove from list
        Destroy(go, lifetime);
        StartCoroutine(RemoveFromListAfterDelay(go, lifetime));
    }

    IEnumerator RemoveFromListAfterDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        active.Remove(go);
    }
}
