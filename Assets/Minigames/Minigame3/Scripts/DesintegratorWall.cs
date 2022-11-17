using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesintegratorWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<CameraController>().StopFollowingRock();
        Destroy(collision.gameObject);
    }
}
