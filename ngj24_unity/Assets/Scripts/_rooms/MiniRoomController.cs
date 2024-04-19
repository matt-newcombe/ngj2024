using UnityEditor.Rendering;
using UnityEngine;

public class MiniRoomController : MonoBehaviour
{
    public float rotSpeed = 30f;
    private Quaternion _currentRotation = Quaternion.identity;
    private Quaternion _goalRotation = Quaternion.identity;

    void Start()
    {
        
    }

    void Update()
    {
        ProcessUpdate();
        _currentRotation = Quaternion.Slerp(_currentRotation, _goalRotation, rotSpeed * Time.deltaTime);
        transform.rotation = _currentRotation;
    }

    void ProcessUpdate()
    {
        // Roll
        if (Input.GetKeyDown(KeyCode.A))
        {
            _goalRotation *= Quaternion.AngleAxis(90f, Vector3.up);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _goalRotation *= Quaternion.AngleAxis(-90f, Vector3.up);
        }
        
        // Pitch
        if (Input.GetKeyDown(KeyCode.W))
        {
            _goalRotation *= Quaternion.AngleAxis(90f, Vector3.right);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _goalRotation *= Quaternion.AngleAxis(-90f, Vector3.right);
        }
        
        // Yaw
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _goalRotation *= Quaternion.AngleAxis(90f, Vector3.forward);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            _goalRotation *= Quaternion.AngleAxis(-90f, Vector3.forward);
        }
    }
}
