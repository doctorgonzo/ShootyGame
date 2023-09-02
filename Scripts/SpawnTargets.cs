//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnTargets : MonoBehaviour
//{
//    public GameObject spawn0, spawn1, spawn2, spawn3, spawn4, spawn5, spawn6, spawn7, spawn8, spawn9, spawn10, spawn11, spawn12, spawn13, spawn14, spawn15, spawn16, spawn17;
//    public GameObject target;
//    private GameObject target0;
//    private GameObject target1; 
//    private GameObject target2; 
//    private GameObject target3; 
//    private GameObject target4;
//    private GameObject target5;
//    private GameObject target6;
//    private GameObject target7;
//    private GameObject target8;
//    private GameObject target9;
//    private GameObject target10;
//    private GameObject target11;
//    private GameObject target12;
//    private GameObject target13;
//    private GameObject target14;
//    private GameObject target15;
//    private GameObject target16;
//    private GameObject target17;
//    public List<GameObject> targets = new List<GameObject>();
//    public List<GameObject> spawnPoints = new List<GameObject>();
//    private int spawnPoint;
//    private string spawnName;
//    private GameObject tempItem;
//    void Start()
//    {
//        target0 = Instantiate(target, spawn0.transform.position, spawn0.transform.rotation);
//        target1 = Instantiate(target, spawn1.transform.position, spawn1.transform.rotation);
//        target2 = Instantiate(target, spawn2.transform.position, spawn2.transform.rotation);
//        target3 = Instantiate(target, spawn3.transform.position, spawn3.transform.rotation);
//        target4 = Instantiate(target, spawn4.transform.position, spawn4.transform.rotation);
//        target5 = Instantiate(target, spawn5.transform.position, spawn5.transform.rotation);
//        target6 = Instantiate(target, spawn6.transform.position, spawn6.transform.rotation);
//        target7 = Instantiate(target, spawn7.transform.position, spawn7.transform.rotation);
//        target8 = Instantiate(target, spawn8.transform.position, spawn8.transform.rotation);
//        target9 = Instantiate(target, spawn9.transform.position, spawn9.transform.rotation);
//        target10 = Instantiate(target, spawn10.transform.position, spawn10.transform.rotation);
//        target11 = Instantiate(target, spawn11.transform.position, spawn11.transform.rotation);
//        target12 = Instantiate(target, spawn12.transform.position, spawn12.transform.rotation);
//        target13 = Instantiate(target, spawn13.transform.position, spawn13.transform.rotation);
//        target14 = Instantiate(target, spawn14.transform.position, spawn14.transform.rotation);
//        target15 = Instantiate(target, spawn15.transform.position, spawn15.transform.rotation);
//        target16 = Instantiate(target, spawn16.transform.position, spawn16.transform.rotation);
//        target17 = Instantiate(target, spawn17.transform.position, spawn17.transform.rotation);
//        targets.Add(target0);
//        targets.Add(target1);
//        targets.Add(target2);
//        targets.Add(target3);
//        targets.Add(target4);
//        targets.Add(target5);
//        targets.Add(target6);
//        targets.Add(target7);
//        targets.Add(target8);
//        targets.Add(target9);
//        targets.Add(target10);
//        targets.Add(target11);
//        targets.Add(target12);
//        targets.Add(target13);
//        targets.Add(target14);
//        targets.Add(target15);
//        targets.Add(target16);
//        targets.Add(target17);
//        foreach (var item in targets)
//        {
//            item.GetComponent<target>().isAlive = true;
//            item.GetComponent<target>().startPosition = item.transform;
//        }
//    }

//    void Update()
//    {
//        foreach (var item in targets)
//        {
//            if (item.GetComponent<target>().isAlive == false)
//            {
//                if (item.GetComponent<target>().lives > 0)
//                {
//                    item.SetActive(true);
//                    Instantiate(item, item.GetComponent<target>().startPosition.transform.position, item.GetComponent<target>().startPosition.rotation);
//                }
//            }
//            if (item.transform.position.y < 5.5f)
//            {
//                item.transform.position += new Vector3(0, Random.Range(0.005f, 0.03f), 0);
//            }
//        }
//    }
//}
