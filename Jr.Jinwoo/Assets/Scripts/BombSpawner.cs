using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public enum BombType { Normal, Power, Special};
    public GameObject[] bombs; //데이터들
    public List<BombData> bombDatas = new List<BombData>();
    float timer;
    [SerializeField] float time = 7f;
    float x, y;

    public Transform up, down, left, right;
    private void Start()
    {
        timer = time;

        x = Vector3.Distance(right.position, left.position);
        y = Vector3.Distance(up.position, down.position);
        SpawnBomb(3);
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = time;
            int randomSpawnCount = Random.Range(0, 4);
            
            SpawnBomb(randomSpawnCount);
        }
    }


    void SpawnBomb(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int bombType = Random.Range(0, bombs.Length - 1);
            Bomb newBomb = null;
            switch (bombType)
            {
                case (int)BombType.Normal:
                    newBomb = NormalPooling.Instance.GetObject().GetComponent<Bomb>();
                    newBomb.BombData = bombDatas[(int)bombType];
                    break;
                case (int)BombType.Power:
                    newBomb = PowerPooling.Instance.GetObject().GetComponent<Bomb>();
                    newBomb.BombData = bombDatas[(int)bombType];
                    break;

                case (int)BombType.Special:
                    newBomb = SpecialPooling.Instance.GetObject().GetComponent<Bomb>();
                    newBomb.BombData = bombDatas[(int)bombType];
                    break;

                default:
                    break;
            }

            newBomb.transform.position = new Vector3(Random.Range(-x/2f, x/2f), 0.5f, Random.Range(-y/2f, y/2f));
            newBomb.transform.eulerAngles = new Vector3(-90f, 0, 0);
        }
    }
}
