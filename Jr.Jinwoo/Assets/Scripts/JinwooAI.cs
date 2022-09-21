using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinwooAI : AI
{
    float minDistacne = 0;
    int targetIndex = 0;
    public override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        navMeshAgent.speed = moveSpeed;
        bombs.Clear();
        foreach (Bomb item in FindObjectsOfType<Bomb>())
        {
            if (item.isOnce)
            {
                bombs.Add(item);
            }
        }
        if (bombs.Count > 0)
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if (minDistacne > Vector3.Distance(this.transform.position, bombs[i].transform.position))
                {
                    minDistacne = Vector3.Distance(this.transform.position, bombs[i].transform.position);
                    targetIndex = i;
                }
            }

            StartCoroutine(SetState());
        }
        else
        {

            StartCoroutine(WaitBomb());
        }
    }

    public override void HoldBomb()
    {
        aiAni.OnHold(true);
        StartCoroutine(MoveToPlayer());
    }

    public override void ResetState()
    {
        navMeshAgent.ResetPath();
        bombs.Clear();
        foreach (Bomb item in FindObjectsOfType<Bomb>())
        {
            if (item.isOnce)
            {
                bombs.Add(item);
            }
        }
        if (bombs.Count > 0)
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if (minDistacne > Vector3.Distance(this.transform.position, bombs[i].transform.position))
                {
                    minDistacne = Vector3.Distance(this.transform.position, bombs[i].transform.position);
                    targetIndex = i;
                }
            }
            StartCoroutine(SetState());
        }
        else
        {
            StartCoroutine(WaitBomb());
        }
    }

    IEnumerator MoveToPlayer()
    {
        int randomTraceIndex = Random.Range(0, players.Count - 1);
        while (isHold && !isHit && !isGetUp && !isThrow)
        {

            Vector3 dir = (players[randomTraceIndex].transform.position - transform.position).normalized;
            aiAni.OnMove(dir);
            navMeshAgent.SetDestination(players[randomTraceIndex].transform.position);

            if (Vector3.Distance(this.transform.position, players[randomTraceIndex].transform.position) < 2f)
            {
                navMeshAgent.ResetPath();
                aiAni.OnMove(Vector3.zero);

                aiAni.OnThrow(players[randomTraceIndex].transform.position);
                isThrow = true;
                isHold = false;
                StopCoroutine(MoveToPlayer());
            }

            yield return null;
        }
    }

    IEnumerator WaitBomb()
    {

        float randomX = Random.Range(-1f, 2f);
        float randomY = Random.Range(-1f, 2f);

        navMeshAgent.SetDestination(this.transform.position + new Vector3(randomX, 0, randomY));
        aiAni.OnMove(this.transform.position + new Vector3(randomX, 0, randomY));

        yield return new WaitForSeconds(1f);

        ResetState();
        StopCoroutine(WaitBomb());
    }
    IEnumerator SetState()
    {
        while (!isHold && !isHit && !isGetUp && !isThrow && !isMove)
        {

            if (nearBomb != null)
            {
                navMeshAgent.ResetPath();
                aiAni.OnMove(Vector3.zero);
                bomb = nearBomb;
                bomb.SetOnBomb(bombPosition);
                bomb.isOnce = false;
                HoldBomb();
                StopCoroutine(SetState());
                yield return null;
            }

            navMeshAgent.SetDestination(bombs[targetIndex].transform.position);
            Vector3 dir = (bombs[targetIndex].transform.position - transform.position).normalized;
            aiAni.OnMove(dir);

            yield return null;
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
}
