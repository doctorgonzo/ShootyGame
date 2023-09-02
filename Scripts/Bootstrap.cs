using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    private UIShit uiShit;
    private StatsManager statsManager;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            uiShit = UIShit.UISTuff.GetComponent<UIShit>();
            if (GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>())
            {
                uiShit.shootScript.reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>();
                uiShit.shootScript.reloadBar.GetComponent<Image>().enabled = false;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
        {
            uiShit = UIShit.UISTuff.GetComponent<UIShit>();
            uiShit.shootScript = Camera.main.GetComponent<Shoot>();
            uiShit.shootScript.reloadBar.gameObject.SetActive(true);
            uiShit.shootScript.reloadBar.GetComponent<Image>().enabled = true;
            LevelManager.Instance.enabled = true;
            ScoreManager.Instance.enabled = true;
            SpawnManager.Instance.enabled = true;
            TimerManager.Instance.enabled = true;
            statsManager = StatsManager.StatsMan.GetComponent<StatsManager>();
        }
        else if (SceneManager.GetActiveScene().name == "Stats")
        {

        }
    }

    void Update()
    {
        
    }
}
