using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int maxScore = 5;
    public float timeRemaining = 60f;

    private UIManager uiManager;
    private bool gameOver = false;

    public CharacterController controller;
    private float alturaOriginal;
    [HideInInspector] public bool forzarAgachado = false;

    void Awake()
    {
        instance = this;
        uiManager = FindObjectOfType<UIManager>();
        Time.timeScale = 1;
    }

    void Start()
    {
        alturaOriginal = controller.height;
    }
    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        timeRemaining -= Time.deltaTime;
        uiManager.UpdateTime(timeRemaining);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            gameOver = true;
            uiManager.MostrarPantallaGameOver();
            Time.timeScale = 0;
        }
        
        bool quierePararse = !Input.GetKey(KeyCode.LeftControl);

        if (!quierePararse || forzarAgachado)
        {
            controller.height = alturaOriginal / 2f;
        }
        else
        {
            controller.height = alturaOriginal;
        }
    }

public void AddScore()
    {
        if (gameOver) return;

        score++;
        uiManager.UpdateScore(score);

        if (score >= maxScore)
        {
            gameOver = true;
            uiManager.MostrarPantallaWin();
            Time.timeScale = 0;
        }
    }
}

