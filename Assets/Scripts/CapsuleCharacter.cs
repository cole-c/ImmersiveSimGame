using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCharacter : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5;
        rotationSpeed = 5000;

        GameObject cam = GameObject.Find("Main Camera");
        cameraTransform = cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        Vector3 movementDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) movementDirection += cameraTransform.forward;
        if (Input.GetKey(KeyCode.S)) movementDirection -= cameraTransform.forward;
        if (Input.GetKey(KeyCode.A)) movementDirection -= cameraTransform.right;
        if (Input.GetKey(KeyCode.D)) movementDirection += cameraTransform.right;

        //TODO change this to jump
        movementDirection.y = 0f;

        transform.position += movementDirection.normalized * moveSpeed * Time.deltaTime;

        //Keep camera at player's back
        //if (movementDirection != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed * Time.deltaTime);
        //}

        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

}
