using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

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
	[SerializeField] TimerManager timerManager;
	private AudioSource noisePlayer;
	public int ammoCountSingle=8;
	public int ammoCountAuto=30;
	public int ammoCountShotgun = 12;
	public int fullAmmoSingle=8;
	public int fullAmmoAuto=30;
	public int fullAmmoShotgun = 12;
	public int amountOfProjectiles = 8;
    public Image reloadBar;
	public int weaponNum = 0;
	private float reloadTimer = 1.33f;
	private float shotsFired;
	private float shotsHit;
	private float shotsMissed;
	private float accuracy;
	public float fireRate =8f;
	public float autoFireRate = 12f;
	public float nextTimeToFire = 0f;
	private float shotTimer = 0.0f;
	private float timeBetweenShots = 0.0f;
	private float averageShotTime = 0.0f;
	private float averageTime = 0.0f;
	private int timesReloaded = 0;
	private int streakAmount;
	private int streakCounter = 0;
	private int bullseyeCounter = 0;
	private int targetsDestroyedCount = 0;
	public int activeTargetsCount = 0;
	public int actualActiveTargetCount = 0;
	public int tractorCount = 1;
	public int targetCount = 1;
	private bool playParticles;
	private bool fillBar = false;
	private bool fiftyShotsFired = false;
	private bool tenShotsHit = false;
	private bool reloadDone = false;
	public int damageOutput;
	private int weapon0Dmg = 50;
	private int weapon1Dmg = 25;
	private int weapon2Dmg= 15;
	private int currentWeaponDmg;
	private GameObject hitObject;
	public List<GameObject> targetsToSpawn = new List<GameObject>();
	private List<float> shotTimes = new List<float>();
	public GameObject tractorPrefab;
	private ScoreManager scoreManager;
    #region Actions
    public event Action ReloadHappened;
	public event Action FiftyShotsFired;
	public event Action TenShotsHit;
    #endregion
    #endregion
    private void OnLevelWasLoaded(int level)
	{
		if (SceneManager.GetActiveScene().buildIndex > 0 && GameObject.FindGameObjectWithTag("ReloadBar") != null && SceneManager.GetActiveScene().name != "Stats")
		{
			reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<UnityEngine.UI.Image>();
			if (reloadBar != null)
			{
				reloadBar.fillAmount = 1f;
			}
		}
		if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
		{
			scoreManager = ScoreManager.Instance;
		}
        if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			weaponNum = 1;
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
		scoreManager = ScoreManager.Instance;
		autoFireRate = 10f;
		if (GameObject.FindGameObjectWithTag("ReloadBar"))
		{
			reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>();
		}
	}
    void Start()
	{
		shotsFired = 0;
		shotsHit = 0;
		shotsMissed = 0;
		accuracy = 0;
		streakAmount = 0;
		damageOutput = 50;
		shotTimer = 0.0f;
		timeBetweenShots = 0.0f;
		averageShotTime = 0.0f;
		averageTime = 0.0f;
		timesReloaded= 0;
		reloadTimer = 1.33f;
		noisePlayer = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
		{
			timerManager = GameObject.Find("New Game Object").GetComponent<TimerManager>();
		}
		if (SceneManager.GetActiveScene().buildIndex > 1 && SceneManager.GetActiveScene().name != "Stats")
		{
			scoreManager = GameObject.Find("New Game Object").GetComponent<ScoreManager>();
		}
		if (GameObject.FindGameObjectWithTag("ReloadBar"))
		{
			reloadBar = GameObject.FindGameObjectWithTag("ReloadBar").GetComponent<Image>();
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
        if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().name != "Stats")
        {
			switch (weaponNum)
			{
				case 0:
					currentWeaponDmg = weapon0Dmg;
					damageOutput = currentWeaponDmg;
					break;
					case 1:
					currentWeaponDmg = weapon1Dmg;
					damageOutput = currentWeaponDmg;
					break;
				case 2:
					currentWeaponDmg= weapon2Dmg;
					damageOutput = currentWeaponDmg;
					break;
			}
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
                Debug.Log("FILLBAR");
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
                streakNumberScript.gameObject.SetActive(true);
            }
            if (streakAmount == 0)
            {
                streakNameScript.gameObject.SetActive(false);
                streakNumberScript.gameObject.SetActive(false);
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
                accuracy = shotsHit / shotsFired;
            }
            bullseyeStreakText.GetComponent<TMPro.TextMeshProUGUI>().text = "Consecutive Bullseyes: " + bullseyeCounter.ToString();
            targetsDestroyedText.GetComponent<TMPro.TextMeshProUGUI>().text = "Targets Destroyed: " + targetsDestroyedCount.ToString();
            shotsFiredText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Fired: " + shotsFired.ToString();
            shotsHitText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Hit: " + shotsHit.ToString();
            shotsMissedText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shots Missed: " + shotsMissed.ToString();
            accuracyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Accuracy: " + accuracy.ToString("P");
            activeTargetsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Active Targets: " + activeTargetsCount.ToString();
            if (Input.GetMouseButton(0) && weaponNum == 1 && Time.time >= nextTimeToFire && ammoCountAuto > 0)
            {
                nextTimeToFire = Time.time + 1f / autoFireRate;
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
            if (Input.GetMouseButtonDown(0) && ammoCountSingle == 0 && weaponNum == 0)
            {
                noisePlayer.PlayOneShot(emptyGunSound);
            }
            if (Input.GetMouseButtonDown(0) && ammoCountAuto == 0 && weaponNum == 1)
            {
                noisePlayer.PlayOneShot(emptyGunSound);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (fillBar == false)
                {
                    reloadDone = false;
                    //fillBar = true;
                    reloadBar.fillAmount = 0;
                    StartCoroutine(Reload());
                }
            }
        }
       
	}
	IEnumerator Reload()
	{
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
				bullseyeCounter++;
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
				damageOutput = currentWeaponDmg;
				bullseyeCounter = 0;
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
				streakAmount = 0;
				streakCounter = 0;
				bullseyeCounter = 0;
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
				streakAmount = 0;
				streakCounter = 0;
				bullseyeCounter = 0;
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
			streakAmount = 0;
			streakCounter = 0;
			bullseyeCounter = 0;
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
				bullseyeCounter++;
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
				damageOutput = currentWeaponDmg;
				bullseyeCounter = 0;
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
				streakAmount = 0;
				streakCounter = 0;
				bullseyeCounter = 0;
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
				streakAmount = 0;
				streakCounter = 0;
				bullseyeCounter = 0;
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
			streakAmount = 0;
			streakCounter = 0;
			bullseyeCounter = 0;
			playParticles = false;
		}
	}

	public void ShootShotgun()
	{
        shotsFired++;
        ammoCountShotgun--;
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
                bullseyeCounter++;
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
                damageOutput = currentWeaponDmg;
                bullseyeCounter = 0;
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
                streakAmount = 0;
                streakCounter = 0;
                bullseyeCounter = 0;
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
                streakAmount = 0;
                streakCounter = 0;
                bullseyeCounter = 0;
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
            streakAmount = 0;
            streakCounter = 0;
            bullseyeCounter = 0;
            playParticles = false;
        }
    }

	void ClearDebug()
	{
		Debug.Log("");
		Debug.ClearDeveloperConsole();
	}
}
