using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Xsl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]public UIShit uiShitScript;
    [SerializeField] public Observer observer;
    [SerializeField] public Shoot shootScript;
    [SerializeField]public TimerManager timerManager;
    [SerializeField] public SpawnManager spawnManager;
    [SerializeField]public SheThinksMyTractorsSexy sexyScript;
    [SerializeField] private AudioClip stageSelectMusic;
    [SerializeField] public AudioClip level1Music;
    [SerializeField] public AudioClip level2Music;
    [SerializeField] public AudioClip level3Music;
    [SerializeField] public AudioSource audioSource;
    public float volume = 1.0f;
    private GameObject ChonkSpawn1;
    private GameObject ChonkSpawn2;
    private GameObject ChonkSpawn3;
    private bool stage1Loaded = false;
    private bool stage2Loaded = false;
    private bool stage3Loaded = false;
    public int currentIndex = 0;
    public GameObject reloadBar;
    public event Action<int> levelLoaded;
    private AchievementsPopUp achPop;
    private bool started = false;
    private CinemachineVirtualCamera vCam;

    private new void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
        {
            uiShitScript = UIShit.UISTuff.GetComponent<UIShit>();
            observer = Observer.Observe.GetComponent<Observer>();
            shootScript = Camera.main.GetComponent<Shoot>();
            reloadBar = GameObject.Find("ReloadBar");
            //instantiates a New Game Object with a timermanager component
            timerManager = TimerManager.Instance;
            spawnManager = SpawnManager.Instance;
            shootScript.tractorPrefab = Resources.Load<GameObject>("Prefabs/Tractor");
            sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
            audioSource = Camera.main.GetComponent<AudioSource>();
            //this line instantiates the AchievementsCanvas Object by making a clone of the prefab
            achPop = AchievementsPopUp.AchPops.GetComponent<AchievementsPopUp>();
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
        {
            uiShitScript = UIShit.UISTuff.GetComponent<UIShit>();
            observer = Observer.Observe.GetComponent<Observer>();
            shootScript = Camera.main.GetComponent<Shoot>();
            reloadBar = GameObject.Find("ReloadBar");
            timerManager = TimerManager.Instance;
            spawnManager = SpawnManager.Instance;
            SceneManager.sceneLoaded += OnSceneLoaded;
            vCam = GameObject.FindWithTag("vCam").GetComponent<CinemachineVirtualCamera>();
        }
        DontDestroyOnLoad (this);
    }
    public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
        {
            uiShitScript = UIShit.UISTuff.gameObject.GetComponent<UIShit>();
            observer = Observer.Observe.GetComponent <Observer>();
            shootScript = GameObject.Find("Main Camera").GetComponent<Shoot>();
            timerManager = TimerManager.Instance;
            spawnManager = SpawnManager.Instance;
            reloadBar = GameObject.FindGameObjectWithTag("ReloadBar");
            reloadBar.SetActive(true);
            started = true;
            DontDestroyOnLoad(this);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            uiShitScript = UIShit.UISTuff.gameObject.GetComponent<UIShit>();
            shootScript = GameObject.Find("Main Camera").GetComponent<Shoot>();
            spawnManager = SpawnManager.Instance;
            reloadBar = shootScript.reloadBar.gameObject;
            reloadBar.SetActive(true);
            started = true;
            stage1Loaded = false;
            stage2Loaded = false;
        }
        audioSource = Camera.main.GetComponent<AudioSource>();
        HandleInitialLevelSetup();
        currentIndex = SceneManager.GetActiveScene().buildIndex;
        started = true;
    }
    public virtual void HandleInitialLevelSetup()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                spawnManager.AddAllSpawnPoints();
                spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                sexyScript.MoveTractorToEnd();
                spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                audioSource.PlayOneShot(stageSelectMusic);
                break;
            case 2:
                shootScript.weaponNum = 0;
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 20;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 15;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 8;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 5;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level1Music);
                break;
            case 3:
                shootScript.weaponNum = 0;
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 20;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 15;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 8;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        shootScript.fullAmmoSingle = 5;
                        shootScript.ammoCountSingle = shootScript.fullAmmoSingle;
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level2Music);
                break;
            case 4:
                ChonkSpawn1 = GameObject.Find("ChonkSpawn1");
                ChonkSpawn2 = GameObject.Find("ChonkSpawn2");
                ChonkSpawn3 = GameObject.Find("ChonkSpawn3");
                shootScript.weaponNum = 1;
                //myCursorScript.ChangeCursor(myCursorScript.weapon1CursorTex);
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn3.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level3Music);
                break;
            case 5:
                ChonkSpawn1 = GameObject.Find("ChonkSpawn1");
                ChonkSpawn2 = GameObject.Find("ChonkSpawn2");
                ChonkSpawn3 = GameObject.Find("ChonkSpawn3");
                shootScript.weaponNum = 1;
                //myCursorScript.ChangeCursor(myCursorScript.weapon1CursorTex);
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn3.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level3Music);
                break;
            case 6:
                ChonkSpawn1 = GameObject.Find("ChonkSpawn1");
                ChonkSpawn2 = GameObject.Find("ChonkSpawn2");
                ChonkSpawn3 = GameObject.Find("ChonkSpawn3");
                shootScript.weaponNum = 2;
                //myCursorScript.ChangeCursor(myCursorScript.weapon1CursorTex);
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn1.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn2.transform);
                        spawnManager.SpawnOne(shootScript.testTarget1, ChonkSpawn3.transform);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level3Music);
                break;
            case 7:
                shootScript.weaponNum = 0;
                vCam.GetComponent<CinCamClampExt>().angleBounds.x = 15;
                vCam.GetComponent<CinCamClampExt>().angleBounds.y = 60;
                switch (timerManager.difficultyLevel)
                {
                    case 0:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 1:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 2:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                    case 3:
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.haystackTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        shootScript.targetsToSpawn.Add(shootScript.archeryTarget);
                        spawnManager.AddAllSpawnPoints();
                        spawnManager.SpawnTractorAtStart(shootScript.tractorPrefab);
                        sexyScript = shootScript.tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
                        sexyScript.MoveTractorToEnd();
                        spawnManager.SpawnAllRandom(shootScript.targetsToSpawn);
                        break;
                }
                audioSource.Stop();
                audioSource.PlayOneShot(level3Music);
                break;
        }
    }
    void Update()
    {
        if (started == false)
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        //handles what to do with the destroyed targets in the Level Select scene
        if (shootScript != null)
        {
            if (stage2Loaded == false)
            {
                if (shootScript.targetCount <= 0)
                {
                    shootScript.targetsToSpawn.Clear();
                    LoadStage(3);
                    stage2Loaded = true;
                }
            }
            if (stage1Loaded == false)
            {
                if (shootScript.tractorCount <= 0)
                {
                    shootScript.targetsToSpawn.Clear();
                    LoadStage(2);
                    stage1Loaded = true;
                }
            }
            if (stage3Loaded == false)
            {

            }
        }
    }
    public void LoadNextLevel()
    {
        int currentSceneIndex = StatsManager.Instance.curSceneIndex;
        int nextSceneIndex = StatsManager.Instance.nextSceneIndex;
        levelLoaded?.Invoke(nextSceneIndex);

        SceneManager.LoadScene(nextSceneIndex);
    }
    public void LoadStage(int stageIndex)
    {
        SceneManager.LoadScene(stageIndex);
        if (stageIndex == 0)
        {
            uiShitScript.OnLevelWasLoaded(0);
        }
    }
    public void LoadStatsScreen()
    {
        //StatsManager.Instance.gameObject.SetActive(true);
            foreach (var item in StatsManager.Instance.gameObject.GetComponentsInChildren<Transform>(true))
            {
                item.gameObject.SetActive(true);
            }
        SceneManager.LoadScene("Stats");
        GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>().enabled = false;
    }
    public void Continue()
    {
        SceneManager.LoadScene(StatsManager.Instance.nextSceneIndex);
    }
    public void Replay()
    {
        SceneManager.LoadScene(StatsManager.Instance.curSceneIndex);
    }

}
