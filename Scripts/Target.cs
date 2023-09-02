using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    public int lives;
    public int health;
    public GameObject spawnPoint;
    private GameObject prefab;
    private LevelManager levelManager;
    private bool setHealth = false;
    public List<GameObject> availableSpawnPoints;
    public SpawnManager spawnManager;
    public Shoot shootScript;


    void Awake()
    {
        prefab = this.gameObject;
        health = 100;
        lives = 2;
        spawnManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SpawnManager>();
        shootScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shoot>();
        levelManager = LevelManager.Instance.GetComponent<LevelManager>();
    }

    void Update()
    {
        if (gameObject.name == "ChonkyTarget(Clone)")
        {
            if (setHealth == false)
            {
                SetHealth();
            }
        }
    }
    private void SetHealth()
    {
        health = 1000;
        lives = 3;
        setHealth = true;
    }
    public void TakeDamge(int incomingDamage)
    {
        if (health - incomingDamage <= 0)
        {
            health -= incomingDamage;
            prefab.GetComponent<Target>().lives--;
            if (prefab.GetComponent<Target>().lives > 0)
            {
                gameObject.SetActive(false);
                shootScript.activeTargetsCount--;
                Invoke("Respawn", UnityEngine.Random.Range(0.75f, 3.33f));
            }
            else
            {
                spawnManager.RemoveAvailableSpawnPoint(prefab.GetComponent<Target>().spawnPoint);
                Die();
            }
        }
        else
        {
            health -= incomingDamage;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        shootScript.activeTargetsCount--;
        shootScript.actualActiveTargetCount--;
        if (shootScript.actualActiveTargetCount <= 1)
        {
            levelManager.LoadStatsScreen();
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        shootScript.activeTargetsCount++;
        var rando = UnityEngine.Random.Range(0, spawnManager.availableSpawnPoints.Count - 1);
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            Destroy(prefab.transform.GetChild(i).gameObject);
        }
        if (spawnManager.availableSpawnPoints.Count > 0)
        {
            prefab.transform.position = spawnManager.availableSpawnPoints[rando].transform.position;
            prefab.transform.rotation = spawnManager.availableSpawnPoints[rando].transform.rotation;
            prefab.GetComponent<Target>().spawnPoint = spawnManager.availableSpawnPoints[rando];
            spawnManager.RemoveAvailableSpawnPoint(prefab.GetComponent<Target>().spawnPoint);
            prefab.GetComponent<Target>().health = 100;
            health = 100;
            if (gameObject.name == "ChonkyTarget(Clone)")
            {
                health = 1000;
            }
        }
        else
        {
            Debug.Log("NO SPAWNS");
        }
    }
}
