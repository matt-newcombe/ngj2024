using UnityEngine;

public class RoomReplica : MonoBehaviour
{
    public bool attached;    
    public int placementScale = 20;

    [HideInInspector, SerializeField]
    public Room room;

    private void Update()
    {
        //TODO replace this, only update when replica has been moved or attached/detached
        UpdateRoom();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            UpdateRoom();
    }

    public void UpdateRoom()
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

        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        Vector3 worldPos = pivot.TransformPoint(localPos * placementScale - pivot.localPosition);

        room.transform.SetPositionAndRotation(worldPos, transform.rotation);
    }
}
