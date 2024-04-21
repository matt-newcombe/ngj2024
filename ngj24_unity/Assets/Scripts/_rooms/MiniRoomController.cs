using UnityEngine;

public class MiniRoomController : MonoBehaviour
{
    public float rotSpeed = 30f;
    private Quaternion _currentRotation = Quaternion.identity;
    private Quaternion _goalRotation = Quaternion.identity;
    public VecSpringDamp vecSpring;
    public float moveVelocity = .1f;

    private Vector3 _placedPosition;

    public enum State
    {
        Loose,
        Carrying,
        Placing,
        Placed
    }

    public State state = State.Loose;
    
    private void Start()
    {
        _currentRotation = _goalRotation = transform.rotation;
        _placedPosition = transform.position;
        vecSpring.Init(transform.position);
    }

    void Update()
    {
        switch (state)
        {
            case State.Carrying:
            {
                _currentRotation = Quaternion.Slerp(_currentRotation, _goalRotation, rotSpeed * Time.deltaTime);
                transform.rotation = _currentRotation;
            } break;
            case State.Placing:
            {
                transform.position = vecSpring.MoveTowards(_placedPosition, moveVelocity);

                if (CloseToTargetPos())
                {
                    state = State.Placed;
                }
            } break;
            case State.Placed:
            {
                transform.position = vecSpring.MoveTowards(_placedPosition, moveVelocity);
            } break;
        }
    }

    public void StartCarry()
    {
        state = State.Carrying;
    }

    public void StopCarry()
    {
        state = State.Loose;
    }

    public void DropInPlace(Vector3 place)
    {
        state = State.Placing;
        vecSpring.Init(transform.position);
        _placedPosition = place;
    }

    public bool IsInPlace()
    {
        return state == State.Placed;
    }

    private bool CloseToTargetPos()
    {
        float distance = Vector3.Distance(vecSpring.Pos(), _placedPosition);
        return distance < 0.01f;
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
