using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float startMoveSmoothingTime;
    [SerializeField] private float endMoveSmoothingTime;

    private Vector3 movementInput;

    public void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 _moveVector = transform.TransformDirection(movementInput) * movementSpeed;
        rb.velocity = new Vector3(_moveVector.x, rb.velocity.y, _moveVector.z);
    }
}
