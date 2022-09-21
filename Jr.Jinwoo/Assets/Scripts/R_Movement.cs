using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Movement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 moveDir;
    float moveSpeed = 3f;
    [SerializeField] float jumpPower = 6f;

    bool isGround;
    [HideInInspector] public bool isHit;
    R_AnimationController rAnimation;
    R_CharacterController characterController;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rAnimation = GetComponentInChildren<R_AnimationController>();
        characterController = GetComponent<R_CharacterController>();
    }

    private void Update()
    {
        isHit = characterController.isHit;
        if (!isHit)
        {
            rb.position += (moveDir * Time.deltaTime * moveSpeed);
            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }

    }

    public void MoveTo(Vector3 dir)
    {
        moveDir = new Vector3(dir.x, moveDir.y, dir.z);
    }

    public void JumpTo()
    {
        if (isGround && !isHit)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
