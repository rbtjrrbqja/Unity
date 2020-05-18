using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    // ObecjtPooling manage object pooling if this game.
    // Retry function reset all platform's position and set all false

    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;

    private List<GameObject> platformList;
    private List<GameObject> movingPlatformList;

    private void Start()
    {
        platformList = new List<GameObject>();
        movingPlatformList = new List<GameObject>();

        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < 20; i++)
        {
            var obj = Instantiate(platformPrefab) as GameObject;
            platformList.Add(obj);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
        }

        for (int i = 0; i < 20; i++)
        {
            var obj = Instantiate(movingPlatformPrefab);
            movingPlatformList.Add(obj);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
        }
    }

    public GameObject GetObject(string s)
    {
        List<GameObject> list = null;

        switch(s)
        {
            case "Platform":
                list = platformList;
                break;

            case "MovingPlatform":
                list = movingPlatformList;
                break;
        }

        foreach(GameObject ret in list)
        {
            if (!ret.activeSelf)
                return ret;
        }

        return null;
    }

    public void Retry()
    {
        foreach(GameObject obj in platformList)
        {
            obj.transform.position = Vector2.zero;
            obj.SetActive(false);
        }

        foreach (GameObject obj in movingPlatformList)
        {
            obj.transform.position = Vector2.zero;
            obj.SetActive(false);
        }
    }
}
