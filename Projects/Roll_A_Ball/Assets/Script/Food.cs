using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float da = Time.deltaTime * 360;
        transform.Rotate(new Vector3(da,da,da));
    }
}
