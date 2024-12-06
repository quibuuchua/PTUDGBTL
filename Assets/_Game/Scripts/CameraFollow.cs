using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform tf;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed = 20;

    // Update is called once per frame
    void LateUpdate()
    {
        tf.position = Vector3.Lerp(tf.position, target.position + offset, Time.deltaTime * moveSpeed);
    }
}
