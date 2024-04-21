using System.Collections;
using StarterAssets;
using UnityEngine;

public class PlayerKillReset : MonoBehaviour
{
    public float KillHeight = -100f;
    public FirstPersonController controller;
    private Vector3 _playerSpawnPos = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerSpawnPos = controller.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.transform.position.y < KillHeight)
        {
            StartCoroutine(ResetRoutine());
        }
    }

    IEnumerator ResetRoutine()
    {
        controller.GetComponent<CharacterController>().enabled = false;
        yield return null;
        controller.transform.position = _playerSpawnPos + Vector3.up * 30f;
        yield return null;
        controller.GetComponent<CharacterController>().enabled = true;
    }
}
