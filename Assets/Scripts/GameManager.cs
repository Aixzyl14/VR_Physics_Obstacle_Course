using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMov playerMov;
    private bool WaterTouching = false;
    public enum GameState { Start, Middle, End }
    public GameState currentState = GameState.Start;
    public GameState previousState = GameState.Start;


    private void Awake()
    {
        playerMov = FindObjectOfType<PlayerMov>();
        if (playerMov == null )
        {
            Debug.Log("PlayerMov script not found!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        WaterTouching = playerMov.WaterTouching;
        if (WaterTouching)
        {
            currentState = GameState.End;
        }

        if (currentState == GameState.End)
        {
            GameEnd();
        }
    }

    public void ReloadScene()
    {
        Debug.Log("Restarting");
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    private void GameEnd()
    {
        ReloadScene();
    }

   
}
