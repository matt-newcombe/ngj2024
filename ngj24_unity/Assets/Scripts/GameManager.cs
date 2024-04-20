using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform centerPivot;
    public Room centerRoom;

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
