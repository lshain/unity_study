using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    public Transform target = null;

    private Vector3 mDis = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        if (target != null)
        {
            mDis = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + mDis;
        } 
    }
}
