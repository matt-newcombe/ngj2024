using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

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
        room.transform.rotation = transform.rotation;
        room.transform.position = transform.localPosition * placementScale;
    }
}
