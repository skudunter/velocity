using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject chunkPrefab; // Reference to the chunk prefab
    GameObject player; // Reference to the player GameObject
    float chunkSize = 100f; // Size of each chunk
    float unloadDistance = 150f; // Distance from the player to unload chunks
    int maxChunks = 9; // Maximum number of chunks to keep in the scene

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Assumes you have a "Player" tag assigned to the player GameObject
        SpawnChunks();
    }

    void SpawnChunks()
    {
        Vector3 playerPosition = player.transform.position;
        int currentChunkX = Mathf.FloorToInt(playerPosition.x / chunkSize);
        int currentChunkZ = Mathf.FloorToInt(playerPosition.z / chunkSize);

        int chunkCount = 0;

        for (int dz = -1; dz <= 1; dz++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int chunkX = currentChunkX + dx;
                int chunkZ = currentChunkZ + dz;

                Vector3 chunkPosition = new Vector3(chunkX * chunkSize, 0, chunkZ * chunkSize);
                float distanceToPlayer = Vector3.Distance(playerPosition, chunkPosition);

                if (distanceToPlayer <= unloadDistance && chunkCount < maxChunks)
                {
                    if (!ChunkExists(chunkX, chunkZ))
                    {
                        CreateChunk(chunkX, chunkZ, chunkPosition);
                        chunkCount++;
                    }
                }
                else
                {
                    DestroyChunk(chunkX, chunkZ);
                }
            }
        }
    }

    bool ChunkExists(int x, int z)
    {
        GameObject existingChunk = GameObject.Find("Chunk_" + x + "_" + z);
        return (existingChunk != null);
    }

    void CreateChunk(int x, int z, Vector3 position)
    {
        GameObject chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity);
        chunkObject.name = "Chunk_" + x + "_" + z;
        chunkObject.transform.parent = transform;
    }

    void DestroyChunk(int x, int z)
    {
        GameObject chunkObject = GameObject.Find("Chunk_" + x + "_" + z);
        if (chunkObject != null)
        {
            Destroy(chunkObject);
        }
    }

    void Update()
    {
        SpawnChunks();
    }
}
