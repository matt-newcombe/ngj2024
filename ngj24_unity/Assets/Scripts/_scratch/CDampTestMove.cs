using UnityEngine;

public class CDampTestMove : MonoBehaviour
{
    public float goalVelocity = 1f;
    public VecSpringDamp vecSpring;
    private Vector3 targetPos;
    
    void Start()
    {
        targetPos = transform.position;
        vecSpring.Init(transform.position);
    }

    void Update()
    {
        ProcessInput();
        transform.position = vecSpring.MoveTowards(targetPos, goalVelocity);
    }

    void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetPos.z += 1;
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetPos.z -= 1;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetPos.x -= 1;
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetPos.x += 1;
        }
    }
}
