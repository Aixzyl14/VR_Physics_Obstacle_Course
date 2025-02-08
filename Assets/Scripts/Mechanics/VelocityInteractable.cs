using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VelocityInteractable : XRGrabInteractable
{
    private ControllerVelocity controllerVelocity = null;
    private MeshRenderer meshRenderer = null;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
    }


}
