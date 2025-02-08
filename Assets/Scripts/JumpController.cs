using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]


public class JumpController : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpActionReference;
    [SerializeField] private float jumpForce = 550f;
    [SerializeField] private float forwardjumpForce = 25f;

    [Header("Camera Rotation")]
    private Quaternion CameraAngle;
    public GameObject CameraRef;
    public float cameraY;
    [SerializeField] private Transform cam;

    [Header("Variable Detectors")]
    private XROrigin _xrRig;
    private CapsuleCollider _collider;
    private Rigidbody _body;
    private bool isJumping;
    private int noOfJumps;

    private enum State {grounded, jumping}
    private State currentState = State.grounded;


    //private bool isGrounded => Physics.Raycast( // => when called it'll run function
    //    new Vector2(transform.position.x, transform.position.y + 2.0f),
    //    Vector3.down, 2.0f);

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _xrRig = GetComponent<XROrigin>();
        jumpActionReference.action.performed += OnJump;

    }

    private void exitGame()
    {
        Debug.Log("something Wrong has occured");
        Application.Quit(); //tailor this for platform being used;
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        //if (!isGrounded) return;
 
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
   
            switch (currentState)
            {
                case State.grounded:
                    _body.AddForce(Vector3.up * jumpForce);
                    _body.AddForce(camForward * forwardjumpForce);
                   // _body.AddForce(Vector3.forward * (10 + (cameraY * 0.01f)));
                    //_body.AddForce(Vector3.)
                    noOfJumps++;
                    currentState = State.jumping;
                    break;

                case State.jumping:
                    if (noOfJumps < 2)
                         _body.AddForce(Vector3.up * jumpForce);
                         _body.AddForce(camForward * forwardjumpForce);
                         noOfJumps++;
                    
                break;

                default:
                    exitGame();
                    break;
            
        }
  
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(noOfJumps);
        // Debug.Log(currentState);
       
        HeightAdjust();
        //if(Input.GetKeyUp(KeyCode.Space))
        // {
        //   OnJumpa();
        // }

        Quaternion CameraAngle = CameraRef.transform.rotation;
        Vector3 Camerarot = CameraAngle.eulerAngles;
        cameraY = Camerarot.y;

    }

    private void HeightAdjust()
    {
        var center = _xrRig.CameraInOriginSpacePos;
        _collider.center = new Vector3(center.x, _collider.center.y, center.z);
        _collider.height = _xrRig.CameraInOriginSpaceHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("grounded");
            currentState = State.grounded;
            noOfJumps = 0;

        }
    }

    //  private void OnCollisionEnter (Collision collision)
    // {
    //     if (collision.collider.tag == "Ground")
    //     {
    //         Debug.Log("Grounded");
    //     }
        
    // }
}
