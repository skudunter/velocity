using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject chunkPrefab; // Reference to the chunk prefab

    GameObject chunkObject;

    void Start()
    {
        chunkObject = Instantiate(chunkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        chunkObject.transform.parent = this.transform;
    }
}
