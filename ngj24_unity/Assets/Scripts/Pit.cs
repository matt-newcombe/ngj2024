using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pit : MonoBehaviour
{
    public CollisionEvents trigger;

    public Camera pitCamera;
    
    public GameObject pitLight;
    public GameObject fakePlayer;
    public GameObject darkness;
    public GameObject directionalLight;

    public Color pitFogColor;
    public Color pitAmbientColor;
    
    private float timer;

    void Awake()
    {
        trigger.triggerEnter += TriggerEnter;
    }

    private void TriggerEnter(Collider collider)
    {
        if (collider.tag != "Player")
            return;

        FirstPersonController.instance._mainCamera.SetActive(false);
        FirstPersonController.instance.gameObject.SetActive(false);
        darkness.SetActive(false);
        
        pitCamera.gameObject.SetActive(true);
        fakePlayer.SetActive(true);
        pitLight.gameObject.SetActive(true);

        RenderSettings.ambientLight = pitAmbientColor;
        RenderSettings.fogColor = pitFogColor;  
    }

    private void Update()
    {
        if (fakePlayer.gameObject.activeSelf)
        {
            float previousTimer = timer;
            timer += Time.deltaTime;

            if (timer >= 7.5f && previousTimer < 7.5f)
            {
                directionalLight.SetActive(false);
                pitLight.gameObject.SetActive(false);
            }

            if(timer > 11f)
            {
                string sceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
