using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that play sound sfx if the player hits the friend/enemy
/// It update the times the enemy is hit by one to the GameController
/// </summary>
public class EnemyCollider : MonoBehaviour
{
    private const string TopTag = "TopBubble";
    public GameController controller;
    private const int enemyHit = 1;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == TopTag) return;
        FindObjectOfType<BubbleController>().PopBubble(other.transform.position);
        Destroy(other.gameObject);
        AudioManager.instance.PlayRandomEnemySound(SceneManager.GetActiveScene().buildIndex < 4);
        controller.CheckEnemyHits(enemyHit);
    }
}
