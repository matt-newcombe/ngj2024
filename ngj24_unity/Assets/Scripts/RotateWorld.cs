using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : Cube
{
    public Transform[] targets;
    public float[] angles;

    public Transform world;
    public Door door;

    float timer;

    private void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.up * -9.81f, ForceMode.Force);
    }
    void Update()
    {
        FirstPersonController player = FirstPersonController.instance;
        if (player.currentlyCarrying == this.rigidbody) 
        {
            door.isOpen = false;
            return;
        }

        timer += Time.deltaTime;
        
        if (timer > 0.5f)
        {
            float cuboidAngle = Vector3.SignedAngle(transform.up, Vector3.up, transform.forward);
            //Debug.Log("cuboid angle " + cuboidAngle);

            float smallestDiff = 360f;
            int index = 0;

            for (int i = 0, length = angles.Length; i < length; i++)
            {
                float angle = angles[i];

                float angleDiff = Mathf.Abs(Mathf.DeltaAngle(angle, cuboidAngle));
                //Debug.Log("target " + target.gameObject.name + " , " + angle + " diff " + angleDiff);
                
                if (angleDiff < smallestDiff)
                {
                    smallestDiff = angleDiff;
                    index = i;
                }
            }

            Transform worldTarget = targets[index];
            //Debug.Log(worldTarget.gameObject.name);

            if (worldTarget != null)
            {
                world.SetPositionAndRotation(worldTarget.position, worldTarget.rotation);
                Physics.gravity = worldTarget.up * -9.81f;
            }

            timer = 0f;
        }
    }
}
