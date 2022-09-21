using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance => instance;

    float shakeTime;
    float shakeIntensity;
    float power = 10f;
    SimpleCamera controller;
    public CameraShake()
    {
        instance = this;
    }

    private void Awake()
    {
        controller = GetComponent<SimpleCamera>();
    }

    public void OnShakeCamera(float intensity = 0.1f, float shakeTime = .03f)
    {
        this.shakeIntensity = intensity;
        this.shakeTime = shakeTime;

        //StopCoroutine(ShakeByRotation());
        //StartCoroutine(ShakeByRotation());

        StopCoroutine(ShakeByPosition());
        StartCoroutine(ShakeByPosition());
    }

    IEnumerator ShakeByPosition()
    {
        controller.IsOnShake = true;
        Vector3 originPos = this.transform.position;

        while (shakeTime > 0f)
        {
            transform.position = originPos + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = originPos;
        controller.IsOnShake = false;
    }

    IEnumerator ShakeByRotation()
    {
        controller.IsOnShake = true;
        Vector3 originRotation = this.transform.eulerAngles;

        while (shakeTime > 0f)
        {
            float x = 0f;
            float y = 0f;
            float z = Random.Range(-1, 1);
            transform.eulerAngles = originRotation + new Vector3(x,y,z)* shakeIntensity * power;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.eulerAngles = originRotation;
        controller.IsOnShake = false;
    }
}
