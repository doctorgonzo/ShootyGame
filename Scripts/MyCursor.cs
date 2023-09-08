using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyCursor : Singleton<MyCursor>
{
    private static string prefabPath = "Prefabs/MyCursor";
    private static MyCursor myCursor;
    public static MyCursor TheCursor
    {
        set
        {
            myCursor = value;
        }
        get
        {
            if (myCursor == null)
            {
                UnityEngine.Object myCursorRef = Resources.Load(prefabPath);
                GameObject myCursorObj = Instantiate(myCursorRef) as GameObject;
                if (myCursorObj != null)
                {
                    myCursor = myCursorObj?.GetComponent<MyCursor>();
                    DontDestroyOnLoad(myCursorObj);
                }
            }
            return myCursor;
        }
    }
    public Texture2D weapon0CursorTex;
    public Texture2D weapon1CursorTex;
    public Texture2D weapon2CursorTex;
    public Texture2D currentCursor;
    public Texture2D previousCursor;
    Vector3 worldPosition;
    public GameObject cursorDummy;
    private Vector2 hotSpot;
    private Shoot shootScript;
    public bool cursorSet = false;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
   
    void Start()
    {
        cursorDummy = GameObject.Find("dummyCursor(Clone)");
        currentCursor = Resources.Load<Texture2D>("crosshair 1");
        weapon0CursorTex = Resources.Load<Texture2D>("crosshair 1");
        weapon1CursorTex = Resources.Load<Texture2D>("crosshair2");
        weapon2CursorTex = Resources.Load<Texture2D>("crosshair3");
        Cursor.lockState = CursorLockMode.Confined;
        var xspot = currentCursor.width / 2;
        var yspot = currentCursor.height / 2;
        hotSpot = new Vector2(xspot, yspot);
        shootScript = Camera.main.GetComponent<Shoot>();
        Cursor.SetCursor(currentCursor, hotSpot, CursorMode.ForceSoftware);
        DontDestroyOnLoad(this);
    }

    private void OnLevelWasLoaded(int level)
    {
        cursorDummy = GameObject.Find("dummyCursor(Clone)");
        shootScript = Camera.main.GetComponent<Shoot>();
    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        if (cursorDummy != null)
        {
            cursorDummy.transform.position = worldPosition;
        }
        if (cursorSet == false)
        {
            switch (shootScript.weaponNum)
            {
                case 0:
                    ChangeCursor(weapon0CursorTex);
                    cursorSet = true;
                    break;
                case 1:
                    ChangeCursor(weapon1CursorTex);
                    cursorSet = true;
                    break;
                case 2:
                    ChangeCursor(weapon2CursorTex);
                    cursorSet = true;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings -1))
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            var allTractors = GameObject.FindGameObjectsWithTag("Tractor");
            foreach (var item in allTargets)
            {
                item.GetComponent<Target>().TakeDamge(6969);
            }
        }
    }
    public void ChangeCursor(Texture2D newCursor)
    {
        cursorSet = false;
        currentCursor = newCursor;
        Cursor.SetCursor(currentCursor, hotSpot, CursorMode.ForceSoftware);
    }
}
