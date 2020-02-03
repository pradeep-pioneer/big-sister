using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Follow on Player's X/Z plane
// 2. Smooth rotations around the player in 45 degree increments

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;

    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float smoothSpeed = 0.5f;

    Quaternion targetRotation;
    Vector3 targetPosition;
    bool smoothRotating = false;

    void Update()
    {
        MoveWithTarget();
        LookAtTarget();

        if(Input.GetKeyDown(KeyCode.G) && !smoothRotating)
        {
            StartCoroutine("RotateAroundTarget", 2);
        }
        if(Input.GetKeyDown(KeyCode.H) && !smoothRotating)
        {
            StartCoroutine("RotateAroundTarget", -2);
        }
    }

    void MoveWithTarget()
    {
        targetPosition = target.position + offsetPos;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed * Time.deltaTime);
    }

    IEnumerator RotateAroundTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        while(dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }

        smoothRotating = false;

        offsetPos = targetOffsetPos;
    }
}
