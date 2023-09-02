using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Observer : Singleton<Observer>
{
    private static string prefabPath = "Prefabs/Observer";
    private static Observer observe;
    public static Observer Observe
    {
        set
        {
            observe = value;
        }
        get
        {
            if (observe == null)
            {
                Object observeRef = Resources.Load(prefabPath);
                GameObject observeObject = Instantiate(observeRef) as GameObject;
                if (observeObject != null)
                {
                    observe = observeObject?.GetComponent<Observer>();
                    DontDestroyOnLoad(observeObject);
                }
            }
            return observe;
        }
    }
    public Shoot observedShootScript;
    public LevelManager observedLevelManager;
    public AchievementsPopUp achievementsPopUp;
    public TextMeshProUGUI achievementsText;
    public Sprite imgFirstLevelCleared;
    public Sprite imgFirstReload;

    private void Start()
    {
        DontDestroyOnLoad(this);
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            observedLevelManager = LevelManager.Instance.GetComponent<LevelManager>();
            observedShootScript = Camera.main.GetComponent<Shoot>();
            achievementsPopUp = GameObject.Find("AchievementCanvas(Clone)").GetComponent<AchievementsPopUp>();
            achievementsText = AchievementsPopUp.Instance.GetComponentInChildren<TextMeshProUGUI>(true);
            if (observedShootScript != null)
            {
                observedShootScript.ReloadHappened += OnReloadHappened;
                observedShootScript.FiftyShotsFired += OnFiftyShotsFired;
                observedShootScript.TenShotsHit += OnTenShotsHit; ;
            }
            if (observedLevelManager != null)
            {
                observedLevelManager.levelLoaded += OnLevelCleared;
            }

        }
    }
    
    private void OnLevelWasLoaded(int level)
    {
        DontDestroyOnLoad(gameObject);
        observedLevelManager = LevelManager.Instance.GetComponent<LevelManager>();
        observedShootScript = Camera.main.GetComponent<Shoot>();
        achievementsPopUp = GameObject.Find("AchievementCanvas(Clone)").GetComponent<AchievementsPopUp>();
        achievementsText = achievementsPopUp.GetComponentInChildren<TextMeshProUGUI>(true);
        if (observedShootScript != null)
        {
            observedShootScript.ReloadHappened += OnReloadHappened;
            observedShootScript.FiftyShotsFired += OnFiftyShotsFired;
            observedShootScript.TenShotsHit += OnTenShotsHit; ;
        }
        if (observedLevelManager != null)
        {
            observedLevelManager.levelLoaded += OnLevelCleared;
        }
    }
    private void OnReloadHappened()
    {
        Debug.Log("WE GOT US A RELOAD HERE!!!!!");
        achievementsText.text = "This Guy Reloads...";
        achievementsPopUp.ShowAchievementImage(imgFirstReload);
        achievementsPopUp.ShowAchievementText(achievementsText.text);
    }

    private void OnLevelCleared(int levelNum)
    {
        Debug.Log("THIS GUY'S CLEARING LEVELS OVER HERE! OH! LEVEL " + levelNum.ToString());
        achievementsText = achievementsPopUp.GetComponentInChildren<TextMeshProUGUI>(true);
        achievementsText.text = "First Steps...";
        achievementsPopUp.ShowAchievementImage(imgFirstLevelCleared);
        achievementsPopUp.ShowAchievementText(achievementsText.text);
    }

    private void OnFiftyShotsFired()
    {
        Debug.Log("THIS GUY SHOT FIFTY LOADS!!!!");
    }   
    
    private void OnTenShotsHit()
    {
        Debug.Log("LOOK AT THIS GUY, HITTING 10 THINGS!!");
    }    

    private void OnDestroy()
    {
        if(observedShootScript != null) 
        {
            observedShootScript.ReloadHappened -= OnReloadHappened;
        }
    }

    private void Update()
    {
           
    }



}
