using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniRoomManipulator : MonoBehaviour
{
    public FirstPersonController FirstPersonController;
    public GridController GridController;
    public Camera cam;
    public float focusDistPastMouse = 5f;
    [FormerlySerializedAs("holdingOffsetWS")] public Vector3 HoldingOffsetWS = new Vector3(-5f, -5f, 0f);
    
    private MiniRoomController _room;
    private Vector3 _placeRoomPos;
    private bool _holding = false;
    private bool _validPlacePos = false;

    public Material HighlightRenderMaterial;

    void Start()
    {
        FirstPersonController.PickedUpCarryable += PickedUpCarryable;
        FirstPersonController.DroppedCarryable += DroppedCarryable;
    }

    private void PickedUpCarryable(Interactable obj)
    {
        if (obj.TryGetComponent(out MiniRoomController miniRoom))
        {
            if (miniRoom.transform == GridController.PinnedRoom) return;
            
            _holding = true;
            miniRoom.PickedUp();
            miniRoom.GetComponent<Rigidbody>().isKinematic = false;
            
            _placeRoomPos = miniRoom.transform.position;
            _room = miniRoom;
        }
    }

    private void DroppedCarryable(Interactable obj)
    {
        if (obj.TryGetComponent(out MiniRoomController miniRoom))
        {
            _holding = false;
            if (_validPlacePos)
            {
                miniRoom.GetComponent<Rigidbody>().isKinematic = true;
                miniRoom.DropInPlace(_placeRoomPos);
            }
        }
    }

    void Update()
    {
        if (!_holding)
        {

        }
        else
        {
            _validPlacePos = FindValidSpaceToPlaceIn(out Vector3 placedPos);
            if (_validPlacePos)
            {
                _placeRoomPos = cam.ViewportToWorldPoint(placedPos);
                HighlightBestPos(_placeRoomPos);
            }   
        }
    }

    void HighlightBestPos(Vector3 targetPos)
    {
        RenderParams rp = new RenderParams(HighlightRenderMaterial);
        Matrix4x4 mat = Matrix4x4.TRS(targetPos, _room.transform.rotation, _room.transform.localScale);
        Graphics.RenderMesh(rp, _room.GetComponent<MeshFilter>().sharedMesh, 0, mat);
    }

    bool FindValidSpaceToPlaceIn(out Vector3 bestPosVP)
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
        bool foundOne = false;
        float bestDist = float.MaxValue;
        bestPosVP = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        
        for (int i = 0; i < openPositionsWS.Count; i++)
        {
            Vector3 openPosWS = openPositionsWS[i];
            Vector3 openPosVP = openPostionsVP[i];
            
            Vector3 vecToTarget = openPosWS - mouseWP;
            Vector3 dirToTarget = vecToTarget.normalized;
            
            Ray ray = new Ray(mouseWP, vecToTarget.normalized);
            float viewportDistance = Vector3.Distance(new Vector3(openPosVP.x, openPosVP.y, 0f), mouseVP);
            
            if (vecToTarget.magnitude > 5f) continue;
            if (Vector3.Dot(dirToTarget, FirstPersonController.transform.forward) < 0.5f) continue;
            // Check that we can ray cast without hitting anything
            if (Physics.SphereCast(ray, radius:.125f, maxDistance: vecToTarget.magnitude)) continue;
            if (!(viewportDistance < bestDist)) continue;
            
            // Store new best placements
            bestDist = viewportDistance;
            bestPosVP = openPosVP;
            foundOne = true;
        }

        return foundOne;
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
