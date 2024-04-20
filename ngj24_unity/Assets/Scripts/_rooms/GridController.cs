using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public float GridSizeMetres = 1;
    public int MaxGridSize = 4;
    public Vector3 ManualGridOffset;

    public List<MiniRoomController> MiniRooms;
    public Transform PinnedRoom;

    public bool DebugShowGrid = false;

    void Start()
    {
        
    }
    
    public List<Vector3> FindAllOpenPositions()
    {
        List<Vector3> closedPositions = FindAllClosedPositions();
        List<Vector3> openPostions = new List<Vector3>();
        List<Vector3> checkPositions = new List<Vector3>();
        
        foreach (Vector3 pos in closedPositions)
        {
            checkPositions.Clear();
            // Check up, down, left, right, forward, backward
            checkPositions.Add(new Vector3(pos.x +GridSizeMetres, pos.y, pos.z));
            checkPositions.Add(new Vector3(pos.x -GridSizeMetres, pos.y, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y+GridSizeMetres, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y-GridSizeMetres, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y, pos.z+GridSizeMetres));
            checkPositions.Add(new Vector3(pos.x, pos.y, pos.z-GridSizeMetres));

            foreach (var checkPos in checkPositions)
            {
                bool safe = true;
                foreach (var miniRoom in MiniRooms)
                {
                    if (miniRoom.GetComponent<Collider>().bounds.Contains(checkPos))
                    {
                        safe = false;
                    }
                }
                
                if (safe)
                    openPostions.Add(checkPos);
            }
        }

        return openPostions;
    }

    List<Vector3> FindAllClosedPositions()
    {
        List<Vector3> closedPositions = new List<Vector3>();
        
        for (float x = -MaxGridSize; x < MaxGridSize; x += GridSizeMetres)
        {
            for (float y = -MaxGridSize; y < MaxGridSize; y += GridSizeMetres)
            {
                for (float z = -MaxGridSize; z < MaxGridSize; z += GridSizeMetres)
                {
                    foreach (var miniRoom in MiniRooms)
                    {
                        Vector3 pos = GridOffset + new Vector3(x, y, z);
                        if (miniRoom.GetComponent<Collider>().bounds.Contains(pos))
                        {
                            closedPositions.Add(pos);
                        }
                    }
                }
            }
        }

        return closedPositions;
    }

    void OnDrawGizmos()
    {
        List<Vector3> openPositions = FindAllOpenPositions();
        
        foreach (var openPos in openPositions)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(openPos, 0.1f);
        }

        if (DebugShowGrid)
        {
            for (float x = -MaxGridSize; x < MaxGridSize; x += GridSizeMetres)
            {
                for (float y = -MaxGridSize; y < MaxGridSize; y += GridSizeMetres)
                {
                    for (float z = -MaxGridSize; z < MaxGridSize; z += GridSizeMetres)
                    {
                        Gizmos.DrawWireSphere(GridOffset + new Vector3(x,y,z), 0.1f);
                    }
                }
            }
        }
    }

    private Vector3 GridOffset => PinnedRoom.position + ManualGridOffset;
}
