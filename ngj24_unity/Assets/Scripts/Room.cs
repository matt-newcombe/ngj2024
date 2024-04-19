using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float roomSize = 30f;
    public float replicaScale = 0.05f;
    
    [HideInInspector, SerializeField]
    private RoomReplica roomReplica;

    void OnDrawGizmos()
    {
        //Validate
        if (roomReplica == null)
        {
            GameObject newRoomReplica = new GameObject();
            roomReplica = newRoomReplica.AddComponent<RoomReplica>();
            roomReplica.room = this;
        }

        roomReplica.gameObject.name = gameObject.name + "Replica";

        //Bounds
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * roomSize);

        if(roomReplica != null) 
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = roomReplica.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * roomSize * replicaScale);
        }
        
        //Check for child count differences for all rooms
        bool changeDetected = false;
        if (roomReplica.transform.childCount > 0)
        {
            Transform holder = roomReplica.transform.GetChild(0);
            if (Mathf.Approximately(holder.localScale.x, replicaScale) == false)
                changeDetected = true;

            if (holder.childCount != transform.childCount)
                changeDetected = true;
        }

        //Check for transform changes for selected room
        if (Selection.activeTransform)
        { 
            Room room = GetComponentInParent<Room>();
            if (room) 
            {
                Transform[] children = GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < children.Length; i++)
                {
                    Transform child = children[i];
                    if (!changeDetected && child.hasChanged)
                        changeDetected = true;

                    child.hasChanged = false;
                }
            }
        }

        if (changeDetected)
            RefreshReplica();
    }

    void RefreshReplica()
    {
        //Clear
        for (int i = roomReplica.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = roomReplica.transform.GetChild(i);

            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        //Replicate
        GameObject holder = new GameObject();
        holder.transform.parent = roomReplica.transform;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);

            GameObject childReplica = Instantiate(child.gameObject, holder.transform);
            childReplica.transform.SetLocalPositionAndRotation(child.localPosition, child.localRotation);

            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter.sharedMesh == null)
            {
                MeshFilter meshFilterReplica = childReplica.GetComponent<MeshFilter>();
                meshFilter.sharedMesh = meshFilterReplica.sharedMesh;
            }
        }

        holder.transform.localScale = Vector3.one * replicaScale;
        holder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        holder.hideFlags = HideFlags.HideInHierarchy;

        //Debug.Log("Replicate");
    }
}
