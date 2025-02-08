using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PhysicsHands : MonoBehaviour
{
    [Header("PID")]
    [SerializeField] float frequency = 50f, rotFrequency = 100f;
    [SerializeField] float damping = 1f, rotDamping = 0.9f;
    [SerializeField] Transform target;
    [SerializeField] Rigidbody playerRigidbody;
    [Space]
    [Header("Hookes Law")]
    [SerializeField] float climbForce = 500f;
    [SerializeField] float climbDrag = 250f;
    [Space]
    [Header("Grabbing")]
    [SerializeField] InputActionReference grabReference;


    Vector3 _previousPosition;

    Rigidbody _rigidbody;
    bool _isColliding; //avoid collison between hands
    private Collision _collision;


    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
        transform.position = target.position;
        transform.rotation = target.rotation;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = float.PositiveInfinity;
        _previousPosition = transform.position;
        grabReference.action.started += OnGrab;
        grabReference.action.canceled += OnGrab;
    }

    void OnDestroy()
    {
        grabReference.action.started -= OnGrab;
        grabReference.action.canceled -= OnGrab;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();
        if (_isColliding) HookesLaw();
    }

    private void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();

        playerRigidbody.AddForce(force, ForceMode.Acceleration);
        playerRigidbody.AddForce(drag * -playerRigidbody.velocity * climbDrag, ForceMode.Acceleration);
    }

    private float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - _previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / (handVelocity.magnitude + 0.01f);
        drag = Math.Clamp(drag, 0.03f, 1f);
        _previousPosition = transform.position;
        return drag;
    }

    void PIDMovement()
    {
        //stabalise movement on player hand
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigidbody.velocity - _rigidbody.velocity) * kdg; //calculate between player rigid and controller rigid to calculate how much force to add
        _rigidbody.AddForce(force, ForceMode.Acceleration);


    }

    void PIDRotation()
    {
        float kp = (6f * rotFrequency) * (6f * rotFrequency) * 0.25f;
        float kd = 4.5f * rotFrequency * rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -_rigidbody.angularVelocity * kdg;
        _rigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
        _collision = collision;
    }

    private void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
        _collision = null;
    }

    void OnGrab(InputAction.CallbackContext ctx)
    {
        if (_collision != null && _collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            FixedJoint joint = rb.AddComponent<FixedJoint>();
            joint.connectedBody = rb;
        }
    }


    void OnRelease(InputAction.CallbackContext ctx)
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {
            Destroy(joint);
        }
    }

}
