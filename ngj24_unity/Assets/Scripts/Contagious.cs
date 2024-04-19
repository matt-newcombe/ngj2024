using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contagious : Cube
{
    public Material material;
    public AudioClip contagiousClip;

    private List<Interactable> infected = new List<Interactable>();
    private float timer;

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive) 
        {
            Renderer renderer = collision.collider.gameObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                audioSource.clip = contagiousClip;
                audioSource.Play();

                renderer.material = material;
            }
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        timer += Time.deltaTime;

        if(timer > 2f)
        {
            //Spread
            for (int t = 0, infectedCount = infected.Count; t < infectedCount; t++)
            {
                Interactable infectedInteractable = infected[t];
               
                if (infectedInteractable is Cube) 
                {
                    for (int i = 0, count = infectedInteractable.interactables.Count; i < count; i++)
                    {
                        Interactable interactable = infectedInteractable.interactables[i];

                        float distance = Vector3.Distance(infectedInteractable.transform.position, interactable.transform.position);
                        if (distance < 2f)
                            continue;

                        if (infected.Contains(interactable) == false)
                        {
                            audioSource.clip = contagiousClip;
                            audioSource.Play();

                            Renderer[] renderers = interactable.GetComponentsInChildren<Renderer>();
                            for (int s = 0, length = renderers.Length; s < length; s++)
                                renderers[s].material = material;
                        }
                    }
                }
            }

            for (int i = 0, count = interactables.Count; i < count; i++)
            {
                Interactable interactable = interactables[i];
                if (interactable is Cube)
                {
                    float distance = Vector3.Distance(interactable.transform.position, transform.position);
                    if (distance < 2f)
                        continue;

                    if (infected.Contains(interactable) == false)
                    {
                        audioSource.clip = contagiousClip;
                        audioSource.Play();

                        Renderer[] renderers = interactable.GetComponentsInChildren<Renderer>();
                        for (int s = 0, length = renderers.Length; s < length; s++)
                            renderers[s].material = material;
                    }
                }
            }

            timer = 0f;
        }
    }
}
