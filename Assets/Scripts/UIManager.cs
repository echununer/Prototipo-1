using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txt_Score;
    public TextMeshProUGUI txt_Time;

    public GameObject panelWin;
    public GameObject panelGameOver;

    void Start()
    {
        UpdateScore(0);
        UpdateTime(GameManager.instance.timeRemaining);
    }

    public void UpdateScore(int score)
    {
        txt_Score.text = "Score:" + score.ToString() + "/5";
    }

    public void UpdateTime(float time)
    {
        txt_Time.text = "Time:" + Mathf.CeilToInt(time).ToString();
    }

    public void MostrarPantallaWin()
    {
        panelWin.SetActive(true);
    }

    public void MostrarPantallaGameOver()
    {
        panelGameOver.SetActive(true);
    }
}
