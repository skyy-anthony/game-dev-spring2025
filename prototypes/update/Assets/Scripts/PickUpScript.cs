using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private float rotationSensitivity = 1f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;
    
    public Camera mainCamera;  // Reference to the main camera
    public Camera holdLayerCamera;  // Reference to the hold layer camera
    private bool isCameraSwitched = false;  // Flag to track camera state

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor when not using camera switch
        Cursor.visible = false; // Hide cursor when locked
    }

    void Update()
    {
        // Switch camera if object is picked up
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                PickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            RotateObject();
        }
    }

    void PickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            if (hit.transform.gameObject.tag == "canPickUp")
            {
                heldObj = hit.transform.gameObject;
                heldObjRb = hit.transform.GetComponent<Rigidbody>();
                heldObjRb.isKinematic = true;
                heldObj.transform.parent = holdPos;

                // Switch camera to the holdLayerCamera when the object is picked up
                if (!isCameraSwitched)
                {
                    SwitchCamera(true);
                }
            }
        }
    }

    void DropObject()
    {
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;

        // Switch camera back to the main camera when the object is dropped
        SwitchCamera(false);
    }

    void MoveObject()
    {
        heldObj.transform.position = holdPos.position;
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            float xRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            heldObj.transform.Rotate(Vector3.down, xRotation);
            heldObj.transform.Rotate(Vector3.right, yRotation);
        }
    }

    void SwitchCamera(bool holdLayerActive)
    {
        if (holdLayerActive)
        {
            mainCamera.enabled = false;
            holdLayerCamera.enabled = true;
            isCameraSwitched = true;
        }
        else
        {
            mainCamera.enabled = true;
            holdLayerCamera.enabled = false;
            isCameraSwitched = false;
        }
    }
}
