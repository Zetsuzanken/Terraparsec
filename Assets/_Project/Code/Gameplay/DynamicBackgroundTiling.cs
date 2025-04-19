using UnityEngine;

public class DynamicBackgroundTiling : MonoBehaviour
{
    public GameObject[] backgroundTiles;
    public float tileWidth = 19.2f;
    public float buffer = 19.2f;

    private void Update()
    {
        RepositionTiles();
    }

    private void RepositionTiles()
    {
        Vector3 camPos = Camera.main.transform.position;

        foreach (GameObject tile in backgroundTiles)
        {
            Vector3 tilePos = tile.transform.position;
            tilePos.y = camPos.y;
            tile.transform.position = tilePos;

            float distanceX = tile.transform.position.x - camPos.x;

            if (distanceX > (tileWidth / 2) + buffer)
            {
                tile.transform.position -= new Vector3(tileWidth * backgroundTiles.Length, 0, 0);
            }
            else if (distanceX < (-tileWidth / 2) - buffer)
            {
                tile.transform.position += new Vector3(tileWidth * backgroundTiles.Length, 0, 0);
            }
        }
    }
}
