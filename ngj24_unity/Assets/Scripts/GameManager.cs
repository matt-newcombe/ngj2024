using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform centerPivot;
    public Room centerRoom;
    public float roomPlacementScale = 33.55555f;

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
}
