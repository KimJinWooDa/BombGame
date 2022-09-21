using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] BombData bombData;
    public BombData BombData { set { bombData = value; } }

    public float explosionPower, explosionRange;
    Rigidbody rb;
    ParticleSystem ps;
    public bool isOnce;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        isOnce = true;
        this.explosionPower = bombData.ExplosionPower;
        this.explosionRange = bombData.ExplosionRange;
    }
    void Enable()
    {
        ps.Stop();
       // isOnce = true;
    }

    public void Explosion(Transform bombPositoin, Vector3 targetPos)
    {
  
        if (this.gameObject.activeSelf)
        {
            SetOffBomb(bombPositoin);
            StartCoroutine(ThrowBomb(targetPos));
        }

    }
    public IEnumerator ThrowBomb(Vector3 targetPos)
    {
        Vector3 start = this.transform.position;
        Vector3 end = targetPos;

        float throwSpeed = 2f;
        float throwPower = Mathf.Max(0.3f, Vector3.Distance(start, end)/ throwSpeed);
        float gravity = -9.81f;
        float currentTime = 0f;
        float percent = 0f;
        float v0 = (end.y - start.y) - gravity;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / throwPower;

            Vector3 position = Vector3.Lerp(start,end,percent);
            position.y = start.y + (v0 * percent) + (gravity * percent * percent);

            transform.position = position;

            yield return null;
        }
        StartCoroutine(WaitExplosion());
    }

    IEnumerator WaitExplosion()
    {

        yield return new WaitForSeconds(1f);
        //Æã È¿°ú
        ps.Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange , 1<<6);

        foreach (Collider item in colliders)
        {
            if (item.GetComponent<R_CharacterController>())
            {
                R_CharacterController tr = item.GetComponent<R_CharacterController>();

                tr.GetComponentInChildren<R_AnimationController>().OnHit();
                tr.isHit = true;
                tr.isThrow = false;
                tr.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, this.transform.position, explosionRange);
            }
            else if (item.GetComponent<AI>())
            {
                AI tr = item.GetComponent<AI>();

                tr.GetComponentInChildren<AI_Animation>().OnHit();
                tr.isHit = true;
                tr.isThrow = false;
                
                tr.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, this.transform.position, explosionRange);
            }

        }
        yield return new WaitForSeconds(0.1f);
        switch (bombData.bombType)
        {
            case BombData.BombType.Normal:
                NormalPooling.ReturnObject(this.gameObject.transform);
                break;
            case BombData.BombType.Power:
                PowerPooling.ReturnObject(this.gameObject.transform);
                break;
            case BombData.BombType.Special:
                SpecialPooling.ReturnObject(this.gameObject.transform);
                break;
            default:
                break;
        }
    }
    

    public void SetOnBomb(Transform bombPosition)
    {
        transform.SetParent(bombPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.useGravity = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void SetOffBomb(Transform bombPosition)
    {
        bombPosition.DetachChildren();

        GetComponent<SphereCollider>().enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (bombData.bombType)
        {
            case BombData.BombType.Normal:
                NormalPooling.ReturnObject(this.gameObject.transform);
                StopAllCoroutines();
                break;
            case BombData.BombType.Power:
                PowerPooling.ReturnObject(this.gameObject.transform);
                StopAllCoroutines();
                break;
            case BombData.BombType.Special:
                SpecialPooling.ReturnObject(this.gameObject.transform);
                StopAllCoroutines();
                break;
            default:
                break;
        }
    }
}
