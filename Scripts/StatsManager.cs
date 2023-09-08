using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StatsManager : Singleton<StatsManager>
{
    private Shoot shootScript;
    public int curSceneIndex;
    public string curSceneName;
    private Transform[] canvasObjects;
    public int prevSceneIndex; //the scene that you were in that called up the StatsManager
    public int nextSceneIndex; //the scene that would be next after the one that got us to the Stats screen (eg: if you just cleared 2, this points to 3)
    public int TotalShotsFired;
    public int TotalShotsHit;
    public float TotalAccuracy;
    public int TotalBullseyes;
    public int HighestStreak;
    private int prevStreak;
    public int TotalDestroyed;
    public int ArcheryDestroyed;
    public int HayDestroyed;
    public int ChungusDestroyed;
    public int TractorDestroyed;
    public float TotalTime;
    public int TimesReloaded;


    private static string prefabPath = "Prefabs/StatsCanvas";
    private static StatsManager statsMan;
    public static StatsManager StatsMan
    {
        set
        {
            statsMan = value;
        }
        get
        {
            if (statsMan == null)
            {
                Object statsManRef = Resources.Load(prefabPath);
                GameObject statsManObj = Instantiate(statsManRef) as GameObject;
                if (statsManObj != null)
                {
                    statsMan = statsManObj?.GetComponent<StatsManager>();
                    DontDestroyOnLoad(statsManObj);
                }
            }
            return statsMan;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Instance.curSceneName = SceneManager.GetActiveScene().name;
        if (Instance.curSceneName != "Stats" && Instance.curSceneName != "Startup")
        {
            Instance.shootScript = Camera.main.GetComponent<Shoot>();
            Instance.curSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Instance.prevSceneIndex = Instance.curSceneIndex - 1;
            Instance.nextSceneIndex = Instance.curSceneIndex + 1;
            TotalAccuracy += shootScript.accuracy;
        }
        else if(Instance.curSceneName == "Stats" /*|| Instance.curSceneName == "Startup"*/)
        {
            //gameObject.SetActive(true);
            //foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            //{
            //    item.gameObject.SetActive(true);
            //}
            canvasObjects = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var item in canvasObjects)
            {
                if (item.name != "Canvas")
                {
                    item.gameObject.SetActive(false);
                }
            }
            //set all the stats variables
            TotalShotsFired += shootScript.shotsFired;
            TotalShotsHit += shootScript.shotsHit;
            TotalAccuracy += shootScript.accuracy;
            //TotalAccuracy = shootScript.accuracy;
            TotalAccuracy = (shootScript.accuracy * shootScript.levelNum + shootScript.accuracy) / (shootScript.levelNum + 1);
            TotalBullseyes += shootScript.totalBullseyes;
            //HighestStreak += shootScript.highestStreak;
            prevStreak = shootScript.highestStreak;
            if (prevStreak > HighestStreak)
            {
                HighestStreak = prevStreak;
            }
            else if (prevStreak < HighestStreak)
            {
                HighestStreak = shootScript.highestStreak;
            }
            TotalDestroyed += shootScript.targetsDestroyedCount;
            ArcheryDestroyed += shootScript.archDestroyed;
            HayDestroyed += shootScript.hayDestroyed;
            ChungusDestroyed += shootScript.chungusDestroyed;
            TractorDestroyed += shootScript.tractorDestroyed;
            TotalTime += shootScript.totalTime;
            TimesReloaded += shootScript.timesReloaded;
            GameObject.Find("TotalShotsFiredNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalShotsFired.ToString();
            GameObject.Find("TotalShotsHitNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalShotsHit.ToString();
            GameObject.Find("TotalAccuracyNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalAccuracy.ToString("P");
            GameObject.Find("TotalBullseyesNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalBullseyes.ToString();
            GameObject.Find("HighestStreakNum").GetComponent<TMPro.TextMeshProUGUI>().text = HighestStreak.ToString();
            GameObject.Find("TotalDestroyedNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalDestroyed.ToString();
            GameObject.Find("ArchDestroyedNum").GetComponent<TMPro.TextMeshProUGUI>().text = ArcheryDestroyed.ToString();
            GameObject.Find("HayDestroyedNum").GetComponent<TMPro.TextMeshProUGUI>().text = HayDestroyed.ToString();
            GameObject.Find("ChungusDestroyedNum").GetComponent<TMPro.TextMeshProUGUI>().text = ChungusDestroyed.ToString();
            GameObject.Find("TractorDestroyedNum").GetComponent<TMPro.TextMeshProUGUI>().text = TractorDestroyed.ToString();
            GameObject.Find("TotalTimeNum").GetComponent<TMPro.TextMeshProUGUI>().text = TotalTime.ToString("F2");
            GameObject.Find("TimesReloadedNum").GetComponent<TMPro.TextMeshProUGUI>().text = TimesReloaded.ToString();
        }
        else if (Instance.curSceneName == "Startup")
        {
            canvasObjects = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var item in canvasObjects)
            {
                if (item.name != "Canvas")
                {
                    item.gameObject.SetActive(false);
                }
            }
            foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        Instance.curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Instance.curSceneName = SceneManager.GetActiveScene().name;
        if (Instance.curSceneIndex > 0)
        {
            Instance.shootScript = Camera.main.GetComponent<Shoot>();
            Instance.prevSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            Instance.curSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Instance.nextSceneIndex = Instance.curSceneIndex + 1;
        }
        if (Instance.curSceneName != "Stats" && Instance.curSceneIndex > 0)
        {
            Instance.shootScript = Camera.main.GetComponent<Shoot>();
        }
        //if (Instance.curSceneName == "Stats")
        //{
        //    foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
        //    {
        //        item.gameObject.SetActive(true);
        //    }
        //}
    }

    public void ContinueButton()
    {
            gameObject.SetActive(true);
            foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            {
                item.gameObject.SetActive(true);
            }
            canvasObjects = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var item in canvasObjects)
            {
                if (item.name != "Canvas")
                {
                    item.gameObject.SetActive(false);
                }
            }
        LevelManager.Instance.Continue();
    }

    public void ReplayButton()
    {
        LevelManager.Instance.Replay();
    }

    public void QuitButton()
    {
        foreach (var item in Instance.GetComponentsInChildren<Transform>(true))
        {
            if (item.gameObject.name != "StatsCanvas(Clone)")           
            {
                item.gameObject.SetActive(false);
            }
        }
        canvasObjects = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        foreach (var item in canvasObjects)
        {
            item.gameObject.SetActive(true);
        }
        LevelManager.Instance.LoadStage(0);
    }

    void Update()
    {
        Instance.prevSceneIndex = Instance.curSceneIndex - 1;
        Instance.nextSceneIndex = Instance.curSceneIndex + 1;
    }
}
