using UnityEngine;

public class RoomReplica : MonoBehaviour
{
    //public float placementScale = 20;

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

        if (Application.isPlaying) 
        {
            MiniRoomController controller = GetComponent<MiniRoomController>();

            if (controller) 
            {
                room.gameObject.SetActive(controller.IsInPlace());

                if (!controller.IsInPlace())
                    return;
            }
        }
        else
        {
            room.gameObject.SetActive(true);
        }

        Room centerRoom = GameManager.Instance.centerRoom;
        Transform pivot = GameManager.Instance.centerPivot;

        if (!centerRoom || !pivot || room == centerRoom)
            return;

        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        Vector3 worldPos = pivot.TransformPoint(localPos * GameManager.Instance.GetRoomPlacementScale() - pivot.localPosition);

        room.transform.SetPositionAndRotation(worldPos, transform.rotation);
    }
}
