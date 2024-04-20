using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform centerPivot;
    public Room centerRoom;
    
    public float roomSize = 30f;
    public float replicaSize = 0.5f;

    private static GameManager instance;
    
    public static GameManager Instance 
    { 
        get 
        {
            if (!instance) 
                instance = FindAnyObjectByType<GameManager>();
            
            return instance; 
        }
    }

    public float GetReplicaScale()
    {
        return (1f / roomSize) * replicaSize;
    }

    public float GetRoomPlacementScale()
    {
        return roomSize / replicaSize;
    }
}
