using System.Collections.Generic;
using UnityEngine;

// the actual bounding box which loads the scenes through chunkmanager
[RequireComponent(typeof(Collider2D))]
public class ChunkLoader : MonoBehaviour
{
    public ChunkManager chunkManager;

    public Chunk targetChunk;
    public Transform chunkPosition;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.CompareTag("Player")) return;
        chunkManager.LoadChunk(targetChunk, chunkPosition.position);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.transform.CompareTag("Player")) return;
        chunkManager.UnloadChunk(targetChunk);
    }
}
