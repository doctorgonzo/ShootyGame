using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : Singleton<TimerManager>
{
    private float timeLeft;
    public TMPro.TextMeshProUGUI timerText;
    public int difficultyLevel = 2;
    private SpawnManager spawnManager;
    void Start()
    {
        switch (difficultyLevel)
        {
            case 0:
                timeLeft = 90.00f;
                break;
                case 1:
                timeLeft = 60.00f;
                break;
            case 2:
                timeLeft = 45.00f;
                break;
            case 3:
                timeLeft = 30.00f;
                break;
        }
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            timerText = GameObject.Find("TimerText").GetComponent<TMPro.TextMeshProUGUI>();
        }
        spawnManager = GameObject.Find("Main Camera").GetComponent<SpawnManager>();
        DontDestroyOnLoad(this);
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
        {
            timerText = GameObject.Find("TimerText").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Remaining: " + timeLeft.ToString("F2");
            if (timeLeft <= 0)
            {
                TimesUp();
            }
        }
    }

    public void AddTime(float timeToAdd)
    {
        timeLeft += timeToAdd;
    }

    public void RemoveTime(float timeToRemove)
    {
        timeLeft -= timeToRemove;
    }

    public void TimesUp()
    {
        
    }

    public void SetTime(float timeToSet)
    {
        timeLeft = timeToSet;
    }
}
