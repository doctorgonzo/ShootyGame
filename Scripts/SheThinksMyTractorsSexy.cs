using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

    public class SheThinksMyTractorsSexy : MonoBehaviour
    {
    public int lives;
    private int startingLives;
    public int health;
    private int startingHealth;
    public float speed;
    private float startingSpeed;
    private GameObject startSpawnPoint;
    private GameObject endSpawnPoint;
    private GameObject startingSpawnPoint;
    private GameObject tractorPrefab;
    public Shoot shootScript;
    public SpawnManager spawnManager;
    private int tractorSpawnCount = 1;
    private ScoreManager scoreManager;

    void Awake()
    {
        tractorPrefab = this.gameObject;
        startingHealth = health;
        startingLives = lives;
        startingSpeed = speed;
        shootScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shoot>();
        spawnManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SpawnManager>();
        startSpawnPoint = GameObject.FindGameObjectWithTag("TractorStart");
        endSpawnPoint = GameObject.FindGameObjectWithTag("TractorEnd");
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        tractorPrefab = gameObject;
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
        switch (tractorSpawnCount)
        {
            case 1:
                if (Vector3.Distance(tractorPrefab.transform.position, endSpawnPoint.transform.position) > .1f)
                {
                    tractorPrefab.transform.position = Vector3.MoveTowards(tractorPrefab.transform.position, endSpawnPoint.transform.position, speed * Time.deltaTime);
                    tractorPrefab.transform.localRotation = Quaternion.AngleAxis(180f, new Vector3(0, 1,0));
                }
                else
                {
                    tractorSpawnCount = 2;
                    tractorPrefab.transform.localRotation = Quaternion.AngleAxis(0f, new Vector3(0, 1, 0));
                }
                break;

            case 2:
                if (Vector3.Distance(tractorPrefab.transform.position, startSpawnPoint.transform.position) > .1f)
                {
                    tractorPrefab.transform.position = Vector3.MoveTowards(tractorPrefab.transform.position, startSpawnPoint.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    tractorSpawnCount = 1;
                }
                break;
        }
    }

    public void MoveTractorToEnd()
    {
        tractorPrefab = this.gameObject;
        startSpawnPoint = GameObject.FindGameObjectWithTag("TractorStart");
        endSpawnPoint = GameObject.FindGameObjectWithTag("TractorEnd");
        tractorSpawnCount = 1;
    }

    public void MoveTractorToStart()
    {
        tractorSpawnCount = 2;
    }

    public void TakeDamge(int incomingDamage)
    {
        if (health - incomingDamage <= 0)
        {
            health -= incomingDamage;
            lives--;
            if (scoreManager != null)
            {
                scoreManager.IncreaseScore(420);
            }
            if (lives > 0)
            {
                gameObject.SetActive(false);
                shootScript.activeTargetsCount--;
                Invoke("Respawn", UnityEngine.Random.Range(0.33f, 3.333f));
            }
            else
            {
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
        Destroy(this.gameObject);
        shootScript.activeTargetsCount--;
        shootScript.tractorDestroyed++;
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        shootScript.activeTargetsCount++;
        tractorPrefab.transform.position = endSpawnPoint.transform.position;
        tractorPrefab.transform.rotation = endSpawnPoint.transform.rotation;
        health = startingHealth;
        shootScript.tractorDestroyed++;
        MoveTractorToStart();
    }
}
