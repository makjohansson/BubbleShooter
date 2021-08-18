using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// MonoBehaviour class that position a Bubble shoot from the canon at correct position
/// using OnCollisionEnter to detect TopBubble the CanonBubble is colliding with.
/// </summary>
public class BubbleCollider : MonoBehaviour
{
    private const float leftBorder = -3.4f;
    private const float rightBorder = 3.4f;
    private const float addX = BubbleController.AddX;
    private const float subtractY = BubbleController.SubtractY;
    private const string TopTag = "TopBubble";
    private bool dropBubbles;
    private int score = 0;
    private BubbleController bubbleController;
    private Rigidbody otherRb;
    private GameObject CanonBubble;
    private BubbleMatrix matrix;
    private BubblePositionController bubblePositionController;
    private List<GameObject> bubblesToPop;
    private List<GameObject> bubblesToDrop;
    private int[,] bubblesToKeepMatrix;
    private bool addOne;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == TopTag) return;
        otherRb = other.gameObject.GetComponent<Rigidbody>();
        otherRb.isKinematic = true;
        bubbleController = FindObjectOfType<BubbleController>();
        CanonBubble = other.gameObject;
        CanonBubble.tag = TopTag;
        CanonBubble.AddComponent<BubbleCollider>();
        CanonBubble.name = CanonBubble.name + "." + BubbleController.idCounter++;
        bubblePositionController = new BubblePositionController(CanonBubble, transform.position);
        matrix = bubbleController.GetMatrix();
        bubblePositionController.PopBubbles();
        bubblesToPop = bubblePositionController.GetBubblesToPop();
        DestroyBubbles();
    }

    /// <summary>
    /// Destroys bubbles if the count in bubbles to pop is <= 2.
    /// Also increment the score by 10 foreach bubble that is destroyed and use ClusterCheck and DropBubbles
    /// to control what bubble should be dropped and also destroyed. 
    /// </summary>
    private void DestroyBubbles()
    {
        if (bubblesToPop.Count <= 2) return;
        if (bubblesToPop.Count > 5) bubbleController.runPowerUP();
        bubblesToDrop =  new List<GameObject>();
        foreach (var bubble in bubblesToPop)
        {
            Destroy(bubble);
            matrix.remove(matrix.PositionInMatrix(bubble).Item1, matrix.PositionInMatrix(bubble).Item2);
            score +=  bubbleController.getScorePerBubble();
            bubbleController.PopBubble(bubble.transform.position);
        }
        ClusterCheck();
        DropBubbles();
        bubbleController.UpdatePlayerScore(score);
    }

    /// <summary>
    /// Make a bubble that was not connected to the top wall to drop and be destroyed.
    /// The players score is incremented with 10 per bubble dropped.
    /// </summary>
    private void DropBubbles()
    {
        
        foreach (var bubble in bubblesToDrop)
        {
            bubble.GetComponent<Rigidbody>().isKinematic = false;
            bubble.GetComponent<Rigidbody>().useGravity = true;
            Destroy(bubble.GetComponent<BubbleCollider>());
            var (row, col) = matrix.PositionInMatrix(bubble);
            matrix.remove(row, col);
            score += bubbleController.getScorePerBubble();
        }
    }
    
    
    /// <summary>
    /// Make a check to the left, top left and top of a specific bubble to check if it is connected to
    /// top wall or not. If not connected the bool addOne will be set to true and the bubble will be added to
    /// the bubblesToKeep matrix
    /// </summary>
    /// <param name="keep">Mirror matrix of the bubble matrix</param>
    /// <param name="row">bubble row</param>
    /// <param name="col">bubble column</param>
    private void CheckLeft(int[,] keep, int row, int col)
    {
        if (col == 0)
        {
            if (keep[row - 1, col] == 1) addOne = true;
            if (keep[row, col + 1] == 1) addOne = true;
            return;
        }
        if (row % 2 != 0)
        {
            if (keep[row - 1, col] == 1)
                addOne = true;
            if (keep[row - 1, col + 1] == 1)
                addOne = true;
            if (keep[row, col - 1] == 1)
                addOne = true;
            return;
        }
        switch (col)
        {
            case 12:
                if (keep[row - 1, col - 1] == 1)
                    addOne = true;
                break;
            default:
                if (keep[row - 1, col] == 1)
                    addOne = true;
                if (keep[row - 1, col - 1] == 1)
                    addOne = true;
                if (keep[row, col - 1] == 1)
                    addOne = true;
                break;
        }
    }

    /// <summary>
    /// Make a check to the right, top right and top of a specific bubble to check if it is connected to
    /// top wall or not. If not connected the bool addOne will be set to true and the bubble will be added to
    /// the bubblesToKeep matrix
    /// </summary>
    /// <param name="keep">Mirror matrix of the bubble matrix</param>
    /// <param name="row">bubble row</param>
    /// <param name="col">bubble column</param>
    private void CheckRight(int[,] keep, int row, int col)
    {
        if (col == 0)
        {
            if (keep[row - 1, col] == 1) addOne = true;
            if (row % 2 == 0) return;
            if (keep[row - 1, col + 1] == 1) addOne = true;
            return;
        }
        if (row % 2 != 0)
        {
            if (keep[row - 1, col] == 1)
                addOne = true;
            if (keep[row - 1, col + 1] == 1)
                addOne = true;
            if (keep[row, col + 1] == 1)
                addOne = true;
            return;
        }
        switch (col)
        { 
            case 12:
                if (keep[row - 1, col - 1] == 1)
                    addOne = true;
                break;
            default:
                if (keep[row - 1, col] == 1)
                    addOne = true;
                if (keep[row - 1, col - 1] == 1)
                    addOne = true;
                if (keep[row, col + 1] == 1)
                    addOne = true;
                break;
        }
    }

    // Clean this when everything is done and also do an extra check over the logic
    /// <summary>
    /// Create a mirror matrix of bubble matrix and set (row, col) to 1 if a specific bubble has a bubble on the
    /// left, top left, top right, right. If not the (row,col) = 0. Check first from the left and then from the right for each row.
    /// to cover each possible cases it also make the check from the bottom and up.
    /// If there is a bubble on a position in the bubble matrix but the position in bubblesToKeep is = 0 then the bubble will be dropped.
    /// </summary>
    private void ClusterCheck()
    {
        var numberOfRows = matrix.GetNumberOfRows();
        var numberOfCols = matrix.GetNumberOfColumns();
        bubblesToKeepMatrix = new int[numberOfRows, numberOfCols];
        
        // Check from top row, from the left and from the right foreach column
        for (var row = 0; row < numberOfRows; row++)
        {
            for (var col = 0; col < numberOfCols; col++)
            {
                if (!matrix.BubbleOnPosition(row, col)) continue;
                if (row == 0)
                {
                    bubblesToKeepMatrix[row, col] = 1;
                }
                else
                {
                    CheckLeft(bubblesToKeepMatrix, row, col);
                    if (!addOne) continue;
                    bubblesToKeepMatrix[row, col] = 1;
                    addOne = false;
                }
            }
            
            for (var col = numberOfCols - 1; col >= 0; col--)
            {
                if (!matrix.BubbleOnPosition(row, col)) continue;
                if (row == 0)
                {
                    bubblesToKeepMatrix[row, col] = 1;
                }
                else
                {
                    CheckRight(bubblesToKeepMatrix, row, col);
                    if (!addOne) continue;
                    bubblesToKeepMatrix[row, col] = 1;
                    addOne = false;
                }
            }
        }
        
        // Check from the bottom, from the left and from the right for each column
        for (var row = numberOfRows - 1; row >= 0; row--)
        {
            for (var col = 0; col < numberOfCols; col++)
            {
                if (!matrix.BubbleOnPosition(row, col)) continue;
                if (row == 0)
                {
                    bubblesToKeepMatrix[row, col] = 1;
                }
                else
                {
                    CheckLeft(bubblesToKeepMatrix, row, col);
                    if (!addOne) continue;
                    bubblesToKeepMatrix[row, col] = 1;
                    addOne = false;

                }
            }
            
            for (var col = numberOfCols - 1; col >= 0; col--)
            {
                if (!matrix.BubbleOnPosition(row, col)) continue;
                if (row == 0)
                {
                    bubblesToKeepMatrix[row, col] = 1;
                }
                else
                {
                    CheckRight(bubblesToKeepMatrix, row, col);
                    if (!addOne) continue;
                    bubblesToKeepMatrix[row, col] = 1;
                    addOne = false;
                }
            }
        }
        
        for (var row = 0; row < matrix.GetNumberOfRows(); row++)
        {
            for (var col = 0; col < matrix.GetNumberOfColumns(); col++)
            {
                if (bubblesToKeepMatrix[row, col] != 1 & matrix.BubbleOnPosition(row, col))
                    bubblesToDrop.Add(matrix.GetBubble(row, col));
            }
        }
    }
}