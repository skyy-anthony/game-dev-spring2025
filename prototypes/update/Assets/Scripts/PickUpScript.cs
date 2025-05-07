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

    public Camera mainCamera;         // Reference to the main camera
    public Camera holdLayerCamera;   // Reference to the hold layer camera
    private bool isCameraSwitched = false;  // Flag to track camera state

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera.enabled = true;
        holdLayerCamera.enabled = false;

        SyncCameras();
    }

    void Update()
    {
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

        if (isCameraSwitched)
        {
            // Sync cameras during object hold
            SyncCameras();

            // If the held object has been destroyed externally
            if (heldObj == null)
            {
                SwitchCamera(false);
            }
        }
    }

    void PickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            if (hit.transform.CompareTag("canPickUp"))
            {
                heldObj = hit.transform.gameObject;
                heldObjRb = heldObj.GetComponent<Rigidbody>();
                heldObjRb.isKinematic = true;
                heldObj.transform.parent = holdPos;

                SwitchCamera(true);
            }
        }
    }

    void DropObject()
    {
        if (heldObj != null)
        {
            heldObjRb.isKinematic = false;
            heldObj.transform.parent = null;
            heldObj = null;
        }

        SwitchCamera(false);
    }

    void MoveObject()
    {
        if (heldObj != null)
        {
            heldObj.transform.position = holdPos.position;
        }
    }

    void RotateObject()
    {
        if (heldObj != null && Input.GetKey(KeyCode.R))
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
            SyncCameras();
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

    void SyncCameras()
    {
        if (mainCamera != null && holdLayerCamera != null)
        {
            holdLayerCamera.transform.position = mainCamera.transform.position;
            holdLayerCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }
}
