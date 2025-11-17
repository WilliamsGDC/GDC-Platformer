using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // needed for each level
    public string id;
    public List<string> connectedScenes = new();
}
