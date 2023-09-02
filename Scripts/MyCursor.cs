using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyCursor : MonoBehaviour
{
    public Texture2D weapon0CursorTex;
    public Texture2D weapon1CursorTex;
    public Texture2D weapon2CursorTex;
    public Texture2D currentCursor;
    public Texture2D previousCursor;
    Vector3 worldPosition;
    public GameObject cursorDummy;
    private Vector2 hotSpot;
    private Shoot shootScript;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        var xspot = currentCursor.width / 2;
        var yspot = currentCursor.height / 2;
        hotSpot = new Vector2(xspot, yspot);
        shootScript = Camera.main.GetComponent<Shoot>();
        Cursor.SetCursor(currentCursor, hotSpot, CursorMode.ForceSoftware);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        cursorDummy.transform.position = worldPosition;
        switch (shootScript.weaponNum)
        {
            case 0:
                ChangeCursor(weapon0CursorTex);
                break;
                case 1:
                ChangeCursor(weapon1CursorTex);
                break;
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
        currentCursor = newCursor;
        Cursor.SetCursor(currentCursor, hotSpot, CursorMode.ForceSoftware);
    }
}
