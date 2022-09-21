using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    Vector3 offset;

    float cameraSpeed = 2f;

    Transform myCharacter;

    public bool IsOnShake { get; set; }
    private void Start()
    {
        offset = transform.position;
        myCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //���߿� ���������ϸ� �������
    }

    private void Update()
    {
        if (IsOnShake) return;
        transform.position = Vector3.Lerp(this.transform.position, myCharacter.position + offset, cameraSpeed * Time.deltaTime);
    }
}
