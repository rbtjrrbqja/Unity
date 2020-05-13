using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 dir;
    public GameManager gameManager;
    private float delay = 0.1f;

    private state currentState;
    private bool isNewState;
    private List<GameObject> animalList;
    private Vector2 lastPos;
    private BoxCollider2D myCollider;

    private void Start()
    {
        animalList = new List<GameObject>();
        myCollider = GetComponent<BoxCollider2D>();
        SetState(state.CountDown);
        StartCoroutine(FSMMain());
    }

    private void Update()
    {
        InputKey();
    }

    IEnumerator FSMMain()
    {
        while (true)
        {
            isNewState = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator CountDown()
    {
        Debug.Log("CountDown");
        transform.position = new Vector2(-5.5f, -5.5f);

        do
        {
            yield return null;
            if (isNewState) break;

        } while (!isNewState);
    }

    IEnumerator GamePlay()
    {
        Debug.Log("GamePlay");
        dir = Vector2.right;

        // Add player object to animalList for moving
        animalList.Add(gameObject);

        do
        {
            // world position -> grid cell position
            Vector3Int pos = gameManager.grid.WorldToCell(transform.position);

            // out of range of time map
            if(gameManager.mapTile.GetTile(pos) == null)
            {
                SetState(state.GameOver);
                gameManager.SetState(state.GameOver);
            }

            // moving delay = game speed
            yield return new WaitForSeconds(delay);

            if (isNewState) 
                break;

            // save position of last animal in animalList 
            lastPos = animalList[animalList.Count - 1].transform.position;

            // avoid game over during moving
            myCollider.enabled = false;

            // all animals moving
            for (int i = animalList.Count - 1; i > 0; i--)
                animalList[i].transform.position = animalList[i - 1].transform.position;

            // player object moving
            transform.position += (Vector3)dir;

            myCollider.enabled = true;

        } while (!isNewState);
    }

    IEnumerator GameOver()
    {
        Debug.Log("GameOver");

        for (int i = 1; i < animalList.Count; i++)
            Destroy(animalList[i]);

        animalList.Clear();

        do
        {
            yield return null;
            if (isNewState) break;

        } while (!isNewState);
    }

    public void SetState(state s)
    {
        currentState = s;
        isNewState = true;
    }

    private void InputKey()
    {
        if (gameManager.currentState != state.GamePlay)
            return;

        if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Vector2.up)
            dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Vector2.down)
            dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Vector2.left)
            dir = Vector2.right;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Vector2.right)
            dir = Vector2.left;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Animal")
        {
            Debug.Log("Animal collision");

            // collision to animal in animalList -> gameover
            if (animalList.Contains(collision.gameObject))
            {
                SetState(state.GameOver);
                gameManager.SetState(state.GameOver);
            }
            // get new animal
            else
            {
                animalList.Add(collision.gameObject);
                collision.transform.position = lastPos;
                gameManager.PlayerAnimalGet();
            }
        }
    }
}
