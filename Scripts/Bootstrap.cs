using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    private UIShit uiShit;
    private StatsManager statsManager;
    private CinemachineVirtualCamera vCam;
    private Transform dummyCursor;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && (!GameObject.FindWithTag("vCam")) && (!GameObject.Find("dummyCursor(Clone)")))
        {
            vCam = Instantiate(Resources.Load("CM vcam2").GetComponent<CinemachineVirtualCamera>());
            dummyCursor = Instantiate(Resources.Load("Prefabs/dummyCursor").GetComponent<Transform>());
            DontDestroyOnLoad(dummyCursor);
            DontDestroyOnLoad(vCam);
            uiShit = UIShit.UISTuff.GetComponent<UIShit>();
            if (uiShit.shootScript.reloadBar != null)
            {
                uiShit.shootScript.reloadBar.GetComponent<Image>().enabled = false;
            }
            vCam.GetComponent<CinCamClampExt>().refOrientation = dummyCursor.transform;
            vCam.LookAt = dummyCursor.transform;
            uiShit.vcam = vCam;
        }
        else if (SceneManager.GetActiveScene().buildIndex > 0) //&& SceneManager.GetActiveScene().name != "Stats")
        {
            if (vCam == null && (!GameObject.FindWithTag("vCam")) && (!GameObject.Find("dummyCursor(Clone)")))
            {
                dummyCursor = Instantiate(Resources.Load("Prefabs/dummyCursor").GetComponent<Transform>());
                DontDestroyOnLoad(dummyCursor);
                vCam = Instantiate(Resources.Load("CM vcam2").GetComponent<CinemachineVirtualCamera>());
                vCam.GetComponent<CinCamClampExt>().refOrientation = dummyCursor.transform;
                vCam.LookAt = dummyCursor.transform;
                DontDestroyOnLoad(vCam);
            }
            if (SceneManager.GetActiveScene().name == "Stats")
            {
                uiShit = UIShit.UISTuff.GetComponent<UIShit>();
                uiShit.shootScript = Camera.main.GetComponent<Shoot>();
                uiShit.shootScript.reloadBar.GetComponent<Image>().enabled = false;
            }
            else
            {
                uiShit = UIShit.UISTuff.GetComponent<UIShit>();
                uiShit.shootScript = Camera.main.GetComponent<Shoot>();
                uiShit.shootScript.reloadBar.gameObject.SetActive(true);
                uiShit.shootScript.reloadBar.GetComponent<Image>().enabled = true;
                LevelManager.Instance.enabled = true;
                LevelManager.Instance.gameObject.name = "LevelManager";
                ScoreManager.Instance.enabled = true;
                ScoreManager.Instance.gameObject.name = "ScoreManager";
                SpawnManager.Instance.enabled = true;
                SpawnManager.Instance.gameObject.name = "SpawnManager";
                TimerManager.Instance.enabled = true;
                TimerManager.Instance.gameObject.name = "TimerManager";
                MyCursor.Instance.cursorSet = false;
                MyCursor.Instance.gameObject.name = "MyCursor";
                DontDestroyOnLoad(MyCursor.Instance);
                statsManager = StatsManager.StatsMan.GetComponent<StatsManager>();
                uiShit.shootScript.timerManager = GameObject.Find("TimerManager").GetComponent<TimerManager>();
                uiShit.shootScript.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                uiShit.shootScript.streakParticles = GameObject.FindWithTag("vCam").GetComponentInChildren<ParticleSystem>();
            }
        }
    }

    void Update()
    {
        
    }
}
