using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Class used to check if bubbles should be popped of not
/// </summary>
public class BubblePopper
{
    private readonly BubbleMatrix matrix;
    private readonly List<GameObject> bubblesToPop;
    private readonly List<GameObject> bubblesToAdd;
    private GameObject bubbleToCheck;
    private readonly List<int> bubblesToRemoveRow;
    private readonly List<int> bubblesToRemoveCol;
    private int bubbleRow;
    private int bubbleCol;

    public BubblePopper(BubbleMatrix matrix, GameObject bubble)
    {
        this.matrix = matrix;
        bubblesToRemoveRow = new List<int>();
        bubblesToRemoveCol = new List<int>();
        bubblesToPop = new List<GameObject>();
        bubblesToAdd = new List<GameObject>();
        bubblesToPop.Add(bubble);
        PositionInMatrix(bubble);
        CheckNeighbours();
        CheckAllBubblesInList();
    }

    /// <summary>
    /// Locate position of bubble in BubbleMatrix and add the row and column to check for neighbours.
    /// </summary>
    /// <param name="bubble">Bubble to locate</param>
    private void PositionInMatrix(GameObject bubble)
    {
        var (row, col) = matrix.PositionInMatrix(bubble);
        bubbleToCheck = matrix.GetBubble(row, col);
        bubbleRow = row;
        bubbleCol = col;
        bubblesToRemoveRow.Add(row);
        bubblesToRemoveCol.Add(col);
        
    }

    /// <summary>
    /// Check if neighbours in one position in each direction for a set odf specific rows and columns.
    /// </summary>
    private void CheckNeighbours()
    {
        var checker = 0;
        var overAndUnderRowCol = bubbleRow % 2 == 0 ? bubbleCol - 1 : bubbleCol;
        var thisRowCol = bubbleCol - 1;
        var row = bubbleRow;
        while (checker < 2)
        {
            FindSameColorBubbles(bubbleRow - 1, overAndUnderRowCol);
            FindSameColorBubbles(bubbleRow, thisRowCol);
            FindSameColorBubbles(bubbleRow + 1, overAndUnderRowCol++);
            thisRowCol += 2;
            checker++;
        }
    }

    /// <summary>
    /// Find bubbles with same color, if is same color bubbles are added to list of bubbles to pop.
    /// </summary>
    /// <param name="row">row in BubbleMatrix to check</param>
    /// <param name="col"> column in BubbleMatrix to check</param>
    private void FindSameColorBubbles(int row, int col)
    {
        if (row < 0) return;
        var count = matrix.GetNumberOfColumns();
        if (col >= count || col < 0) return;
        var nextBubbleToCheck = matrix.GetBubble(row, col);
        if (nextBubbleToCheck == null) return;
        if (NameSplit(bubbleToCheck.name).Equals(NameSplit(nextBubbleToCheck.name))
            && !bubblesToPop.Contains(nextBubbleToCheck))
        {
            bubblesToAdd.Add(nextBubbleToCheck);
        }
    }

    /// <summary>
    /// Check if neighbours for bubbles in bubblesToPop have same color as the specific color
    /// being check 
    /// </summary>
    private void CheckAllBubblesInList()
    {
        do
        {
            var startingPoint = bubblesToPop.Count;
            bubblesToPop.AddRange(bubblesToAdd);
            bubblesToAdd.Clear();
            for (var bubble = startingPoint; bubble < bubblesToPop.Count; bubble++)
            {
                PositionInMatrix(bubblesToPop[bubble]);
                CheckNeighbours();
            }
        } while (bubblesToAdd.Count != 0);
    }

    /// <summary>
    /// Split the name at "."
    /// Bubble color is returned.
    /// </summary>
    /// <param name="name">Name of bubble</param>
    /// <returns></returns>
    private string NameSplit(string name)
    {
        return name.Split('.')[0];
    }
    
    /// <summary>
    /// Return a list of bubbles to pop
    /// </summary>
    /// <returns>Bubbles to pop</returns>
    public List<GameObject> GetBubblesToPop()
    {
        return bubblesToPop;
    }
}