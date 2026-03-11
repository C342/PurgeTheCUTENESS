using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2 (horizontalMovement*Speed, verticalMovement*Speed);

        if (Input.GetKeyDown(KeyCode.A)

        void FixedUpdate()
        {

        }
    }
}