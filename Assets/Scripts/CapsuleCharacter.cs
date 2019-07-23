using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCharacter : MonoBehaviour
{
    public float moveSpeed;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody character;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5;

        character = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        moveVelocity = moveInput * moveSpeed;
    }

    private void FixedUpdate()
    {
        character.velocity = moveVelocity;
    }
}
