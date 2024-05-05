using StarterAssets;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject winText;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("what "+ other, other.gameObject);
        //Debug.Log("what " + other.attachedRigidbody);
        //winText.SetActive(true);

        //if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out FirstPersonController player))
        if (other.TryGetComponent(out FirstPersonController player))
        {
            winText.SetActive(true);
        }
    }
}
