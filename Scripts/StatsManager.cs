using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//TODO: Fix the stupid dummyCursor. Need to make it spawn itself and persist. Already made a prefab, just need to implement. Need to change
//all references to dummyCursor or cursorDummy. Probably need to tag the dummyCursor prefab as such. 
public class StatsManager : Singleton<StatsManager>
{
    private Shoot shootScript;
    private int curSceneIndex;
    private string curSceneName;
    public int prevSceneIndex; //the scene that you were in that called up the StatsManager
    public int nextSceneIndex; //the scene that would be next after the one that got us to the Stats screen (eg: if you just cleared 2, this points to 3)


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
        curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        curSceneName = SceneManager.GetActiveScene().name;
        //needs to be here to test individual levels without a shitload of errors
        if (curSceneName == "Stats")
        {
            foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        if (curSceneName != "Stats" && curSceneIndex > 0)
        {
            shootScript = Camera.main.GetComponent<Shoot>();
        }
        if (curSceneName == "Stats")
        {
            foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public void ContinueButton()
    {
        LevelManager.Instance.Continue();
    }

    public void ReplayButton()
    {
        LevelManager.Instance.Replay();
    }

    public void QuitButton()
    {
        foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
        {
            item.gameObject.SetActive(false);
        }
        LevelManager.Instance.LoadStage(0);
        if (curSceneIndex > 0)
        {
            GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>().enabled = true;
        }
    }

    void Update()
    {
        
    }
}
