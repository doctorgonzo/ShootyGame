using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPopUp : Singleton<AchievementsPopUp>
{
    [SerializeField] UnityEngine.UI.Image AchievementPicture;
    [SerializeField] TextMeshProUGUI AchievementText;
    private static string prefabPath = "Prefabs/AchievementCanvas";
    private static AchievementsPopUp achPopUp;
    public static AchievementsPopUp AchPops
    {
        set
        {
            achPopUp = value;
        }
        get
        {
            if (achPopUp == null)
            {
                Object achPopRef = Resources.Load(prefabPath);
                GameObject achObj = Instantiate(achPopRef) as GameObject;
                if (achObj != null)
                {
                    achPopUp = achObj?.GetComponent<AchievementsPopUp>();
                    DontDestroyOnLoad(achObj);
                }
            }
            return achPopUp;
        }
    }
    private new void Awake()
    {
        AchievementPicture = gameObject.GetComponentInChildren<UnityEngine.UI.Image>(true);
        AchievementText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        AchievementText.text = "";
        AchievementPicture.enabled = false;
    }
    private void OnLevelWasLoaded(int level)
    {
        AchievementPicture = gameObject.GetComponentInChildren<UnityEngine.UI.Image>(true);    
        AchievementText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        //AchievementPicture = Resources.Load("Tractor-15").GetComponent<Image>();
        AchievementText.text = "FUCKING TESTING";
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ShowAchievementImage(Sprite AchImage)
    {
        AchievementPicture.enabled = true;
        AchievementPicture.sprite = AchImage;
        AchievementPicture.gameObject.SetActive(true);
        Invoke("StopShowingAch", 6.66f);
    }

    public void ShowAchievementText(string AchText)
    {
        AchievementText.text = AchText; 
        AchievementText.gameObject.SetActive(true);
        Invoke("StopShowingAch", 6.66f);
    }
    private void StopShowingAch()
    {
        AchievementPicture.gameObject.SetActive(false);
        AchievementText.gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
