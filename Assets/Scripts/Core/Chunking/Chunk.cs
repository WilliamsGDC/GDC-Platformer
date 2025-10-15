using System.Collections.Generic;
using UnityEngine;

// each chunk should extend this class
public abstract class Chunk : MonoBehaviour
{
    public string chunkId; // this is only for good measure it isnt used anywhere.

    public abstract void OnLoadChunk(); 
    public abstract void OnUnloadChunk();
}
