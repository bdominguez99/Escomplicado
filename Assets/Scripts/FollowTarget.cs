using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform objetivo;

    void Update()
    {
        transform.position = new Vector3(objetivo.transform.position.x, objetivo.transform.position.y, transform.position.z);
    }
}
