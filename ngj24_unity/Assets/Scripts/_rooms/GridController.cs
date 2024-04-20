using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int gridSizeMetres = 1;
    public int maxGridSize = 5;

    public List<MiniRoomController> MiniRooms;
    
    public List<Vector3> FindAllOpenPositions()
    {
        List<Vector3> closedPositions = FindAllClosedPositions();
        List<Vector3> openPostions = new List<Vector3>();
        List<Vector3> checkPositions = new List<Vector3>();
        
        foreach (Vector3 pos in closedPositions)
        {
            checkPositions.Clear();
            // Check up, down, left, right, forward, backward
            checkPositions.Add(new Vector3(pos.x +1, pos.y, pos.z));
            checkPositions.Add(new Vector3(pos.x -1, pos.y, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y+1, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y-1, pos.z));
            checkPositions.Add(new Vector3(pos.x, pos.y, pos.z+1));
            checkPositions.Add(new Vector3(pos.x, pos.y, pos.z-1));

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
        
        for (int x = -maxGridSize; x < maxGridSize; x += gridSizeMetres)
        {
            for (int y = -maxGridSize; y < maxGridSize; y += gridSizeMetres)
            {
                for (int z = -maxGridSize; z < maxGridSize; z += gridSizeMetres)
                {
                    foreach (var miniRoom in MiniRooms)
                    {
                        if (miniRoom.GetComponent<Collider>().bounds.Contains(new Vector3(x, y, z)))
                        {
                            closedPositions.Add(new Vector3(x,y,z));
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
    }
}
