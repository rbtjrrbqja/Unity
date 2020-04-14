using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public GameObject starPrefabs;
    public int starCount;
    public float starSpeed;

    private float width, height;
    private List<Star> stars;

    private void Awake()
    {
        height = cam.orthographicSize * 1.5f;
        width = height;// / 9 * 16;
        stars = new List<Star>();
    }

    private void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            var star = Instantiate(starPrefabs) as GameObject;
            star.AddComponent<Star>().Init(width, height);
            star.transform.parent = this.transform;

            stars.Add(star.GetComponent<Star>());
        }
    }

    private void Update()
    {
        StarUpdate();
        StarShow();
        Viewcontrol();
    }

    private void StarUpdate()
    {
        for (int i = 0; i < starCount; i++)
        {
            stars[i].z -= starSpeed;

            if(stars[i].z < 1)
            {
                stars[i].z = width;
                stars[i].x = Random.Range(-width / 2, width / 2);
                stars[i].y = Random.Range(-height / 2, height / 2);
                stars[i].pz = stars[i].z;
            }
        }
    }

    private void StarShow()
    {
        for (int i = 0; i < starCount; i++)
        {
            float sx = GlobalFunction.newScale(stars[i].x/ stars[i].z, 0, 1, 0, width);
            float sy = GlobalFunction.newScale(stars[i].y/ stars[i].z, 0, 1, 0, height);

            float px = GlobalFunction.newScale(stars[i].x / stars[i].pz, 0, 1, 0, width);
            float py = GlobalFunction.newScale(stars[i].y / stars[i].pz, 0, 1, 0, height);

            stars[i].pz = stars[i].z;

            stars[i].myLineRenderer.SetPosition(0, new Vector2(sx, sy));
            stars[i].myLineRenderer.SetPosition(1, new Vector2(px, py));
        }
    }

    private void Viewcontrol()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        cam.transform.position = new Vector3(pos.x / 1.5f, pos.y / 1.3f, -10f);
    }
}
