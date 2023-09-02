using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : Singleton<SpawnManager>
{
    public List<GameObject> availableSpawnPoints;
    private Shoot shootScript;
    private TimerManager timerManager;
    private GameObject tractorStart;
    private GameObject tractorEnd;

        private new void Awake()
        {
            shootScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shoot>();
            tractorStart = GameObject.FindGameObjectWithTag("TractorStart");
            tractorEnd = GameObject.FindGameObjectWithTag("TractorEnd");
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            timerManager = TimerManager.Instance;
        }
    }

        public void RemoveAvailableSpawnPoint(GameObject spawnPointToRemove)
        {
            availableSpawnPoints.Remove(spawnPointToRemove);
        }
    
    public void AddAvailableSpawnPoint(GameObject spawnPointToAdd)
        {
            availableSpawnPoints.Add(spawnPointToAdd);
        }

        public void AddAllSpawnPoints()
        {
            availableSpawnPoints.Clear();
            foreach (var item in GameObject.FindGameObjectsWithTag("Spawner"))
            {
                availableSpawnPoints.Add(item);
            }
        }

        void Update()
        {
               
        }

    public void SpawnTractorAtStart(GameObject tractorPrefab)
    {
        var sexyScript = tractorPrefab.GetComponent<SheThinksMyTractorsSexy>();
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            switch (timerManager.difficultyLevel)
            {
                case 0:
                    sexyScript.health = 220;
                    sexyScript.speed = 4.44f;
                    sexyScript.lives = 1;
                    break;
                case 1:
                    sexyScript.health = 320;
                    sexyScript.speed = 6.44f;
                    sexyScript.lives = 1;
                    break;
                case 2:
                    sexyScript.health = 420;
                    sexyScript.speed = 8.44f;
                    sexyScript.lives = 2;
                    break;
                case 3:
                    sexyScript.health = 620;
                    sexyScript.speed = 12.44f;
                    sexyScript.lives = 3;
                    break;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            sexyScript.health = 100;
            sexyScript.speed = 7.77f;
            sexyScript.lives = 2;
        }
        Instantiate(tractorPrefab, tractorStart.transform.position, tractorStart.transform.rotation);
        shootScript.activeTargetsCount++;
        shootScript.actualActiveTargetCount++;
    }

    public void SpawnTractorAtEnd(GameObject tractorPrefab)
    {
        Instantiate(tractorPrefab, tractorEnd.transform.position, tractorEnd.transform.rotation);
        shootScript.activeTargetsCount++;
        shootScript.actualActiveTargetCount++;
    }

    public void SpawnAllRandom(List<GameObject> tempTargetsToSpawn)
        {
        shootScript = GameObject.FindWithTag("MainCamera").GetComponent<Shoot>();
            for (int i = 0; i < tempTargetsToSpawn.Count; i++)
                {
                    var rando = Random.Range(0, availableSpawnPoints.Count - 1);
                    if (availableSpawnPoints.Count > 0)
                    {
                        Instantiate(tempTargetsToSpawn[i], availableSpawnPoints[rando].transform.position, availableSpawnPoints[rando].transform.rotation);
                        shootScript.activeTargetsCount++;
                        shootScript.actualActiveTargetCount++;
                        tempTargetsToSpawn[i].GetComponent<Target>().spawnPoint = availableSpawnPoints[rando];
                        availableSpawnPoints.Remove(availableSpawnPoints[rando]);
                    }
                }
        }

        public void SpawnOneRandom(GameObject tempTargetToSpawn)
        {
            if (availableSpawnPoints.Count > 0)
            {
                var rando = Random.Range(0, availableSpawnPoints.Count - 1);
                for (int i = 0; i < tempTargetToSpawn.transform.childCount; i++)
                {
                    Destroy(tempTargetToSpawn.transform.GetChild(i).gameObject);
                }
                Instantiate(tempTargetToSpawn, availableSpawnPoints[rando].transform.position, availableSpawnPoints[rando].transform.rotation);
                shootScript.activeTargetsCount++;
                shootScript.actualActiveTargetCount++;
                tempTargetToSpawn.GetComponent<Target>().spawnPoint = availableSpawnPoints[rando];
                availableSpawnPoints.Remove(availableSpawnPoints[rando]);
            }
            else
            {
                Debug.Log("OUT OF SPAWN POINTS, BRO!");
            }
        }

    public void SpawnOne(GameObject tempTargetToSpawn, Transform tempPosition)
    {
        Instantiate(tempTargetToSpawn, tempPosition.position, tempPosition.rotation);
        shootScript.activeTargetsCount++;
        shootScript.actualActiveTargetCount++;
    }
}
