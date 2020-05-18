using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameManager generate Random Platforms call GenreateRandomPlatform function;
    // New platforms are randomly generated whenever the player's position reaches 20 * idx heights.
    // Retry Function reset idx to 0 and call GenreateRandomPlatform(0, 40)

    // objectPooling for Get Objects
    public ObjectPooling objectPooling;

    // Player var for check position.y
    public Player player;

    // Camera var for check height to Random Ganerate Platforms
    public Camera cam;

    // Height var
    private int idx = 0;

    // Fisrt, We need to generate Random Platforms
    private void Start()
    {
        GenreateRandomPlatform(0, 40);
    }

    private void Update()
    {
        // player false -> NO update
        if(player.gameObject.activeSelf && (int)(cam.transform.position.y) / 20 != idx)
        {
            idx++;
            GenreateRandomPlatform((idx + 1) * 20, (idx + 2) * 20);
        }
    }

    private void GenreateRandomPlatform(int yStart, int yEnd)
    {
        for(int i = yStart; i < yEnd; i = i + 4)
        {
            int randomNum = Random.Range(0, 4);
            GameObject obj = null;

            // MovingPlatform
            if (randomNum == 0)
            {
                obj = objectPooling.GetObject("MovingPlatform");
                obj.transform.position = new Vector2(Random.Range(-7f, 7f), i);

                Platform tmp = obj.GetComponent<Platform>();

                tmp.easeAmount = Random.Range(0f, 2f);
                tmp.speed = Random.Range(3f, 6f);
                tmp.waitTime = Random.Range(0f, 2f);
            }
            // Normal Platform
            else
            {
                obj = objectPooling.GetObject("Platform");
                obj.transform.position = new Vector2(Random.Range(-7f, 7f), i);
            }

            obj.SetActive(true);
        }
    }

    public void Retry()
    {
        idx = 0;
        GenreateRandomPlatform(0, 40);
    }
}
