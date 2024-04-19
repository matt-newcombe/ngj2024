using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freemdom : MonoBehaviour
{
    public GameObject freedomText;
    public CollisionEvents[] freedomTriggers;

    void Start()
    {
        for (int i = 0; i < freedomTriggers.Length; i++)
        {
            freedomTriggers[i].triggerEnter += Freedom;
        }

    }

    private void Freedom(Collider collider)
    {
        freedomText.SetActive(true);
    }
}
