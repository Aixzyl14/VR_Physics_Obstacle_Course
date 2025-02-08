using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMov : MonoBehaviour
{
    public bool WaterTouching = false;

    [Space, Header("Velocity Tracker")]
    [SerializeField] private InputActionReference leftVelocity;
    [SerializeField] private InputActionReference rightVelocity;
    private float minV = 0.05f;
    private float maxV = 0.95f;
    Vector2 currRightval;
    Vector2 currLeftval;

    [Header("Variable Detectors")]
    private XROrigin _xrRig;
    private CapsuleCollider _collider;
    private Rigidbody _body;

    public InputActionReference positionAction;
    public bool isLeftController;


    private Vector3 controllerPosition;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _xrRig = GetComponent<XROrigin>();
    }

    private void Update()
    {
        //print(leftVelocity);
        //leftVelocity.action.Enable();
        //currRightval = rightVelocity.action.ReadValue<Vector2>();
        //currLeftval = leftVelocity.action.ReadValue<Vector2>();

        //if (leftVelocity >= minV)
        //{
        //    ControllerSwing(currLeftval, currRightval);
        //} 
    }

    //private void ControllerSwing(Vector2 leftVelocity, Vector2 rightVelocity)
    //{
    //    if (leftVelocity != null)
    //    {
    //        if (rightVelocity != null)
    //        { 
    //            //if (! > maxV)
    //            //{

    //            //}  
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            WaterTouching = true;
        }
    }


}
