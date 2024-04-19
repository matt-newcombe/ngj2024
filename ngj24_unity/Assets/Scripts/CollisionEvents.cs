using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvents : MonoBehaviour
{
    public delegate void CollisionEvent(Collider collider);

    public CollisionEvent triggerEnter;
    public CollisionEvent triggerExit;

    private void OnTriggerEnter(Collider collider)
    {
        if (triggerEnter != null)
        {
            triggerEnter(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (triggerExit != null)
        {
            triggerExit(collider);
        }
    }
}
