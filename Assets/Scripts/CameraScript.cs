using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerTransform;
    public Transform camTransform;

    private Camera cam;
    private GameObject player;
    private GameObject throwAimer;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;

    private const float Y_ANGLE_MIN = 25.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        camTransform = transform;
        cam = Camera.main;

        throwAimer = GameObject.FindGameObjectWithTag("ThrowAimer");
    }

    private void Update()
    {
        currentX += (sensitivityX * Input.GetAxis("Mouse X"));
        currentY += (sensitivityY * Input.GetAxis("Mouse Y"));

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = playerTransform.position + rotation * dir;
        camTransform.LookAt(throwAimer.transform.position);
    }

    public float getCamHeight()
    {
        return currentY;
    }

}
