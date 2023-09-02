using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakStuff : MonoBehaviour
{
    private TMPro.TextMeshProUGUI streakText;
    private TMPro.TextMeshProUGUI streakNumber;

    void Start()
    {
        streakText = GameObject.Find("StreakName").GetComponent<TMPro.TextMeshProUGUI>();
        streakNumber = GameObject.Find("StreakNumber").GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    public void SetStreakNumber(int number)
    {
        streakNumber.text = "x " + number.ToString();
    }

    public void SetStreakText(string streakName)
    {
        streakText.text = streakName;
    }
}
