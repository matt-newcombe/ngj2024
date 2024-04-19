using UnityEditor.Rendering;
using UnityEngine;

public class MiniRoomController : MonoBehaviour
{
    public float rotSpeed = 30f;
    private Quaternion _currentRotation = Quaternion.identity;
    private Quaternion _goalRotation = Quaternion.identity;


    void Update()
    {
        _currentRotation = Quaternion.Slerp(_currentRotation, _goalRotation, rotSpeed * Time.deltaTime);
        transform.rotation = _currentRotation;
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
