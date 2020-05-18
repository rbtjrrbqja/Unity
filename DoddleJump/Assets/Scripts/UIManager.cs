using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UIManager manage UI, Caculating Score and Show Score to screen

    // Game Score, Highest position.y
    private int score = 0;

    // Player var
    public Player player;

    // Show score to screen
    public Text scoreText;

    // Retry Button
    public Button retryButton;

    private void Update()
    {
        scoreText.text = "Score : " + score;
        CheckMaxScore();
    }

    private void CheckMaxScore()
    {
        score = Mathf.Max(score, (int)(player.transform.position.y * 10));
    }

    public void GameOver()
    {
        retryButton.gameObject.SetActive(true);
    }

    public void Retry()
    {
        score = 0;
        retryButton.gameObject.SetActive(false);
    }
}
