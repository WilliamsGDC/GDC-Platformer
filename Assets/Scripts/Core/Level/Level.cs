using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this should be attatched to the bounding box of each level
public class Level : MonoBehaviour
{
    [Header("This should include ALL scenes that should be loaded when the player is in this level, including itself!")]
    public List<string> connectedSceneNames = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.QueueLoadSurroundingLevels(this);
        }
    }
}
