using System.Collections.Generic;
using UnityEngine;

// a chunk with no behavior on load/unload
public class NormalChunk : Chunk
{
    public override void OnLoadChunk() { }
    public override void OnUnloadChunk() { }
}
