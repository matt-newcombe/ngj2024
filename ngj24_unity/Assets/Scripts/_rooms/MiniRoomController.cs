using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniRoomController : MonoBehaviour
{
    public float rotSpeed = 30f;
    private Quaternion _currentRotation = Quaternion.identity;
    private Quaternion _goalRotation = Quaternion.identity;
    public VecSpringDamp vecSpring;
    public float moveVelocity = .1f;


    [FormerlySerializedAs("_isPlaced")] public bool Placed = false;
    private Vector3 _placedPosition;
    
    private void Start()
    {
        _currentRotation = _goalRotation = transform.rotation;
    }

    void Update()
    {
        // _currentRotation = Quaternion.Slerp(_currentRotation, _goalRotation, rotSpeed * Time.deltaTime);
        // transform.rotation = _currentRotation;

        if (Placed)
        {
            transform.position = vecSpring.MoveTowards(_placedPosition, moveVelocity);
        }
    }

    public void PickedUp()
    {
        Placed = false;
    }

    public void DropInPlace(Vector3 place)
    {
        Placed = true;
        vecSpring.Init(transform.position);
        _placedPosition = place;
    }

    public void PushPitch()
    {
        _goalRotation = Quaternion.AngleAxis(90f, Vector3.right) * _goalRotation;
    }

    public void PullPitch()
    {
        _goalRotation = Quaternion.AngleAxis(-90f, Vector3.right) * _goalRotation;
    }

    public void PushRoll()
    {
        _goalRotation = Quaternion.AngleAxis(-90f, Vector3.up) * _goalRotation;
    }

    public void PullRoll()
    {
        _goalRotation = Quaternion.AngleAxis(90f, Vector3.up) * _goalRotation;
    }
    
    public void PushYaw()
    {
        _goalRotation = Quaternion.AngleAxis(90f, Vector3.forward) * _goalRotation;
    }

    public void PullYaw()
    {
        _goalRotation = Quaternion.AngleAxis(-90f, Vector3.forward) * _goalRotation;
    }
}
