using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_CharacterController : MonoBehaviour
{
    public Rigidbody rb;

    float x;
    float z;

    Vector3 dir;

    R_Movement rMovement;
    R_AnimationController rAnimation;

    public Transform bombPosition;
    public Bomb bomb;
    public Bomb nearBomb;
    public bool isMove, isJump, isHold, isThrow, isDie, isGetUp, isHit;

    public float throwPower = 3f;

    Vector3 originPos;
    private void Start()
    {
        originPos = this.transform.position;
        rMovement = GetComponent<R_Movement>();
        rAnimation = GetComponentInChildren<R_AnimationController>();
    }
    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    
        dir = new Vector3(x, 0, z).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isThrow && !isDie && !isHit && !isGetUp)
        {
            rMovement.JumpTo();
            rAnimation.OnJump();
        }


        if (isHold && !isThrow && !isDie && !isHit && !isGetUp && !isMove)
        {
            if (Input.GetKeyDown(KeyCode.X)) //폭탄을 들고 있다면 던지기
            {
                isHold = false;
                isThrow = true;
                bomb.isOnce = false;
                rAnimation.OnThrow();
            }
        }

        if (nearBomb != null && Input.GetKeyDown(KeyCode.Z))
        {
            InputGetKeyDonwZ();
        }
    }

    private void FixedUpdate()
    {
        if (dir != Vector3.zero && !isHit && !isDie && !isGetUp && !isThrow)
        {
            rMovement.MoveTo(dir);
            rAnimation.OnMove(dir);
            isMove = true;
        }
        else
        {
            rMovement.MoveTo(Vector3.zero);
            rAnimation.OnMove(Vector3.zero);
            isMove = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Bomb"))
        {
            nearBomb = collision.collider.GetComponent<Bomb>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Bomb"))
        {
            nearBomb = null;
        }
    }

    void InputGetKeyDonwZ()
    {
        if (!isHold) //가지고 있지 않으면 폭탄 들기
        {
            isHold = true;

            bomb = nearBomb;
            bomb.isOnce = false;
            rAnimation.OnHold(true);
            bomb.SetOnBomb(bombPosition);
        }
        else if (isHold)
        {
            isHold = false;
            bomb.isOnce = true;
            rAnimation.OnHold(false); //가지고 있었던 상태라면 그 자리에 다시 두기
            bomb.SetOffBomb(bombPosition);

            bomb = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            transform.position = originPos;
        }
    }
}

