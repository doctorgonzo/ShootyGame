using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : Singleton<ScoreManager>
{
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText = ScoreManager.Instance.scoreText;
    }
    private new void Awake()
    {
        DontDestroyOnLoad(this);
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void IncreaseScore(int amountToAdd)
    {
        score += amountToAdd;
    }

    public void DecreaseScore(int amountToSubtract)
    {
        score -= amountToSubtract;
    }



}
