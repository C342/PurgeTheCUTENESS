using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float Speed;

    private Rigidbody2D rb;
    private float xAxis;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInputs();
        Move();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2 (Speed * xAxis, rb.linearVelocity.y);
    }
}