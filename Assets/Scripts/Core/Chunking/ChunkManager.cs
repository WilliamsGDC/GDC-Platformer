using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this should really be a singleton
public class ChunkManager : MonoBehaviour
{
    public Dictionary<string, Chunk> loadedChunks;

    public void LoadChunk(Chunk chunk, Vector3 chunkPosition)
    {
        string chunkId = chunk.chunkId;
        if (loadedChunks.Keys.Contains(chunkId)) return;

        loadedChunks[chunkId] = Instantiate(chunk, chunkPosition, Quaternion.identity);
    }

    public void UnloadChunk(Chunk chunk)
    {
        string chunkId = chunk.chunkId;
        if (!loadedChunks.Keys.Contains(chunkId)) return;
        loadedChunks[chunkId].OnUnloadChunk();
        Destroy(loadedChunks[chunkId].gameObject);
        loadedChunks.Remove(chunkId);
    }
}
