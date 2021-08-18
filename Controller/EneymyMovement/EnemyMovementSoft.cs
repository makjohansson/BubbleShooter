using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement script for enemy in hard mode. Speed can be set in the Unity editor
/// </summary>
public class EnemyMovementSoft : MonoBehaviour
{
    public float speed;
    
    public Transform moveSpot;

    private const float minX = -3f;
    private const float maxX = 3f;
    

    private void Start()
    {
        moveSpot.position = new Vector3(minX, 0f, 0f);
    }

    // The friend will move back and fort between the min and max X point set
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (!(Vector3.Distance(transform.position, moveSpot.position) < 0.2f)) return;

        moveSpot.position = transform.position.x < 0 ? new Vector3(maxX, 0f, 0f) : new Vector3(minX, 0f, 0f);
    }
    
}