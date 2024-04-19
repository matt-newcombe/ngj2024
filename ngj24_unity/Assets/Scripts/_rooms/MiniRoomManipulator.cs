using UnityEngine;

public class MiniRoomManipulator : MonoBehaviour
{
    public Transform cameraFocusPoint;
    public Camera cam;
    public float moveVelocity = 1f;
    public float focusDistPastMouse = 5f; 
    
    public VecSpringDamp vecSpring;
    private MiniRoomController _room;
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
            SpinRoom();
            
            if (LeftClick())
            {
                _holding = false;
            }
        }
        
        if (_room != null)
        {
            if (_holding)
            {
                vecSpring.MoveTowards(_focusPoint, moveVelocity);
            }
            else
            {
                vecSpring.MoveTowards(_placeRoomPos, moveVelocity);
            }

            _room.transform.position = vecSpring.Pos();
        }
    }

    void CheckPickup()
    {
        if (LeftClick() && ClickedOnMiniRoom(out MiniRoomController room))
        {
            _placeRoomPos = room.transform.position;
            Debug.Log(_placeRoomPos);
            _room = room;
            _holding = true;
            vecSpring.Init(room.transform.position);
        }
    }

    bool LeftClick()
    {
        return Input.GetMouseButtonUp(0);
    }

    bool ClickedOnMiniRoom(out MiniRoomController room)
    {
        room = null;
        
        Ray ray = cam.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, maxDistance: 100f))
        {
            room = hit.transform.GetComponent<MiniRoomController>();
        }

        return room != null;
    }

    void UpdateMouseFocusPoint()
    {
        _focusPoint =
            cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, focusDistPastMouse));
    }
    
    void SpinRoom()
    {
        // Roll
        if (Input.GetKeyDown(KeyCode.A))
        {
            _room.PushRoll();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _room.PullRoll();
        }
        
        // Pitch
        if (Input.GetKeyDown(KeyCode.W))
        {
            _room.PushPitch();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _room.PullPitch();
        }
        
        // Yaw
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _room.PushYaw();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            _room.PullYaw();
        }
    }
}
