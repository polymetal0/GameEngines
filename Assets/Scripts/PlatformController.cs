using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Animator _anim;
    private Transform playerTf;
    private void Start()
    {
        _anim = GetComponentInParent<Animator>();
        playerTf = FindObjectOfType<HumanoidController>().transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _anim.SetBool("isOnPlatform", true);
            playerTf.SetParent(gameObject.transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _anim.SetBool("isOnPlatform", false);
            playerTf.SetParent(null);
        }
    }
}
