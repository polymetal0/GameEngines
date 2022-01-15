using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float rotSpeed = 2f;
    public float speed = .02f;
    public float runSpeed = .04f;
    private Vector2 _movement;
    private bool _isRunning;
    private bool _isInAir;
    private Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        _isRunning = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _isInAir = true;
        StartCoroutine(Jump());
    }

    private void Update()
    {
        
        _anim.SetBool("isMoving", _movement.y != 0);
        _anim.SetBool("isRunning", _isRunning);
        _anim.SetFloat("Speed", _movement.y);// / Math.Abs(_movement.y));
        _anim.SetFloat("Turn", _movement.x);// / Math.Abs(_movement.x));
        _anim.SetBool("isInAir", _isInAir);

    }

    private void FixedUpdate()
    {
        
        //var playerTransform = transform;
        float running = _isRunning ? runSpeed : speed;
        //float moving = _anim.GetBool("isMoving") == true ? 1.0f : 0.0f;


        if (!_isInAir)
        {
            transform.Rotate(Vector3.up * _movement.x * rotSpeed);// * moving);
            transform.Translate(Vector3.forward * _movement.y * running);
        }
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(2.0f);
        _isInAir = false;
    }
}
