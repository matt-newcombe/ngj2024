using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour
{
    public CollisionEvents trigger;
    
    [Header("---- General ----")]
    public FirstPersonController playerInside;
    public List<Interactable> interactables = new List<Interactable>();
    public bool isActive;
    public bool activateOnCarry = true;
    public bool canBeDeactivated = false;
    
    public GameObject activeState;
    public GameObject deactiveState;

    [Header("---- Audio ----")]
    public AudioClip[] interactSounds;
    public AudioClip[] carryStartSounds;
    public AudioSource audioSource;
    
    [HideInInspector]
    public new Rigidbody rigidbody;

    void Awake()
    {
        if (trigger) 
        {
            trigger.triggerEnter += TriggerEnter;
            trigger.triggerExit += TriggerExit;
        }

        rigidbody = GetComponent<Rigidbody>();

        SetCubeActive(isActive);
    }

    public virtual void MouseDown()
    {
    }

    public virtual void SetCubeActive(bool value)
    {
        isActive = value;

        if (activeState)
            activeState.SetActive(isActive);

        if (deactiveState)
            deactiveState.SetActive(!isActive);
    }

    public virtual void PokeInteraction()
    {
        if (isActive)
        {
            if (canBeDeactivated)
                SetCubeActive(false);
        }
        else 
        {
            if(!isActive)
                SetCubeActive(true);
        }

        if(audioSource != null && interactSounds.Length > 0)
        {
            int index = Random.Range(0, interactSounds.Length);
            audioSource.clip = interactSounds[index];
            audioSource.Play();
        }
    }

    public void StartCarry()
    {
        if(activateOnCarry && !isActive)
            SetCubeActive(true);

        if (audioSource != null && carryStartSounds.Length > 0)
        {
            int index = Random.Range(0, carryStartSounds.Length);
            audioSource.clip = carryStartSounds[index];
            audioSource.Play();
        }
    }

    private void TriggerEnter(Collider collider)
    {
        FirstPersonController player = collider.GetComponentInParent<FirstPersonController>();
        if (player)
            playerInside = player;

        Interactable interactable = collider.GetComponent<Interactable>();
        if (interactable)
        {
            if (interactables.Contains(interactable) == false)
                interactables.Add(interactable);
        }
        else
        {
            Interactable interactableParent = collider.GetComponentInParent<Interactable>();
            if (interactableParent && interactables.Contains(interactableParent) == false)
                interactables.Add(interactableParent);
        }
    }

    private void TriggerExit(Collider collider)
    {
        FirstPersonController player = collider.GetComponentInParent<FirstPersonController>();
        if (player)
            playerInside = null;

        Interactable interactable = collider.GetComponent<Interactable>();
        if (interactable)
        {
            interactables.Remove(interactable);
        }
        else
        {
            Interactable interactableParent = collider.GetComponentInParent<Interactable>();
            if (interactableParent)
                interactables.Remove(interactableParent);
        }
    }
}
