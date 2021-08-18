using UnityEngine;

/// <summary>
/// Destroy bubbles that falls and hit the BottomWall and update score to the bubbleController
/// </summary>
public class BottomCollider : MonoBehaviour
{
    private BubbleController bubbleController;
    private void OnCollisionEnter(Collision other)
    {
        bubbleController = FindObjectOfType<BubbleController>();
        bubbleController.UpdatePlayerScore(bubbleController.getScorePerBubble());
        bubbleController.PopBubble(other.transform.position);
        Destroy(other.gameObject);
    }
}
