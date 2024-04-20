using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniRoomManipulator : MonoBehaviour
{
    public GridController GridController;
    public Camera cam;
    public float moveVelocity = 1f;
    public float focusDistPastMouse = 5f;
    [FormerlySerializedAs("holdingOffsetWS")] public Vector3 HoldingOffsetWS = new Vector3(-5f, -5f, 0f);
    
    public VecSpringDamp vecSpring;
    private MiniRoomController _room;
    private Vector3 _focusPoint;

    private Vector3 _placeRoomPos;

    private bool _holding = false;

    public Material HighlightRenderMaterial;

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
            _placeRoomPos = FindValidSpaceToPlaceIn();
            HighlightBestPos(_placeRoomPos);
            
            if (LeftClick())
            {
                _holding = false;
            }
        }
        
        if (_room != null)
        {
            _room.transform.position = vecSpring.MoveTowards(_holding ? _focusPoint : _placeRoomPos, moveVelocity);
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
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, maxDistance: 100f) 
            && hit.transform != GridController.PinnedRoom
            && hit.transform.TryGetComponent(out MiniRoomController roomController))
        {
            room = roomController;
        }

        return room != null;
    }

    void UpdateMouseFocusPoint()
    {
        _focusPoint =
            cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, focusDistPastMouse)) + HoldingOffsetWS;
    }

    void HighlightBestPos(Vector3 targetPos)
    {
        RenderParams rp = new RenderParams(HighlightRenderMaterial);
        Matrix4x4 mat = Matrix4x4.TRS(targetPos, _room.transform.rotation, _room.transform.localScale);
        Graphics.RenderMesh(rp, _room.GetComponent<MeshFilter>().sharedMesh, 0, mat);
    }

    Vector3 FindValidSpaceToPlaceIn()
    {
        List<Vector3> openPositionsWS = GridController.FindAllOpenPositions();
        List<Vector3> openPostionsVP = new List<Vector3>(openPositionsWS.Count);
        
        // Get viewport positions to rank best ones
        for (int i = 0; i < openPositionsWS.Count; i++)
        {
            openPostionsVP.Add(cam.WorldToViewportPoint(openPositionsWS[i]));
        }
        
        Vector3 mouseVP = cam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 mouseWP = cam.ScreenToWorldPoint(Input.mousePosition);
        
        // Setup tracking which one is best for mouse pos
        float bestDist = float.MaxValue;
        Vector3 bestPosVP = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        
        for (int i = 0; i < openPositionsWS.Count; i++)
        {
            Vector3 openPosWS = openPositionsWS[i];
            Vector3 openPosVP = openPostionsVP[i];
            
            Vector3 vecToTarget = openPosWS - mouseWP;
            Ray ray = new Ray(mouseWP, vecToTarget.normalized);
            
            float viewportDistance = Vector3.Distance(new Vector3(openPosVP.x, openPosVP.y, 0f), mouseVP);
            
            // Check that we can ray cast without hitting anything
            if (Physics.SphereCast(ray, radius:.125f, maxDistance: vecToTarget.magnitude)) continue;
            if (!(viewportDistance < bestDist)) continue;
            
            // Store new best placements
            bestDist = viewportDistance;
            bestPosVP = openPosVP;
        }

        return cam.ViewportToWorldPoint(bestPosVP);
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
