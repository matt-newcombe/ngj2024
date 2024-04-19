using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Transform door;
    public Transform doorClosed;
    public Transform doorOpen;
    public bool startOpen = false;

    public bool isOpen;

    private void Start()
    {
        isOpen = startOpen;
    }

    public override void MouseDown()
    {
        isOpen = !isOpen;
    }

    private void Update()
    {
        Transform target = isOpen ? doorOpen : doorClosed;

        Vector3 pos = target.position;
        Quaternion rot = target.rotation;
     
        door.position = Vector3.MoveTowards(door.position, pos, 3f * Time.deltaTime);
        door.rotation = Quaternion.RotateTowards(door.rotation, rot, 35f * Time.deltaTime);
    }
}
