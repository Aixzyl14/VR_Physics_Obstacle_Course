using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]

public class GrabController : MonoBehaviour
{ 
    //[SerializeField] private InputActionReference grabActionReference;
    public enum ClimbingState { Idle, Climbing, LetGo}

    public ClimbingState currentState;
    public ClimbingState previousState;

    public static event Action<GameObject> PlayerCollisionWithWater;

    private void Awake()
    {
        currentState = ClimbingState.Idle;
        previousState = ClimbingState.Idle;
    }



    private void Update()
    {
        if (currentState != previousState)
        {
            switch (currentState)
            {
                case ClimbingState.Idle:
                    Debug.Log("Idle state");
                    break;

                case ClimbingState.Climbing:
                    Debug.Log("Climbing State");
                    break;

                case ClimbingState.LetGo:
                    Debug.Log("Let Go");
                    break;

                default:
                    exitGame();
                    break;
            }
            previousState = currentState;
        }

        if (currentState == ClimbingState.LetGo)
        {
            StartCoroutine(IdleStateConv());
        }
    }

    private IEnumerator IdleStateConv()
    {
        yield return new WaitForSeconds(2f);
        currentState = ClimbingState.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ClimbableWall"))
        {
            currentState = ClimbingState.Climbing;
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            PlayerCollisionWithWater?.Invoke(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (currentState == ClimbingState.Climbing)
        {
            if (!collision.gameObject.CompareTag("ClimbableWall"))
            {
                currentState = ClimbingState.LetGo;
            }
        }
    }

    private void exitGame()
    {
        Debug.Log("something Wrong has occured");
        Application.Quit(); //tailor this for platform being used;
    }
}
