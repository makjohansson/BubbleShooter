using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement script for enemy in hard mode. Speed can be set in the Unity editor
/// </summary>
public class EnemyMovementHard : MonoBehaviour
{
    public float speed;

    public Transform moveSpot;

    private const float minX = -3.2f;
    private const float maxX = 3.2f;
   
    private void Start()
    {
        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0f, 0f);
    }
    
    // The enemy will move in a random manner inside the max x positions   
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (!(Vector3.Distance(transform.position, moveSpot.position) < 0.2f)) return;
        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0f, 0f);
    }
    
    
}
