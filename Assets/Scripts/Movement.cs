using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MovementSpeed = 10;
    public float RotationSpeed = 10;

    public Animator Animator;

    public bool IsMoving;
    public bool Hault;

    private int _movementInstructionId = 0;
    private int _rotationInstructionId = 0;
    private Vector3 _lastPosition;

    // Movement
    public IEnumerator Follow(GameObject obj, Vector3 offset)
    {
        var myId = ++_movementInstructionId;
        IsMoving = true;

        while (_movementInstructionId == myId)
        {
            yield return new WaitForEndOfFrame();
            _lastPosition = transform.position;
            var targetPosition = obj.transform.position + offset;
            if (!(Vector3.Distance(transform.position, targetPosition) >
                  (MovementSpeed * Time.deltaTime) + 0.0001)) continue;
            var targetDirection = (targetPosition - transform.position).normalized;
            transform.position += (targetDirection * MovementSpeed * Time.deltaTime);
        }

        if (_movementInstructionId == myId)
            IsMoving = false;
    }
    public IEnumerator FollowPath(List<Vector3> path)
    {
        var myId = ++_movementInstructionId;
        IsMoving = true;

        while (_movementInstructionId == myId && path.Count > 0)
        {
            yield return new WaitForEndOfFrame();
            _lastPosition = transform.position;
            var targetPosition = path.First();
            var targetDirection = (targetPosition - transform.position).normalized;
            transform.Translate(targetDirection * MovementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < (MovementSpeed * Time.deltaTime) + 0.0001)
                path.Remove(targetPosition);
        }

        if (_movementInstructionId == myId)
            IsMoving = false;
    }
    public IEnumerator MoveTo(Vector3 pos)
    {
        var myId = ++_movementInstructionId;
        IsMoving = true;

        while (_movementInstructionId == myId && Vector3.Distance(transform.position, pos) > (MovementSpeed * Time.deltaTime) + 0.0001)
        {
            yield return new WaitForEndOfFrame();
            _lastPosition = transform.position;
            var targetDirection = (pos - transform.position).normalized;
            transform.position += (targetDirection * MovementSpeed * Time.deltaTime);
        }

        if (_movementInstructionId == myId)
            IsMoving = false;
    }
    public IEnumerator RotateAround(Vector3 pos, Vector3 offset)
    {
        StartCoroutine(MoveTo(pos + offset));
        while (IsMoving)
        {
            yield return new WaitForFixedUpdate();
        }

        var myId = ++_movementInstructionId;
        IsMoving = true;

        while (_movementInstructionId == myId)
        {
            yield return new WaitForEndOfFrame();
            transform.RotateAround(pos, Vector3.up, Time.deltaTime * MovementSpeed * 5);
        }

        if (_movementInstructionId == myId)
            IsMoving = false;
    }

    //Rotation
    public IEnumerator LookForward(Vector3 offset)
    {
        var myId = ++_rotationInstructionId;

        while (_rotationInstructionId == myId)
        {
            yield return new WaitForEndOfFrame();
            if (!IsMoving) continue;

            var targetDir = (transform.position - _lastPosition + offset).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir, Vector3.up), Time.time * RotationSpeed / 5);
        }
    }
    public IEnumerator LookAt(Vector3 pos, Vector3 up)
    {
        var myId = ++_rotationInstructionId;

        while (_rotationInstructionId == myId)
        {
            yield return new WaitForEndOfFrame();
            var targetDir = (pos - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir, up), Time.time * RotationSpeed / 10);
        }
    }
    public IEnumerator Track(GameObject obj, Vector3 offset)
    {
        var myId = ++_rotationInstructionId;

        while (_rotationInstructionId == myId)
        {
            yield return new WaitForEndOfFrame();
            var targetDir = (obj.transform.position + offset - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir, obj.transform.up), Time.time * RotationSpeed / 10);
        }
    }
}