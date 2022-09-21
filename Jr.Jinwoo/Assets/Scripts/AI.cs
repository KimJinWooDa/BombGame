using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    protected float moveSpeed;
    protected List<Bomb> bombs = new List<Bomb>();
    protected List<R_CharacterController> players = new List<R_CharacterController>();
    public Bomb nearBomb;
    public Bomb bomb;
    protected Transform targetPlayer;

    protected AI_Animation aiAni;
    public Transform bombPosition;

    public bool isMove, isJump, isHold, isThrow, isDie, isGetUp, isHit;
    public float throwPower = 2f;
    public virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiAni = GetComponentInChildren<AI_Animation>();
        foreach (var item in FindObjectsOfType<R_CharacterController>())
        {
            players.Add(item);
        }
    }

    public abstract void HoldBomb();

    public abstract void ResetState();
}
