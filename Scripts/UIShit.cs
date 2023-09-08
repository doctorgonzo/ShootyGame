using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class UIShit : Singleton<UIShit>
{
    private static string prefabPath = "Prefabs/GUICanvas";
    private static UIShit uiShit;
    public static UIShit UISTuff
    {
        set
        {
            uiShit = value;
        }
        get
        {
            if (uiShit == null)
            {
                Object uiShitRef = Resources.Load(prefabPath);
                GameObject uiShitObject = Instantiate(uiShitRef) as GameObject;
                if (uiShitObject != null)
                {
                    uiShit = uiShitObject?.GetComponent<UIShit>();
                    DontDestroyOnLoad(uiShitObject);
                }
            }
            return uiShit;
        }
    }
    [SerializeField] TMPro.TMP_Dropdown difficultyDropdown;
    [SerializeField] UnityEngine.UI.Slider volumeSlider;
    [SerializeField] UnityEngine.UI.Slider musicVolumeSlider;
    [SerializeField] TMPro.TMP_Dropdown graphicsDropdown;
    [SerializeField] GameObject uiBackground;
    [SerializeField] TimerManager timerManager;
    [SerializeField] UnityEngine.UI.Button startButton;
    [SerializeField] UnityEngine.UI.Button optionsButton;
    [SerializeField] UnityEngine.UI.Button quitButton;
    [SerializeField] UnityEngine.UI.Button backButton;
    [SerializeField] UnityEngine.UI.Button saveButton;
    [SerializeField] UnityEngine.UI.Button escQuitButton;
    [SerializeField] AudioSource audioSource;
    [SerializeField] public CinemachineVirtualCamera vcam;
    [SerializeField] Transform[] allOptionsObjects;
    [SerializeField] int startingDifficulty = 2;
    [SerializeField] float startingVolume = 0.5f;
    [SerializeField] float musicStartingVolume = 0.2f;
    [SerializeField] int startingGraphics = 2;
    [SerializeField] bool escMenuOpen = false;
    [SerializeField] int wantedDifficulty = 0;
    public Shoot shootScript;
    private new void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            shootScript = Camera.main.GetComponent<Shoot>();
            shootScript.reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<UnityEngine.UI.Image>();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            shootScript = Camera.main.GetComponent<Shoot>();

        }
    }
    public void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "Stats")
        {
            allOptionsObjects = GetComponentsInChildren<Transform>(true);
            foreach (var item in allOptionsObjects)
            {
                switch (item.gameObject.name)
                {
                    case "UIBackground":
                        uiBackground = item.gameObject;
                        break;
                    case "StartButton":
                        startButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "OptionsButton":
                        optionsButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "PussyButton":
                        quitButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "DifficultySelect":
                        difficultyDropdown = item.gameObject.GetComponent<TMPro.TMP_Dropdown>();
                        break;
                    case "VolumeSlider":
                        volumeSlider = item.gameObject.GetComponent<UnityEngine.UI.Slider>();
                        break;
                    case "VolumeSliderMusic":
                        musicVolumeSlider = item.gameObject.GetComponent<UnityEngine.UI.Slider>();
                        break;
                    case "GraphicsDropdown":
                        graphicsDropdown = item.gameObject.GetComponent<TMPro.TMP_Dropdown>();
                        break;
                    case "BackButton":
                        backButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "SaveButton":
                        saveButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "QuitButton":
                        escQuitButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                        break;
                    case "EventSystem":
                        Debug.Log("+++++ONLEVELWASLOADED+++++FOUND ME SOME EVENTSYSTEMS IN HERE!");
                        break;
                    default:
                        break;
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                uiBackground.SetActive(true);
                startButton.gameObject.SetActive(true);
                optionsButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
            }
            if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
            {
                vcam = GameObject.FindGameObjectWithTag("vCam").GetComponent<CinemachineVirtualCamera>();
                SetDifficulty();
            }
            if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
            {
                timerManager = TimerManager.Instance.gameObject.GetComponent<TimerManager>();
            }
            audioSource = Camera.main.GetComponent<AudioSource>();
            if (vcam != null)
            {
                wantedDifficulty = difficultyDropdown.value;
                switch (wantedDifficulty)
                {
                    case 0:
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
                        break;
                    case 1:
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.44f;
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.33f;
                        break;
                    case 2:
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.75f;
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.899f;
                        break;
                    case 3:
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3.666f;
                        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 2.222f;
                        break;
                }
            }
            SetDifficulty();
            SetVolume();
            SetMusicVolume();
            SetGraphics();
        }
    }
    private void Start()
    {
        timerManager = TimerManager.Instance.gameObject.GetComponent<TimerManager>();
        uiBackground.SetActive(false);
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        allOptionsObjects = GetComponentsInChildren<Transform>(true);
        foreach (var item in allOptionsObjects)
        {
            switch (item.gameObject.name)
            {
                case "UIBackground":
                    uiBackground = item.gameObject;
                    break;
                case "StartButton":
                    startButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "OptionsButton":
                    optionsButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "PussyButton":
                    quitButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "DifficultySelect":
                    difficultyDropdown = item.gameObject.GetComponent<TMPro.TMP_Dropdown>();
                    break;
                case "VolumeSlider":
                    volumeSlider = item.gameObject.GetComponent<UnityEngine.UI.Slider>();
                    break;
                case "VolumeSliderMusic":
                    musicVolumeSlider = item.gameObject.GetComponent<UnityEngine.UI.Slider>();
                    break;
                case "GraphicsDropdown":
                    graphicsDropdown = item.gameObject.GetComponent<TMPro.TMP_Dropdown>();
                    break;
                case "BackButton":
                    backButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "SaveButton":
                    saveButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "QuitButton":
                    escQuitButton = item.gameObject.GetComponent<UnityEngine.UI.Button>();
                    break;
                case "EventSystem":
                    Debug.Log("FOUND ME SOME EVENTSYSTEMS IN HERE!");
                    break;
                default:
                    break;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
        {
            vcam = GameObject.FindGameObjectWithTag("vCam").GetComponent<CinemachineVirtualCamera>();
        }
        if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
        {
            timerManager = TimerManager.Instance.gameObject.GetComponent<TimerManager>();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            uiBackground.SetActive(true);
            startButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            HideEscMenu();
        }
        audioSource = Camera.main.GetComponent<AudioSource>();
        difficultyDropdown.value = startingDifficulty;
        volumeSlider.value = startingVolume;
        musicVolumeSlider.value = musicStartingVolume;
        graphicsDropdown.value = startingGraphics;
        SetDifficulty();
        HideEscMenu();
        SetVolume();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex  > 0)
        {
            if (escMenuOpen == true)
            {
                Camera.main.GetComponent<Shoot>().enabled = false;
                Time.timeScale = 0;
            }
            if (escMenuOpen == false)
            {
                Camera.main.GetComponent<Shoot>().enabled = true;
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (escMenuOpen == false)
                {
                    escMenuOpen = true;
                    ShowEscMenu();
                }
                else
                {
                    startingDifficulty = difficultyDropdown.value;
                    startingVolume = volumeSlider.value;
                    musicStartingVolume = musicVolumeSlider.value;
                    startingGraphics = graphicsDropdown.value;
                    Cursor.lockState = CursorLockMode.Locked;
                    Invoke("UnlockCursor", 0.222f);
                    escMenuOpen = false;
                    HideEscMenu();
                }
            }
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetDifficulty()
    {
        //vcam = vCam;
        if (vcam.isActiveAndEnabled == false)
        {
            vcam.gameObject.SetActive(true);
            vcam.enabled = true;
        }
       if (vcam != null)
        {
            vcam.GetComponent<CinCamClampExt>().refOrientation = GameObject.Find("dummyCursor(Clone)").transform;
            vcam.LookAt = GameObject.Find("dummyCursor(Clone)").transform;
            switch (difficultyDropdown.value)
            {
                case 0:
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.44f;
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.44f;
                    if (timerManager != null)
                    {
                        timerManager.difficultyLevel = 0;
                    }
                    break;
                case 1:
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.33f;
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.75f;
                    if (timerManager != null)
                    {
                        timerManager.difficultyLevel = 1;
                    }
                    break;
                case 2:
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.55f;
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1.66f;
                    if (timerManager != null)
                    {
                        timerManager.difficultyLevel = 2;
                    }
                    break;
                case 3:
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3.666f;
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 2.222f;
                    if (timerManager != null)
                    {
                        timerManager.difficultyLevel = 3;
                    }
                    break;
            }
        }
        wantedDifficulty = difficultyDropdown.value;
    }

    public void SetVolume()
    {
        Camera.main.GetComponent<AudioSource>().volume = volumeSlider.value;
    }
    public void SetMusicVolume()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
        LevelManager levelM = LevelManager.Instance;
        AudioSource audioSource2 = levelM.audioSource;
            if (audioSource2 != null)
            {
                audioSource2.volume = musicVolumeSlider.value;
            }
        }
    }

    public void SetGraphics()
    {
        switch (graphicsDropdown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(5, true);
                break;
        }
    }

    public void BackButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ShowStartMenu();
        }
        difficultyDropdown.value = startingDifficulty;
        volumeSlider.value = startingVolume;
        musicVolumeSlider.value = musicStartingVolume;
        graphicsDropdown.value = startingGraphics;
        escMenuOpen = false;
        HideEscMenu();
    }

    public void SaveButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ShowStartMenu();
        }
        startingDifficulty = difficultyDropdown.value;
        startingVolume = volumeSlider.value;
        musicStartingVolume = musicVolumeSlider.value;
        startingGraphics = graphicsDropdown.value;
        escMenuOpen=false;
        HideEscMenu();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("StageSelect");
        StatsManager.Instance.gameObject.SetActive(true);
        uiBackground.SetActive(false);
        HideStartMenu();
    }

    public void OptionsButton()
    {
        ShowEscMenu();
        HideStartMenu();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void HideEscMenu()
    {
        difficultyDropdown.gameObject.SetActive(false);
        volumeSlider.gameObject.SetActive(false);
        musicVolumeSlider.gameObject.SetActive(false);
        graphicsDropdown.gameObject.SetActive(false);
        escQuitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
    }

    private void ShowEscMenu()
    {
        difficultyDropdown.gameObject.SetActive(true);
        volumeSlider.gameObject.SetActive(true);
        musicVolumeSlider.gameObject.SetActive(true);
        graphicsDropdown.gameObject.SetActive(true);
        escQuitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(true);
    }

    private void HideStartMenu()
    {
        optionsButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void ShowStartMenu()
    {
            optionsButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
    }    













}
