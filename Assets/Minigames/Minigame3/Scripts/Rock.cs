using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float m_velocityThreshold;
    [SerializeField] private float m_detroyDelay;

    public bool IsThrown { get; set; } = false;

    public int RelationId { get; private set; }

    public void Init(int relationId)
    {
        RelationId = relationId;
    }

    public void UpdateRock(int relationId, Sprite sprite)
    {
        RelationId = relationId;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Update()
    {
        if (IsThrown && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < m_velocityThreshold)
        {
            FindObjectOfType<CameraController>().StopFollowingRock();
            Destroy(gameObject, m_detroyDelay);
        }
    }
}
