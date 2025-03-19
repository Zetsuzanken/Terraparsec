using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject[] backgroundTiles;
    public float tileWidth = 19.2f;
    public float buffer = 19.2f;

    public float scrollSpeed = 2f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam != null)
        {
            mainCam.transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f);
            RepositionTiles();
        }
    }

    private void RepositionTiles()
    {
        if (mainCam == null)
        {
            return;
        }

        Vector3 camPos = mainCam.transform.position;

        foreach (GameObject tile in backgroundTiles)
        {
            Vector3 tilePos = tile.transform.position;
            tilePos.y = camPos.y;
            tile.transform.position = tilePos;

            float distanceX = tile.transform.position.x - camPos.x;

            if (distanceX > (tileWidth / 2f) + buffer)
            {
                tile.transform.position -= new Vector3(tileWidth * backgroundTiles.Length, 0f, 0f);
            }
            else if (distanceX < (-tileWidth / 2f) - buffer)
            {
                tile.transform.position += new Vector3(tileWidth * backgroundTiles.Length, 0f, 0f);
            }
        }
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OnExitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
