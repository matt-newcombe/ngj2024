using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

public class MiniRoomManipulator : MonoBehaviour
{
    public Transform cameraFocusPoint;
    public Camera cam;
    public float moveVelocity = 1f;
    public float focusDistPastMouse = 5f; 
    
    public VecSpringDamp vecSpring;
    private Transform _hitTransform;
    private Vector3 _focusPoint;

    private Vector3 _placeRoomPos;

    private bool _holding = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!_holding)
        {
            CheckPickup();
        }
        else
        {
            UpdateMouseFocusPoint();
            
            if (LeftClick())
            {
                _holding = false;
            }
        }
        
        if (_hitTransform != null)
        {
            if (_holding)
            {
                vecSpring.MoveTowards(_focusPoint, moveVelocity);
            }
            else
            {
                vecSpring.MoveTowards(_placeRoomPos, moveVelocity);
            }

            _hitTransform.transform.position = vecSpring.Pos();
        }
    }

    void CheckPickup()
    {
        if (LeftClick() && ClickedOnMiniRoom(out Transform room))
        {
            _placeRoomPos = room.position;
            Debug.Log(_placeRoomPos);
            _hitTransform = room;
            _holding = true;
            vecSpring.Init(room.transform.position);
        }
    }

    bool LeftClick()
    {
        return Input.GetMouseButtonUp(0);
    }

    bool ClickedOnMiniRoom(out Transform room)
    {
        room = null;
        
        Ray ray = cam.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, maxDistance: 100f))
        {
            if (hit.transform.GetComponent<MiniRoomController>())
            {
                room = hit.transform;
            }
        }

        return room != null;
    }

    void UpdateMouseFocusPoint()
    {
        _focusPoint =
            cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, focusDistPastMouse));
    }

    void Drop()
    {
    }
}
