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
    private bool _jump;
    private Animator _anim;
    public Collider[] feet;

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
        _jump = context.performed;
        
        _isInAir = true;
        StartCoroutine(Jump());         
    }

    private void Update()
    {
        
        _anim.SetBool("isMoving", _movement.y != 0);
        _anim.SetBool("isRunning", _isRunning);
        _anim.SetFloat("Speed", _movement.y);
        _anim.SetFloat("Turn", _movement.x);
        _anim.SetBool("Jump", _jump);

    }

    private void FixedUpdate()
    {
        
        float running = _isRunning ? runSpeed : speed;
        if (!_isInAir)
        {
            transform.Rotate(Vector3.up * _movement.x * rotSpeed);
            transform.Translate(Vector3.forward * _movement.y * running);
        }

    }

    IEnumerator Jump()
    {
        // la animación del salto dura unos 2.2s
        yield return new WaitForSeconds(2.2f);
        _isInAir = false;
    }


}
