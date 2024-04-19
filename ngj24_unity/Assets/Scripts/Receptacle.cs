using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : Cube
{
    public CollisionEvents goldTrigger;
    public GameObject goldInPlace;
    public Material solidMaterial;
    public AudioClip solidClip;
    public AudioSource audioSourceBeat;

    bool goldInside;
    
    void Update()
    {
        if (!goldInside)
        {
            for (int i = 0; i < interactables.Count; i++)
            {
                Interactable interactable = interactables[i];
                Cube cube = interactable as Cube;
                if (cube && cube.gold)
                {
                    float distance = Vector3.Distance(cube.transform.position, goldTrigger.transform.position);
                    if (distance < 0.2f)
                    {
                        FirstPersonController player = FirstPersonController.instance;
                        player.currentlyCarrying = null;

                        cube.gameObject.SetActive(false);

                        goldInPlace.SetActive(true);
                        goldInside = true;

                        audioSourceBeat.Play();
                    }
                }
            }
        }
        else
        {
            for (int i = 0, count = interactables.Count; i < count; i++)
            {
                Interactable interactable = interactables[i];
                if (interactable == this)
                    continue;

                Cube cube = interactable as Cube;
                if (cube && !cube.solid && cube.gaseous)
                {
                    float distance = Vector3.Distance(cube.transform.position, transform.position);
                    if(distance < 2f) 
                    {
                        cube.solid = true;
                        cube.rigidbody.isKinematic = true;

                        audioSource.clip = solidClip;
                        audioSource.Play();

                        Renderer[] renderers = interactable.GetComponentsInChildren<Renderer>();
                        for (int s = 0, length = renderers.Length; s < length; s++)
                            renderers[s].material = solidMaterial;
                    }
                }
            }
        }
    }
}
