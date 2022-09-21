using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombData", menuName = "BombScriptable/CreateBomb", order = int.MinValue)]
public class BombData : ScriptableObject
{
    [SerializeField]
    private float explosionPower;
    public float ExplosionPower => explosionPower;

    [SerializeField]
    private float explosionRange;
    public float ExplosionRange => explosionRange;

    [SerializeField]
    private string bombName;
    public string BombName => bombName;

    public enum BombType { Normal, Power, Special};
    public BombType bombType;
}
