using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 10.0f;
    private bool hasSetHeight = false;
    void Start()
    {
        target = GameObject.Find("CM vcam2");
        hasSetHeight = false;
    }

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, speed * 20 * Time.deltaTime);
        if (hasSetHeight == false)
        {
            transform.position += new Vector3(Random.Range(1,8), Random.Range(2, 6),-50);
            speed += Random.Range(.225f, 1.333f);
            hasSetHeight = true;
        }
    }
}
