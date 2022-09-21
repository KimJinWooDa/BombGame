using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Animation : MonoBehaviour
{
    Animator animator;

    float getUpTimer = 1f;

    [SerializeField] JinwooAI ai;

    Vector3 targetPos;
    void Awake()
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
        ai.isHold = isOn;
    }


    public void OnHit()
    {
        if (ai.bomb != null)
        {
            ai.bomb.SetOffBomb(ai.bombPosition);
            ai.isHold = false;
            ai.isThrow = false;
        }

        animator.SetTrigger("OnHit");
        ai.navMeshAgent.ResetPath();
        OnMove(Vector3.zero);
        ai.isHit = true;
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

    public void OnThrow(Vector3 targetPos)
    {
        animator.SetBool("Hold", false);
        animator.SetTrigger("Throw");
        this.targetPos = targetPos;
    }

    public void OnThrowBomb()
    {
        Bomb bomb = ai.bomb;

        if (bomb == null) return;
        bomb.isOnce = false;
        Vector3 targetPos = transform.position + ai.transform.localRotation.normalized * new Vector3(0, 0, ai.throwPower);
        bomb.Explosion(ai.bombPosition, targetPos);
        AfterThrowingCharacterState();
    }
    void AfterThrowingCharacterState()
    {
        ai.isHold = false;
        ai.bomb = null;
        ai.nearBomb = null;
        ai.isThrow = false;
        StartCoroutine(WaitCoolTimeState());
        //ai.ResetState();
    }

    public void OnGetUp()
    {
        ai.isHit = false;
        ai.ResetState();
    }

    IEnumerator WaitCoolTimeState()
    {
        yield return new WaitForSeconds(1f);
        ai.ResetState();
    }
}
