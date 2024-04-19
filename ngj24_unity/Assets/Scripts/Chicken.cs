using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Cube
{
    public GameObject eggPrefab;
    public AudioSource audioSourceChickenGhost;
    public Transform eggParent;

    float timer;

    float eggDelay = 3f;
    float eggDelayDecrease = 0.2f;
    
    float chickenGhostTime;

    public override void SetCubeActive(bool value)
    {
        base.SetCubeActive(value);

        if (isActive)
        {
            timer = eggDelay + Random.Range(0f, 1f);
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        FirstPersonController player = FirstPersonController.instance;

        if (player._speed > 5f)
            timer -= 3f * Time.deltaTime;
        else if (player._speed > 1f)
            timer -= Time.deltaTime;

        if(timer < 0)
        {
            Vector3 behindPlayerPos = player.transform.position;
            behindPlayerPos += player.transform.forward * -1.5f + Vector3.up * 0.5f;

            Instantiate(eggPrefab, behindPlayerPos, Quaternion.identity, eggParent);

            audioSourceChickenGhost.transform.position = behindPlayerPos;
                
            if(audioSourceChickenGhost.clip == null || Time.time > chickenGhostTime + audioSourceChickenGhost.clip.length) 
            {
                int index = Random.Range(0, interactSounds.Length);
                audioSourceChickenGhost.clip = interactSounds[index];
                audioSourceChickenGhost.Play();
                chickenGhostTime = Time.time;
            }

            if (eggDelay > 0f)
            {
                eggDelay -= eggDelayDecrease;
                timer = eggDelay + Random.Range(0f, 1f);
            }
            else
            {
                timer = Random.Range(0.7f, 1.2f);
            }
        }
    }
}
