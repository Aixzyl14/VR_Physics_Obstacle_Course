using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterCollision : MonoBehaviour
{


    private void EndGame()
    {
        // Add your game over logic here
        Debug.Log("Game Over");
        ReloadScene();
        // You can also load a game over scene, show a game over UI, etc.
    }
    public void ReloadScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        Debug.Log("bye");
    }
}