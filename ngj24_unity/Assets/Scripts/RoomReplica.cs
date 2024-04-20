using UnityEngine;

public class RoomReplica : MonoBehaviour
{
    public bool attached;

    [HideInInspector, SerializeField]
    public Room room;
    
    public int placementScale = 20;

    private void OnDrawGizmos()
    {
        if (!room)
            return;
       
        room.gameObject.SetActive(attached);

        if (!attached || !GameManager.Instance)
            return;

        Room centerRoom = GameManager.Instance.centerRoom;
        Transform pivot = GameManager.Instance.centerPivot;
        
        if (!centerRoom || !pivot || room == centerRoom)
            return;

        Vector3 offset =  centerRoom.transform.position + pivot.position;

        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        Vector3 worldPos = pivot.TransformPoint(localPos * placementScale - pivot.localPosition);

        room.transform.SetPositionAndRotation(worldPos, transform.rotation);
    }
}
