using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]

public class SwingController : MonoBehaviour
{
    //  public GameObject Rope;
    //  public GameObject Player;
    // private Vector3 ropePosition;
    // private Vector3 playerPosition;
    [SerializeField] private InputActionReference swingInputReference;


    public LineRenderer lineRenderer;

    [Header("Initializing Rope Appearance")]
    [SerializeField] private LineRenderer[] lineRenderers;

    [SerializeField] private float startWidth = 0.1f;

    [SerializeField] private float EndWidth = 0.1f;

    [Header("Rope Variables")]
    private bool isSwinging;

    [SerializeField] Transform ropeTransform;
    [SerializeField] Transform playerTransform;
    private Vector3 previousPlayerPosition;
    private Vector3 previousRopePosition;

    [Header("Player/Rope Physics var")]
    [SerializeField] public float StartswingForce = 800f;
    [SerializeField] public float NewswingForce;
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private Rigidbody ropeRigidBody;
    private float successSwing = 27.5f;

    private bool HoldingRope = false;
    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize Rope Render
        lineRenderer.SetWidth(startWidth, EndWidth);

        // Initialize the previous positions with the initial positions
        previousPlayerPosition = playerTransform.position;
        previousRopePosition = ropeTransform.position;

        playerRigidBody = playerTransform.GetComponent<Rigidbody>();



    }

    private void Update()
    {
       // Debug.Log(HoldingRope);
       // if (HoldingRope)
       // {
            swingInputReference.action.performed += TimeAllocate;
            swingInputReference.action.canceled += calc;
       // }
    }

    private void calc(InputAction.CallbackContext obj)
    {
        Vector3 swingDirection = CalculateSwingDirection(playerTransform, ropeTransform);
        float holdDuration = Time.time - startTime;
        NewswingForce = StartswingForce * (holdDuration * 3); //force * timedur * multiplier
        playerRigidBody.AddForce(swingDirection * NewswingForce, ForceMode.Acceleration);
        ropeRigidBody.AddForce(swingDirection * NewswingForce, ForceMode.Acceleration);
        Debug.Log("unpressed" + holdDuration);
        holdDuration = 0f;

    }

    private void TimeAllocate(InputAction.CallbackContext obj)
    {
        startTime += Time.time;
        Debug.Log("pressed" + Time.time);
    }


    private void OnEnable()
    {


    }


    private Vector3 CalculateSwingDirection(Transform player, Transform rope)
    {
        return rope.position - player.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HoldingRope = true;
        }
    }

    // private Vector3 CalculateSwingDirection(Transform playerTransform, Transform ropeTransform)
    // {
    // return Rope.position - Player.position;
    // }
}
