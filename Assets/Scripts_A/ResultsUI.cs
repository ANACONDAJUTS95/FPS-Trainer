using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    public Text enemiesEliminatedText;
    public Text bulletsUsedText;
    public Text totalTimeText;

    public static ResultsUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateResults(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        enemiesEliminatedText.text = "Enemies Eliminated: " + enemiesEliminated;
        bulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
        totalTimeText.text = "Total Time to Finish: " + totalTime.ToString("F2") + " seconds";
    }
}
