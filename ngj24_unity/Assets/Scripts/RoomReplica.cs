using UnityEngine;

public class RoomReplica : MonoBehaviour
{
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
        if (!room || !GameManager.Instance)
            return;

        Room centerRoom = GameManager.Instance.centerRoom;
        Transform pivot = GameManager.Instance.centerPivot;

        if (!centerRoom || !pivot || room == centerRoom)
            return;

        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        Vector3 worldPos = pivot.TransformPoint(localPos * GameManager.Instance.GetRoomPlacementScale() - pivot.localPosition);

        room.transform.SetPositionAndRotation(worldPos, transform.rotation);

        if (Application.isPlaying)
        {
            MiniRoomController controller = GetComponent<MiniRoomController>();

            if (controller)
            {
                if (room.gameObject.activeSelf != controller.Placed)
                {
                    if (controller.Placed)
                        room.RecoverObjectInfo();
                    else
                        room.StoreObjectInfo();

                    room.gameObject.SetActive(controller.Placed);
                }
            }
        }
        else
        {
            room.gameObject.SetActive(true);
        }
    }
}
