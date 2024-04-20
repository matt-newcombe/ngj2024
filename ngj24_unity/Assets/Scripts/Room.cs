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
            CreateReplica();
    }

    void CreateRelicaChildren(Transform target, Transform newParent)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            Transform child = target.GetChild(i);

            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (!meshFilter || !meshRenderer)
                continue;

            GameObject childReplica = new GameObject();
            childReplica.name = child.name;
            childReplica.transform.parent = newParent.transform;

            MeshFilter meshFilterReplica = childReplica.AddComponent<MeshFilter>();
            meshFilterReplica.sharedMesh = meshFilter.sharedMesh;

            MeshRenderer meshRendererReplica = childReplica.AddComponent<MeshRenderer>();
            meshRendererReplica.sharedMaterials = meshRenderer.sharedMaterials;

            childReplica.transform.SetLocalPositionAndRotation(child.localPosition, child.localRotation);
            childReplica.transform.localScale = child.localScale;

            //Recursive for grand children
            if (child.childCount > 0) 
                CreateRelicaChildren(child, childReplica.transform);
        }
    }
    
    void CreateReplica()
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

        CreateRelicaChildren(gameObject.transform, holder.transform);

        holder.transform.localScale = Vector3.one * replicaScale;
        holder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        holder.hideFlags = HideFlags.HideInHierarchy;
        
        //Debug.Log("Replicate");
    }
}