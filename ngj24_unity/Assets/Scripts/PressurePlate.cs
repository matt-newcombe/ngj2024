using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Cube
{
    public Rigidbody plate;
    public Rigidbody initialWeight;
    public float weightFactor = 0.1f;
    public float adjustSpeed = 1f;

    [Space]
    public Transform barsUp;
    public Transform barsDown;
    public Rigidbody bars;

    float startHeight;
    float startWeight;
    float currentWeight;

    void Start()
    {
        startHeight = plate.transform.localPosition.y;
        startWeight = initialWeight.mass;
    }

    void FixedUpdate()
    {
        currentWeight = 0f;

        for (int i = 0, count = interactables.Count; i < count; i++) 
        {
            Interactable interactable = interactables[i];
            
            if(interactable.rigidbody.useGravity)
                currentWeight += interactable.rigidbody.mass;
        }

        if (playerInside)
            currentWeight += 1f;

        float currentHeightOffset = (startWeight - currentWeight) * weightFactor;
        float currentHeight = startHeight + currentHeightOffset;
        
        Vector3 nextPos = plate.transform.localPosition;
        nextPos.y = Mathf.Lerp(nextPos.y, currentHeight, adjustSpeed * Time.deltaTime);
        plate.transform.localPosition = nextPos;

        bool doorBlocked = Mathf.Abs(nextPos.y - startHeight) > 0.01f;
        Transform barsTarget = doorBlocked ? barsDown : barsUp;

        Vector3 nextBarPos = bars.position;
        nextBarPos.y = Mathf.Lerp(bars.position.y, barsTarget.position.y, 5f * Time.deltaTime);
        bars.MovePosition(nextBarPos);
    }
}
