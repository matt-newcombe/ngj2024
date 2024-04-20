using UnityEngine;

public class Stars : MonoBehaviour
{
    public GameObject target;
    public int count;
    
    [Space]
    public float distanceMin;
    public float distanceMax;

    [Space]
    public float sizeMin;
    public float sizeMax;

    [Space]
    bool randomRotation;
    bool faceCenter;

    void Awake()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 direction = Random.onUnitSphere;
            Vector3 pos = center + direction * Random.Range(distanceMin, distanceMax);

            Quaternion rot = Quaternion.identity;

            if (randomRotation)
                rot = Random.rotation;
            else if (faceCenter)
                rot = Quaternion.LookRotation(pos - center);

            GameObject newObject = Instantiate(target, pos, rot, transform);
            newObject.transform.localScale = Vector3.one * Random.Range(sizeMin, sizeMax);
        }

        target.SetActive(false);
    }
}
