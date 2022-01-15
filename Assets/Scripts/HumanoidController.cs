using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidController : MonoBehaviour
{
    public float speed = 1.5f;
    public float damper = .1f;
    private Vector2 _movement;
    private Animator _anim;
    private bool isRunning;
    private bool idle;
    private bool isJumping;
    private int idleLoop = 0;
    public Collider[] feet;
    public Cinemachine.CinemachineVirtualCamera cam;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetFloat("idleType", Random.Range(1, 3 + 1));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        isJumping =  context.performed;
        _anim.SetBool("isJumping", isJumping);
    }
 
    private void Update()
    {
        float accel = 1f;

        if (isRunning)
        {
            accel = 3f;
        }

        _anim.SetFloat("Horizontal", _movement.x * speed * accel, damper, 
            Time.deltaTime);
        _anim.SetFloat("Vertical", _movement.y * speed * accel, damper, 
            Time.deltaTime);

       
        if (Mathf.Abs(_anim.GetFloat("Horizontal")) < 0.1 && Mathf.Abs(_anim.GetFloat("Vertical")) < 0.1 && _anim.GetBool("isGrounded"))
        {
            _anim.SetBool("Idle", true);
            if (!idle)
            {
                idle = true;
                StartCoroutine(ChangeIdle());
            }
        }
        else
        {
            if (idle)
            {
                _anim.SetBool("Idle", false);
                StopAllCoroutines();
                idleLoop = 0;
                idle = false;
            }
        }

    }

    private void FixedUpdate()
    {
        // si no hay ningún collider a menos de 0.5m bajo los pies del personaje, está cayendo

        RaycastHit hit;
        if (Physics.Raycast(feet[0].transform.position, Vector3.down, out hit, 0.5f) || Physics.Raycast(feet[1].transform.position, Vector3.down, out hit, 0.5f))
        {
            _anim.SetBool("isGrounded", true);
        }
        else
        {
            _anim.SetBool("isGrounded", false);
        }
    }


    public void Injured()
    {
        float weight = _anim.GetLayerWeight(1);
        _anim.SetLayerWeight(1, 1 - weight);
        _anim.SetBool("Injured", !_anim.GetBool("Injured"));
    }

    public void Aim()
    {
        float weight = _anim.GetLayerWeight(2);
        _anim.SetLayerWeight(1, 0);
        _anim.SetBool("Injured", false);
        _anim.SetLayerWeight(2, 1 - weight);
        _anim.SetLayerWeight(3, (1 - weight));
        _anim.SetBool("Aiming", !_anim.GetBool("Aiming"));

        if (_anim.GetBool("Aiming"))
        {
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance = 3.3f;
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().ShoulderOffset.x = 1.0f;
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraSide = 1.0f;
        }
        else        
        {
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance = 5f;
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().ShoulderOffset.x = 0.5f;
            cam.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraSide = 0.5f;
        }
    }

    IEnumerator ChangeIdle()
    {
        // cada idle dura 5s
        yield return new WaitForSeconds(5);
        idle = false;
        _anim.SetFloat("idleType", Random.Range(1, 3 + 1));
        idleLoop += 5;

        // al llegar a 30s, cambia al cuarto idle
        if (idleLoop >= 30)
        {
            idleLoop = 0;
            _anim.SetBool("Idle", false);
        }
    }

}
