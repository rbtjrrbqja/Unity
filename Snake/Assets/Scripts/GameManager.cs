using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum state { CountDown, GamePlay, GameOver };

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap mapTile;
    public Text countDownText;
    public GameObject gameOverWindow;
    public GameObject animalPrefab;
    public float spawnTime;

    [HideInInspector]
    public bool isNewState = false;

    [HideInInspector]
    public state currentState;

    private Player player;
    private float spawnDelay;
    private bool isAnimalOnMap;

    [HideInInspector]
    public GameObject currentAnimal;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        SetState(state.CountDown);
        StartCoroutine(FSMMain());
    }

    public void SetState(state s)
    {
        currentState = s;
        isNewState = true;
    }

    IEnumerator FSMMain()
    {
        while(true)
        {
            isNewState = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator CountDown()
    {
        Debug.Log("GM CountDown");

        countDownText.fontSize = 100;

        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countDownText.fontSize = 70;
        countDownText.text = "START!!!";
        yield return new WaitForSeconds(1f);

        countDownText.text = "";

        player.SetState(state.GamePlay);
        SetState(state.GamePlay);
    }

    IEnumerator GamePlay()
    {
        Debug.Log("GM GamePlay");
        spawnDelay = 0f;
        isAnimalOnMap = false;

        do
        {
            if(spawnDelay < spawnTime && !isAnimalOnMap)
                spawnDelay += Time.deltaTime;
            else if(spawnDelay >= spawnTime)
            {
                spawnDelay = 0f;
                Vector3Int pos = new Vector3Int(Random.Range(-6, 5), Random.Range(-6, 5), 0);
                currentAnimal = Instantiate(animalPrefab, mapTile.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);

                currentAnimal.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Random.Range(1, 12).ToString());

                isAnimalOnMap = true;
            }

            yield return null;
            if (isNewState) break;

        } while (!isNewState);
    }

    IEnumerator GameOver()
    {
        Debug.Log("GM GameOver");
        gameOverWindow.SetActive(true);

        if (currentAnimal != null)
            Destroy(currentAnimal);

        do
        {
            yield return null;
            if (isNewState) break;

        } while (!isNewState);
    }

    public void RetryButton()
    {
        SetState(state.CountDown);
        player.SetState(state.CountDown);

        gameOverWindow.SetActive(false);
    }

    public void PlayerAnimalGet()
    {
        currentAnimal = null;
        isAnimalOnMap = false;
    }
}
