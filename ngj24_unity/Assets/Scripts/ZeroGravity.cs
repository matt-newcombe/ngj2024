using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : Cube
{
    public AudioClip gasClip;

    void Update()
    {
        for (int i = 0, count = interactables.Count; i < count; i++)
        {
            Interactable interactable = interactables[i];

            float distance = Vector3.Distance(interactable.transform.position, transform.position);
            if (distance > 2f)
                continue;

            Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbody.isKinematic)
                continue;
            
            if (rigidbody != null) 
                rigidbody.useGravity = false;

            Cube cube = interactable as Cube;
            if (cube && !cube.gaseous) 
            {
                audioSource.clip = gasClip;
                audioSource.Play();

                cube.gaseous = true;
            }
        }
    }
}
