using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;
public class Shoot : MonoBehaviour
{
	#region ClassLevelVariables
	public GameObject gibs;
	public GameObject shotsFiredText;
	public GameObject shotsHitText;
	public GameObject shotsMissedText;
	public GameObject accuracyText;
	public GameObject streakText;
	public GameObject streakNumberText;
	public GameObject bullseyeStreakText;
	public GameObject targetsDestroyedText;
	public GameObject activeTargetsText;
	public GameObject timeBetweenShotsText;
	public GameObject averageShotTimeText;
	public GameObject bulletHit;
	public GameObject archeryTarget;
	public GameObject haystackTarget;
	public GameObject testTarget1;
	public GameObject testTarget2;
	public GameObject testTarget3;
	public SpawnManager spawnManager;
	public ParticleSystem streakParticles;
	public AudioClip bulletHitEnemySound;
	public AudioClip bulletHitWallSound;
	public AudioClip bulletHitNothingSound;
	public AudioClip gunFiredSound;
	public AudioClip bulletHitGrassSound;
	public AudioClip bulletHitGroundSound;
	public AudioClip bulletHitTractorSound;
	public AudioClip emptyGunSound;
	public AudioClip reloadSound;
	public AudioClip tripleCombo;
	public AudioClip superCombo;
	public AudioClip hyperCombo;
	public AudioClip brutalCombo;
	public AudioClip masterCombo;
	public AudioClip awesomeCombo;
	public AudioClip blasterCombo;
	public AudioClip monsterCombo;
	public AudioClip kingCombo;
	public AudioClip killerCombo;
	public AudioClip ultraCombo;
	public AudioClip comboBreaker;
	public StreakStuff streakNameScript;
	public StreakStuff streakNumberScript;
	public Target targetScript;
	public SheThinksMyTractorsSexy sexyScript;
	[SerializeField] public TimerManager timerManager;
	private AudioSource noisePlayer;
	public int levelNum = 0;
	public int ammoCountSingle=8;
	public int ammoCountAuto=30;
	public int ammoCountShotgun = 80;
	public int fullAmmoSingle=8;
	public int fullAmmoAuto=30;
	public int fullAmmoShotgun = 12;
	public int amountOfProjectiles = 8;
    public Image reloadBar;
    public int weaponNum = 0;
    private float reloadTimer = 1.33f;
    #region Statistics
    public int shotsFired;
	public int shotsHit;
	public int shotsMissed;
	public float accuracy;
	public float fireRate =8f;
	public float nextTimeToFire = 0f;
	private float shotTimer = 0.0f;
	public float timeBetweenShots = 0.0f;
	public float averageShotTime = 0.0f;
	private float averageTime = 0.0f;
	public int timesReloaded = 0;
	private int streakAmount;
	private int streakCounter = 0;
	private int bullseyeStreakCounter = 0;
	public int singleShotBullseyes = 0;
	public int autoBullseyes = 0;
	public int shotgunBullseyes = 0;
	public int totalBullseyes = 0;
	public int targetsDestroyedCount = 0;
	public int highestStreak = 0;
    public int prevHighestStreak = 0;
    public int archDestroyed = 0;
	public int hayDestroyed = 0;
	public int chungusDestroyed = 0;
	public int tractorDestroyed = 0;
	public float totalTime = 0.0f;
    #endregion
    public int activeTargetsCount = 0;
	public int actualActiveTargetCount = 0;
	public int tractorCount = 1;
	public int targetCount = 1;
	private bool playParticles;
	private bool fillBar = false;
	private bool fiftyShotsFired = false;
	private bool tenShotsHit = false;
	private bool reloadDone = false;
	private bool shotShotgun = false;
	public int damageOutput;
	private int shotgunDamageOutput;
	private GameObject hitObject;
	public List<GameObject> targetsToSpawn = new List<GameObject>();
	private List<float> shotTimes = new List<float>();
	public GameObject tractorPrefab;
	public ScoreManager scoreManager;
    #region Actions
    public event Action ReloadHappened;
	public event Action FiftyShotsFired;
	public event Action TenShotsHit;
    #endregion
    #endregion
    private void OnLevelWasLoaded(int level)
	{
        tractorPrefab = Resources.Load<GameObject>("/Prefabs/Tractor");
        if (SceneManager.GetActiveScene().buildIndex > 0 && GameObject.FindGameObjectWithTag("ReloadBar") != null)
		{
			reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<UnityEngine.UI.Image>();
			if (reloadBar != null)
			{
				reloadBar.fillAmount = 1f;
			}
		}
		if (SceneManager.GetActiveScene().buildIndex > 0)
		{
			scoreManager = ScoreManager.Instance;
		}
		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			weaponNum = 1;
		}
		switch (SceneManager.GetActiveScene().buildIndex)
		{
			case 2:
				levelNum = 0;
				break;
			case 3:
				levelNum = 1;
				break;
				case 4:
				levelNum = 2;
				break;
			case 5:
				levelNum = 3;
				break;
			case 6:
				levelNum = 4;
				break;
			case 7:
				levelNum = 5;
				break;
		}
	}
    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    private void Awake()
    {
		//scoreManager = ScoreManager.Instance;
		//timerManager = TimerManager.Instance;
        tractorPrefab = Resources.Load<GameObject>("/Prefabs/Tractor");
		amountOfProjectiles = 8;
		fullAmmoShotgun = 80;
		ammoCountShotgun = 80;
    }
    void Start()
	{
		shotsFired = 0;
		shotsHit = 0;
		shotsMissed = 0;
		accuracy = 0;
		streakAmount = 0;
		damageOutput = 50;
		shotgunDamageOutput = 20;
		shotTimer = 0.0f;
		timeBetweenShots = 0.0f;
		averageShotTime = 0.0f;
		averageTime = 0.0f;
		timesReloaded= 0;
		reloadTimer = 1.33f;
		noisePlayer = GetComponent<AudioSource>();
		tractorPrefab = Resources.Load<GameObject>("/Prefabs/Tractor");
        if (SceneManager.GetActiveScene().buildIndex > 1)
		{
			timerManager = GameObject.Find("TimerManager").GetComponent<TimerManager>();
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
	}
	void FigureAverageShotTime()
	{
		foreach (var item in shotTimes)
		{
			averageTime += item;
		}
		averageShotTime = averageTime / shotTimes.Count;
		averageTime = 0.0f;
		averageShotTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Shot Time: " + averageShotTime.ToString("F3");
	}
	void Update()
	{
		totalBullseyes = singleShotBullseyes + autoBullseyes + shotgunBullseyes;
		totalTime += Time.deltaTime;
		if (prevHighestStreak > highestStreak)
		{
			highestStreak = prevHighestStreak;
		}
		else if (prevHighestStreak < highestStreak)
		{
			highestStreak = streakAmount;
		}
		if (SceneManager.GetActiveScene().buildIndex > 0)
		{
            if (shotsFired == 50 && fiftyShotsFired == false)
            {
                FiftyShotsFired?.Invoke();
                fiftyShotsFired = true;
            }
            if (shotsHit == 10 && tenShotsHit == false)
            {
                TenShotsHit?.Invoke();
                tenShotsHit = true;
            }
            switch (reloadBar.fillAmount)
            {
                case <= .33f:
                    reloadBar.color = Color.red;
                    break;
                case <= .66f:
                    reloadBar.color = Color.yellow;
                    break;
                case <= 1f:
                    reloadBar.color = Color.green;
                    break;
            }
            reloadBar.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 43, Input.mousePosition.z);
            if (fillBar == true)
            {
                //Debug.Log("FILLBAR");
                reloadBar.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 43, Input.mousePosition.z);
                reloadBar.fillAmount += 1f / reloadTimer * Time.deltaTime;
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (sexyScript.gameObject.activeSelf == false)
                {
                    tractorCount--;
                }
                if (targetScript.gameObject.activeSelf == false)
                {
                    targetCount--;
                }
            }
            timeBetweenShots = Time.timeSinceLevelLoad - shotTimer;
            if (streakAmount > 0)
            {
                streakNumberScript.SetStreakNumber(streakAmount);
				streakNumberScript.gameObject.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
            }
            if (streakAmount == 0 && streakNumberScript != null && streakNameScript != null)
            {
				streakNameScript.gameObject.GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
				streakNumberScript.gameObject.GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
            }
            if (streakAmount > 2)
            {
                streakNameScript.gameObject.SetActive(true);
            }
            #region Streak Stuff
            if (playParticles)
            {
                int[] streakNumbers = new int[] { 3, 6, 9, 12, 15, 18 };
                AudioClip[] streakClips = new AudioClip[]
                {
                    tripleCombo, superCombo, hyperCombo, brutalCombo, kingCombo, killerCombo, ultraCombo
                };
                if (streakCounter == 1)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[0]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("Basic Bitch Streak!");
                }
                if (streakCounter == 2)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[1]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("I'm Not Your Real Dad Streak!");
                }
                if (streakCounter == 3)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[2]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("Trump Won Streak!");
                }
                if (streakCounter == 4)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[3]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("LOL JK Streak!");
                }
                if (streakCounter == 5)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[4]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("Feliz Quinceañera Streak!");
                }
                if (streakCounter == 6)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[5]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("Two. Full Power. Shots. Streak!");
                }
                if (streakCounter == 7)
                {
                    if (streakNameScript.gameObject.activeSelf == false)
                    {
                        streakNameScript.gameObject.SetActive(true);
                    }
                    noisePlayer.PlayOneShot(streakClips[6]);
                    streakParticles.Play();
                    streakNameScript.SetStreakText("Ran Out of Ideas Streak!");
                }
                playParticles = false;
            }
            #endregion
            if (shotsFired > 0)
            {
                accuracy = (float)shotsHit / (float)shotsFired;
            }
			if (bullseyeStreakText != null && targetsDestroyedText != null && shotsFiredText != null && shotsHitText != null && shotsMissedText != null && accuracyText != null && activeTargetsText != null)
			{
                bullseyeStreakText.GetComponent<TMPro.TextMeshProUGUI>().text = "Consecutive Bullseyes: " + bullseyeStreakCounter.ToString();
                targetsDestroyedText.GetComponent<TMPro.TextMeshProUGUI>().text = "Targets Destroyed: " + targetsDestroyedCount.ToString();
                shotsFiredText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Fired: " + shotsFired.ToString();
                shotsHitText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Hit: " + shotsHit.ToString();
                shotsMissedText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Missed: " + shotsMissed.ToString();
                accuracyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Accuracy: " + accuracy.ToString("P");
				activeTargetsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Active Targets: " + activeTargetsCount.ToString();
				//activeTargetsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Active Targets: " + actualActiveTargetCount.ToString();
            }
			if (SceneManager.GetActiveScene().name != "Stats")
			{
                if (Input.GetMouseButton(0) && weaponNum == 1 && Time.time >= nextTimeToFire && ammoCountAuto > 0)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    ShootAutomatic();
                    reloadBar.fillAmount -= 1f / fullAmmoAuto;
                }
                if (Input.GetMouseButtonDown(0) && ammoCountSingle > 0 && weaponNum == 0)
                {
                    ShootSingle();
                    reloadBar.fillAmount -= 1f / fullAmmoSingle;
                    if (ammoCountSingle == 0)
                    {
                        noisePlayer.PlayOneShot(emptyGunSound);
                    }
                }
                if (Input.GetMouseButtonDown(0) && weaponNum == 2 && ammoCountShotgun > 0)
                {
                    shotShotgun = false;
                    for (int i = 0; i < amountOfProjectiles; i++)
                    {
                        ShootShotgun();
                    }
                    reloadBar.fillAmount -= 8f / fullAmmoShotgun;
                }
                if (Input.GetMouseButtonDown(0) && ammoCountSingle == 0 && weaponNum == 0)
                {
                    noisePlayer.PlayOneShot(emptyGunSound);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (fillBar == false)
                    {
                        reloadDone = false;
                        reloadBar.fillAmount = 0;
                        StartCoroutine(Reload());
                    }
                }
            }
            
        }
	}
	IEnumerator Reload()
	{
		timesReloaded++;
		ammoCountSingle = 0;
		ammoCountAuto = 0;
		noisePlayer.PlayOneShot(reloadSound);
		fillBar = true;
		yield return new WaitForSeconds(reloadTimer);
		if (reloadDone == false)
		{
            ReloadHappened?.Invoke();
			reloadDone = true;
			fillBar = false;
			ammoCountAuto = fullAmmoAuto;
			ammoCountSingle = fullAmmoSingle;
			ammoCountShotgun = fullAmmoShotgun;
		}
    }
	public void ShootSingle()
	{
		shotsFired++;
		ammoCountSingle--;
		shotTimer = Time.timeSinceLevelLoad;
		shotTimes.Add(timeBetweenShots);
		timeBetweenShotsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Time Between Shots: " + timeBetweenShots.ToString("F3");
		FigureAverageShotTime();
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		noisePlayer.PlayOneShot(gunFiredSound);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.isTrigger)
			{
				bullseyeStreakCounter++;
				singleShotBullseyes++;
				if (scoreManager != null)
				{
					scoreManager.IncreaseScore(80);
				}
				damageOutput *= 2;
				if (streakAmount >= 0)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
			}
			else
			{
				damageOutput = 50;
				bullseyeStreakCounter = 0;
			}
			if (hit.collider.gameObject.tag == "Enemy") //hit something tagged as an Enemy
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				var wantedMeshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
				shotsHit++;
				if (scoreManager != null)
				{
					scoreManager.IncreaseScore(20);
				}
				if (streakAmount >= 2)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
				hitObject = hit.collider.gameObject;
				//get the target script of the Enemy that you hit.
				targetScript = hitObject.GetComponent<Target>();
				//if the Enemy you hit has no health, he explodes.fuck you, michael bay.
				if (targetScript.health - damageOutput <= 0)
				{
					targetsDestroyedCount++;
					targetScript.TakeDamge(damageOutput);
					var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
					var gibsMat = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
					if (hitObject.transform.parent != null && hitObject.transform.parent.name == "TractorCompoundColliders")
					{
						gibsMat = hitObject.GetComponentInParent<MeshRenderer>().material;
					}
					foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
					{
						item.mesh = wantedMeshFilter.mesh;
					}
					foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
					{
						chonk.material = gibsMat;
						chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
					}
					if (newGibs.activeSelf == true)
					{
						Destroy(newGibs, 2f);
					}
					noisePlayer.PlayOneShot(bulletHitEnemySound);
				}
				else
				{
					targetScript.TakeDamge(damageOutput);
				}
			}
			#region Tractor Handling
			else if (hit.collider.gameObject.tag == "Tractor")  //hit the Tractor
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				var wantedMeshFilter = hit.collider.gameObject.GetComponentInParent<MeshFilter>();
				shotsHit++;
				var four = 4;
				var twenty = 20;
				var randoFlip = UnityEngine.Random.Range(1, 3);
				if (streakAmount >= 2)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
				hitObject = hit.collider.gameObject.transform.parent.gameObject;
				sexyScript = hitObject.GetComponentInParent<SheThinksMyTractorsSexy>();
				//if the Enemy you hit has no health, he explodes.fuck you, michael bay.
				if (sexyScript.health - damageOutput <= 0)
				{
					foreach (var item in GameObject.FindGameObjectsWithTag("BulletHit"))
					{
						if (item.transform.parent != null && item.transform.parent.tag == "Tractor")
						{
							Destroy(item);
						}
					}
					targetsDestroyedCount++;
					sexyScript.TakeDamge(damageOutput);
					var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
					var gibsMat = sexyScript.gameObject.GetComponent<MeshRenderer>().material;
					foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
					{
						item.mesh = wantedMeshFilter.mesh;
					}
					foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
					{
						chonk.material = gibsMat;
						chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
					}
					Destroy(newGibs, 2f);
					noisePlayer.PlayOneShot(bulletHitEnemySound);
				}
				else
				{
					sexyScript.TakeDamge(damageOutput);
					if (randoFlip == 1 && scoreManager != null)
					{
						scoreManager.IncreaseScore(four);
					}
					if (randoFlip == 2 && scoreManager != null)
					{
						scoreManager.IncreaseScore(twenty);
					}
				}
			}
			#endregion
			else if (hit.collider.gameObject.tag == "Chonks") //hit the enemy's chonks
			{
				if (streakAmount > 2)
				{
					noisePlayer.PlayOneShot(comboBreaker);
					scoreManager.DecreaseScore(50);
				}
				shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                //    highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
				streakCounter = 0;
				bullseyeStreakCounter = 0;
				playParticles = false;
			}
			else //hit anything that wasn't an enemy or its chonks (probably the wall for now)
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				noisePlayer.PlayOneShot(bulletHitWallSound);
				if (streakAmount > 2)
				{
					noisePlayer.PlayOneShot(comboBreaker);
					if (scoreManager != null)
					{
						scoreManager.DecreaseScore(50);
					}
				}
				shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                //    highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
				streakCounter = 0;
				bullseyeStreakCounter = 0;
				playParticles = false;
			}
			Debug.DrawRay(-Camera.main.transform.position, ray.direction * 200, Color.green, 10f, false);
		}
		else //hit nothing that registered as a hit
		{
			noisePlayer.PlayOneShot(bulletHitNothingSound);
			if (streakAmount > 2)
			{
				noisePlayer.PlayOneShot(comboBreaker);
				if (scoreManager != null)
				{
					scoreManager.DecreaseScore(50);
				}
			}
			shotsMissed++;
			prevHighestStreak = streakAmount;
            //if (prevHighestStreak > highestStreak)
            //{
                //highestStreak = prevHighestStreak;
            //}
            streakAmount = 0;
			streakCounter = 0;
			bullseyeStreakCounter = 0;
			playParticles = false;
		}
	}
	public void ShootAutomatic()
	{
		ammoCountAuto--;
		shotsFired++;
		shotTimer = Time.timeSinceLevelLoad;
		shotTimes.Add(timeBetweenShots);
		timeBetweenShotsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Time Between Shots: " + timeBetweenShots.ToString("F3");
		FigureAverageShotTime();
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		noisePlayer.PlayOneShot(gunFiredSound);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.isTrigger)
			{
				bullseyeStreakCounter++;
				autoBullseyes++;
				if (scoreManager != null)
				{
					scoreManager.IncreaseScore(80);
				}
				damageOutput *= 2;
				if (streakAmount >= 0)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
			}
			else
			{
				damageOutput = 50;
				bullseyeStreakCounter = 0;
			}
			if (hit.collider.gameObject.tag == "Enemy") //hit something tagged as an Enemy
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				var wantedMeshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
				shotsHit++;
				if (scoreManager != null)
				{
					scoreManager.IncreaseScore(20);
				}
				if (streakAmount >= 2)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
				hitObject = hit.collider.gameObject;
				//get the target script of the Enemy that you hit.
				targetScript = hitObject.GetComponent<Target>();
				//if the Enemy you hit has no health, he explodes.fuck you, michael bay.
				if (targetScript.health - damageOutput <= 0)
				{
					targetsDestroyedCount++;
					targetScript.TakeDamge(damageOutput);
					var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
					var gibsMat = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
					if (hitObject.transform.parent != null && hitObject.transform.parent.name == "TractorCompoundColliders")
					{
						gibsMat = hitObject.GetComponentInParent<MeshRenderer>().material;
					}
					foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
					{
						item.mesh = wantedMeshFilter.mesh;
					}
					foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
					{
						chonk.material = gibsMat;
						chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
					}
					if (newGibs.activeSelf == true)
					{
						Destroy(newGibs, 2f);
					}
					noisePlayer.PlayOneShot(bulletHitEnemySound);
				}
				else
				{
					targetScript.TakeDamge(damageOutput);
				}
			}
			#region Tractor Handling
			else if (hit.collider.gameObject.tag == "Tractor")  //hit the Tractor
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				var wantedMeshFilter = hit.collider.gameObject.GetComponentInParent<MeshFilter>();
				shotsHit++;
				var four = 4;
				var twenty = 20;
				var randoFlip = UnityEngine.Random.Range(1, 3);
				if (streakAmount >= 2)
				{
					if (streakAmount % 3 - 2 == 0)
					{
						playParticles = true;
						streakCounter++;
					}
				}
				streakAmount++;
				hitObject = hit.collider.gameObject.transform.parent.gameObject;
				sexyScript = hitObject.GetComponentInParent<SheThinksMyTractorsSexy>();
				//if the Enemy you hit has no health, he explodes.fuck you, michael bay.
				if (sexyScript.health - damageOutput <= 0)
				{
					foreach (var item in GameObject.FindGameObjectsWithTag("BulletHit"))
					{
						if (item.transform.parent != null && item.transform.parent.tag == "Tractor")
						{
							Destroy(item);
						}
					}
					targetsDestroyedCount++;
					sexyScript.TakeDamge(damageOutput);
					var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
					var gibsMat = sexyScript.gameObject.GetComponent<MeshRenderer>().material;
					foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
					{
						item.mesh = wantedMeshFilter.mesh;
					}
					foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
					{
						chonk.material = gibsMat;
						chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
					}
					Destroy(newGibs, 2f);
					noisePlayer.PlayOneShot(bulletHitEnemySound);
				}
				else
				{
					sexyScript.TakeDamge(damageOutput);
					if (randoFlip == 1 && scoreManager != null)
					{
						scoreManager.IncreaseScore(four);
					}
					if (randoFlip == 2 && scoreManager != null)
					{
						scoreManager.IncreaseScore(twenty);
					}
				}
			}
			#endregion
			else if (hit.collider.gameObject.tag == "Chonks") //hit the enemy's chonks
			{
				if (streakAmount > 2)
				{
					noisePlayer.PlayOneShot(comboBreaker);
					scoreManager.DecreaseScore(50);
				}
				shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                    //highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
				streakCounter = 0;
				bullseyeStreakCounter = 0;
				playParticles = false;
			}
			else //hit anything that wasn't an enemy or its chonks (probably the wall for now)
			{
				GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
				_bulletHit.transform.parent = hit.transform;
				noisePlayer.PlayOneShot(bulletHitWallSound);
				if (streakAmount > 2)
				{
					noisePlayer.PlayOneShot(comboBreaker);
					if (scoreManager != null)
					{
						scoreManager.DecreaseScore(50);
					}
				}
				shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                    //highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
				streakCounter = 0;
				bullseyeStreakCounter = 0;
				playParticles = false;
			}
			Debug.DrawRay(-Camera.main.transform.position, ray.direction * 200, Color.green, 10f, false);
		}
		else //hit nothing that registered as a hit
		{
			noisePlayer.PlayOneShot(bulletHitNothingSound);
			if (streakAmount > 2)
			{
				noisePlayer.PlayOneShot(comboBreaker);
				if (scoreManager != null)
				{
					scoreManager.DecreaseScore(50);
				}
			}
			shotsMissed++;
			prevHighestStreak = streakAmount;
            //if (prevHighestStreak > highestStreak)
            //{
                //highestStreak = prevHighestStreak;
            //}
            streakAmount = 0;
			streakCounter = 0;
			bullseyeStreakCounter = 0;
			playParticles = false;
		}
	}
	public void ShootShotgun()
	{
        ammoCountShotgun--;
      
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 direction = ray.direction;
		Vector3 spread = Vector3.zero;
		spread +=  Camera.main.transform.forward * UnityEngine.Random.Range(-1f, 1f);
		spread += Camera.main.transform.right * UnityEngine.Random.Range(-0.4f, 0.4f);
		direction += spread.normalized * UnityEngine.Random.Range(0f, 0.2f);
		if (shotShotgun == false)
		{
            shotsFired++;
            shotTimer = Time.timeSinceLevelLoad;
            shotTimes.Add(timeBetweenShots);
            timeBetweenShotsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Time Between Shots: " + timeBetweenShots.ToString("F3");
            FigureAverageShotTime();
            noisePlayer.PlayOneShot(gunFiredSound);
			shotShotgun = true;
        }
        if (Physics.Raycast(ray.origin, direction, out hit))
        {
            if (hit.collider.isTrigger)
            {
                bullseyeStreakCounter++;
				shotgunBullseyes++;
                if (scoreManager != null)
                {
                    scoreManager.IncreaseScore(80);
                }
                shotgunDamageOutput *= 2;
                if (streakAmount >= 0)
                {
                    if (streakAmount % 3 - 2 == 0)
                    {
                        playParticles = true;
                        streakCounter++;
                    }
                }
                streakAmount++;
            }
            else
            {
                shotgunDamageOutput = 20;
                bullseyeStreakCounter = 0;
            }
            if (hit.collider.gameObject.tag == "Enemy") //hit something tagged as an Enemy
            {
                GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                _bulletHit.transform.parent = hit.transform;
                var wantedMeshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
                shotsHit++;
                if (scoreManager != null)
                {
                    scoreManager.IncreaseScore(20);
                }
                if (streakAmount >= 2)
                {
                    if (streakAmount % 3 - 2 == 0)
                    {
                        playParticles = true;
                        streakCounter++;
                    }
                }
                streakAmount++;
                hitObject = hit.collider.gameObject;
                //get the target script of the Enemy that you hit.
                targetScript = hitObject.GetComponent<Target>();
                //if the Enemy you hit has no health, he explodes.fuck you, michael bay.
                if (targetScript.health - shotgunDamageOutput <= 0)
                {
                    targetsDestroyedCount++;
                    targetScript.TakeDamge(shotgunDamageOutput);
                    var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
                    var gibsMat = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    if (hitObject.transform.parent != null && hitObject.transform.parent.name == "TractorCompoundColliders")
                    {
                        gibsMat = hitObject.GetComponentInParent<MeshRenderer>().material;
                    }
                    foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
                    {
                        item.mesh = wantedMeshFilter.mesh;
                    }
                    foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        chonk.material = gibsMat;
                        chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
                    }
                    if (newGibs.activeSelf == true)
                    {
                        Destroy(newGibs, 2f);
                    }
                    noisePlayer.PlayOneShot(bulletHitEnemySound);
                }
                else
                {
                    targetScript.TakeDamge(shotgunDamageOutput);
                }
            }
            #region Tractor Handling
            else if (hit.collider.gameObject.tag == "Tractor")  //hit the Tractor
            {
                GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                _bulletHit.transform.parent = hit.transform;
                var wantedMeshFilter = hit.collider.gameObject.GetComponentInParent<MeshFilter>();
                shotsHit++;
                var four = 4;
                var twenty = 20;
                var randoFlip = UnityEngine.Random.Range(1, 3);
                if (streakAmount >= 2)
                {
                    if (streakAmount % 3 - 2 == 0)
                    {
                        playParticles = true;
                        streakCounter++;
                    }
                }
                streakAmount++;
                hitObject = hit.collider.gameObject.transform.parent.gameObject;
                sexyScript = hitObject.GetComponentInParent<SheThinksMyTractorsSexy>();
                //if the Enemy you hit has no health, he explodes.fuck you, michael bay.
                if (sexyScript.health - shotgunDamageOutput <= 0)
                {
                    foreach (var item in GameObject.FindGameObjectsWithTag("BulletHit"))
                    {
                        if (item.transform.parent != null && item.transform.parent.tag == "Tractor")
                        {
                            Destroy(item);
                        }
                    }
                    targetsDestroyedCount++;
                    sexyScript.TakeDamge(shotgunDamageOutput);
                    var newGibs = Instantiate(gibs, hit.point, Quaternion.identity);
                    var gibsMat = sexyScript.gameObject.GetComponent<MeshRenderer>().material;
                    foreach (var item in newGibs.GetComponentsInChildren<MeshFilter>())
                    {
                        item.mesh = wantedMeshFilter.mesh;
                    }
                    foreach (var chonk in newGibs.gameObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        chonk.material = gibsMat;
                        chonk.transform.localScale = hit.collider.gameObject.transform.localScale / 3;
                    }
                    Destroy(newGibs, 2f);
                    noisePlayer.PlayOneShot(bulletHitEnemySound);
                }
                else
                {
                    sexyScript.TakeDamge(shotgunDamageOutput);
                    if (randoFlip == 1 && scoreManager != null)
                    {
                        scoreManager.IncreaseScore(four);
                    }
                    if (randoFlip == 2 && scoreManager != null)
                    {
                        scoreManager.IncreaseScore(twenty);
                    }
                }
            }
            #endregion
            else if (hit.collider.gameObject.tag == "Chonks") //hit the enemy's chonks
            {
                if (streakAmount > 2)
                {
                    noisePlayer.PlayOneShot(comboBreaker);
                    scoreManager.DecreaseScore(50);
                }
                shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                    //highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
                streakCounter = 0;
                bullseyeStreakCounter = 0;
                playParticles = false;
            }
            else //hit anything that wasn't an enemy or its chonks (probably the wall for now)
            {
                GameObject _bulletHit = Instantiate(bulletHit, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                _bulletHit.transform.parent = hit.transform;
                noisePlayer.PlayOneShot(bulletHitWallSound);
                if (streakAmount > 2)
                {
                    noisePlayer.PlayOneShot(comboBreaker);
                    if (scoreManager != null)
                    {
                        scoreManager.DecreaseScore(50);
                    }
                }
                shotsMissed++;
				prevHighestStreak = streakAmount;
                //if (prevHighestStreak > highestStreak)
                //{
                    //highestStreak = prevHighestStreak;
                //}
                streakAmount = 0;
                streakCounter = 0;
                bullseyeStreakCounter = 0;
                playParticles = false;
            }
            Debug.DrawRay(-Camera.main.transform.position, ray.direction * 200, Color.green, 10f, false);
        }
        else //hit nothing that registered as a hit
        {
            noisePlayer.PlayOneShot(bulletHitNothingSound);
            if (streakAmount > 2)
            {
                noisePlayer.PlayOneShot(comboBreaker);
                if (scoreManager != null)
                {
                    scoreManager.DecreaseScore(50);
                }
            }
            Debug.DrawRay(-Camera.main.transform.position, ray.direction * 200, Color.green, 10f, false);
            shotsMissed++;
			prevHighestStreak = streakAmount;
            //if (prevHighestStreak > highestStreak)
            //{
                //highestStreak = prevHighestStreak;
            //}
            streakAmount = 0;
            streakCounter = 0;
            bullseyeStreakCounter = 0;
            playParticles = false;
        }
    }

	void ClearDebug()
	{
		Debug.Log("");
		Debug.ClearDeveloperConsole();
	}
}
