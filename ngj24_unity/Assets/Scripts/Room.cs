using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private RoomReplica roomReplica;

    struct ObjectInfo
    {
        public Rigidbody body;
        public Vector3 localPos;
        public Quaternion localRot;
    }

    List<ObjectInfo> objectsInside = new List<ObjectInfo>();

    void OnDrawGizmos()
    {
        //Bounds
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * GameManager.Instance.roomSize);

        if (roomReplica != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = roomReplica.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * GameManager.Instance.replicaSize);
        }

        if (Application.isPlaying)
            return;

        bool changeDetected = false;

        //Validate
        if (roomReplica == null)
        {
            GameObject newRoomReplica = new GameObject();
            roomReplica = newRoomReplica.AddComponent<RoomReplica>();
            roomReplica.room = this;

            Rigidbody newRigidbody = newRoomReplica.AddComponent<Rigidbody>();
            newRigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            newRoomReplica.AddComponent<BoxCollider>();

            MiniRoomController newController = newRoomReplica.AddComponent<MiniRoomController>();

            newController.vecSpring = new VecSpringDamp();
            newController.vecSpring.Init(newRoomReplica.transform.position);
            newController.vecSpring.SetHalfLife(0.5f);

            Interactable newInteractable = newRoomReplica.AddComponent<Interactable>();

            changeDetected = true;
        }

        if (roomReplica.TryGetComponent(out BoxCollider collider))
            collider.size = Vector3.one * GameManager.Instance.replicaSize;
        
        roomReplica.gameObject.name = gameObject.name + "Replica";
        
        //Check for child count differences for all rooms
        if (roomReplica.transform.childCount > 0)
        {
            Transform holder = roomReplica.transform.GetChild(0);
            float replicaScale = GameManager.Instance.GetReplicaScale();
            if (Mathf.Approximately(holder.localScale.x, replicaScale) == false)
                changeDetected = true;

            //if (holder.childCount != transform.childCount)
            //    changeDetected = true;
        }
        
        #if UNITY_EDITOR
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
        #endif
        
        if (changeDetected)
            CreateReplica();
    }

    void CreateRelicaChildren(Transform target, Transform newParent)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            Transform child = target.GetChild(i);

            RoomReplica roomReplica = GetComponent<RoomReplica>();
            if (roomReplica) 
                continue;

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
            
            if (child.GetComponent<ReplicaDoNotDestroy>()) continue;

            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        //Replicate
        GameObject holder = new GameObject();
        holder.transform.parent = roomReplica.transform;

        CreateRelicaChildren(gameObject.transform, holder.transform);
        
        float replicaScale = GameManager.Instance.GetReplicaScale();
        holder.transform.localScale = Vector3.one * replicaScale;
        holder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        holder.hideFlags = HideFlags.HideInHierarchy;
            
        //Debug.Log("Replicate "+gameObject.name);
    }

    public void StoreObjectInfo()
    {
        objectsInside.Clear();

        Collider[] collidersInsideRoom = Physics.OverlapBox(transform.position, Vector3.one * GameManager.Instance.roomSize * 0.5f, transform.rotation);

        for (int i = 0; i < collidersInsideRoom.Length; i++)
        {
            Collider colliderInsideRoom = collidersInsideRoom[i];
            Rigidbody attachedRigidbody = colliderInsideRoom.attachedRigidbody;
            if (attachedRigidbody)
            {
                ObjectInfo objectInfo = new ObjectInfo();

                objectInfo.body = attachedRigidbody;

                objectInfo.localPos = transform.InverseTransformPoint(attachedRigidbody.transform.position);
                objectInfo.localRot = Quaternion.Inverse(transform.rotation) * attachedRigidbody.transform.rotation;

                objectsInside.Add(objectInfo);

                objectInfo.body.gameObject.SetActive(false);

                //Debug.Log("stored  " + colliderInsideRoom.gameObject.name + " in " + gameObject.name);
            }
        }
    }

    public void RecoverObjectInfo()
    {
        for (int i = 0; i < objectsInside.Count; i++)
        {
            ObjectInfo objectInfo = objectsInside[i];

            if (!objectInfo.body)
                continue;

            Vector3 worldPos = transform.TransformPoint(objectInfo.localPos);
            Quaternion worldRot = transform.rotation * objectInfo.localRot;

            objectInfo.body.transform.SetPositionAndRotation(worldPos, worldRot);
            objectInfo.body.gameObject.SetActive(true);

            //Debug.Log("recover  " + objectInfo.body.gameObject.name + " in " + gameObject.name);
        }
    }
}