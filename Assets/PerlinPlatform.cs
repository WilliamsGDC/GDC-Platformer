using UnityEngine;

public class PerlinPlatformGenerator : MonoBehaviour
{
    [Header("Perlin Noise Settings")]
    public int width = 100;
    public int height = 20;
    public float scale = 10f;
    public float threshold = 0.5f; // Light enough = above this value

    [Header("Tile Settings")]
    public GameObject tilePrefab; // Prefab with SpriteRenderer + BoxCollider2D
    public Transform parentContainer;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile prefab not assigned!");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                // Inverted: fill tile if Perlin noise value is above the threshold
                if (sample > threshold)
                {
                    Vector2 position = new Vector2(x, y);
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, parentContainer);
                    tile.layer = LayerMask.NameToLayer("Ground");
                    tile.name = $"Tile_{x}_{y}";
                }
            }
        }
    }
}
