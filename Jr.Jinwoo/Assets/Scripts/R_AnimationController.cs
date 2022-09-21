using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_AnimationController : MonoBehaviour
{
    Animator animator;

    float getUpTimer = 1f;

    [SerializeField] R_CharacterController characterController;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(Vector3 dir)
    {
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.z);
    }

    public void OnJump()
    {
        animator.SetTrigger("Jump");

    }

    public void OnHold(bool isOn)
    {
        animator.SetBool("Hold", isOn);
        characterController.isHold = isOn;
    }

    public void OnThrow()
    {
        animator.SetBool("Hold", false);
        animator.SetTrigger("Throw");
    }


    public void OnHit()
    {
        if (characterController.bomb != null)
        {
            characterController.bomb.SetOffBomb(characterController.bombPosition);
            characterController.isHold = false;
        }

        animator.SetTrigger("OnHit");
        StartCoroutine(WaitGetUp());
    }

    IEnumerator WaitGetUp()
    {
        getUpTimer = 1f;
        while (getUpTimer > 0f)
        {
            getUpTimer -= Time.deltaTime;

            yield return null;
        }
        animator.SetTrigger("GetUp");
    }

    public void OnThrowBomb()
    {
        Bomb bomb = characterController.bomb;

        if (bomb == null) return;
        bomb.isOnce = false;
        Vector3 targetPos = transform.position + characterController.transform.localRotation.normalized * new Vector3(0, 0, characterController.throwPower);
        bomb.Explosion(characterController.bombPosition, targetPos);
        AfterThrowingCharacterState();
    }

    public void OnGetUp()
    {
        characterController.isHit = false;
    }

    void AfterThrowingCharacterState()
    {
        characterController.bomb = null;
        characterController.nearBomb = null;
        characterController.isThrow = false; //아 내가 멍청했네 animationGetBool 이걸ㄹ하면되는데
    }
}
