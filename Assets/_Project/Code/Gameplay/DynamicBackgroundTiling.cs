using UnityEngine;

public class DynamicBackgroundTiling : MonoBehaviour
{
    public GameObject[] backgroundTiles;
    public Transform player;
    public float tileWidth = 10.24f;
    public float buffer = 10f;

    void Update()
    {
        RepositionTiles();
    }

    void RepositionTiles()
    {
        foreach (GameObject tile in backgroundTiles)
        {
            float distance = tile.transform.position.x - player.position.x;

            if (distance > tileWidth / 2 + buffer)
            {
                tile.transform.position -= new Vector3(tileWidth * backgroundTiles.Length, 0, 0);
            }
            else if (distance < -tileWidth / 2 - buffer)
            {
                tile.transform.position += new Vector3(tileWidth * backgroundTiles.Length, 0, 0);
            }
        }
    }
}