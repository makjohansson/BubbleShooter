using System.Collections.Generic;
using UnityEngine;


public class BubbleMatrix
{
    private readonly GameObject[,] matrix;
    private const int finalRow = 17;
    
    public BubbleMatrix(int rows, int cols)
    {
        matrix = new GameObject[rows , cols];
    }

    /// <summary>
    /// Insert bubble at specific position.
    /// </summary>
    /// <param name="bubble"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Insert(GameObject bubble, int row, int col)
    {
        matrix[row, col] = bubble;
    }

    /// <summary>
    /// Remove bubble at specific position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void remove(int row, int col)
    {
        matrix[row, col] = null;
    }

    /// <summary>
    /// Return true if there is a bubble on specific position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool BubbleOnPosition(int row, int col)
    {
        return matrix[row, col] != null;
    }
    
    /// <summary>
    /// Return bubble on specific position in BubbleMatrix
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public GameObject GetBubble(int row, int col)
    {
        return matrix[row, col];
    }

    /// <summary>
    /// Total number of rows in BubbleMatrix
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfRows()
    {
        return matrix.GetLength(0);
    }
    
    /// <summary>
    /// Total number of columns in BubbleMatrix
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfColumns()
    {
        return matrix.GetLength(1);
    }

    /// <summary>
    /// Return row number and column number in BubbleMatrix for specific bubble
    /// </summary>
    /// <param name="bubble"></param>
    /// <returns></returns>
    public (int, int) PositionInMatrix(GameObject bubble)
    {
        var bubbleFound = false;
        var bubbleRow = 0;
        var bubbleCol = 0;
        for (var row = 0; row < GetNumberOfRows(); row++)
        {
            for (var col = 0; col < GetNumberOfColumns(); col++)
            {
                var checkBubble = GetBubble(row, col);
                if (checkBubble == null) continue;
                if (!checkBubble.name.Equals(bubble.name)) continue;
                bubbleRow = row;
                bubbleCol = col;
                bubbleFound = true;
                break;
            }
            if (bubbleFound)
                break;
        }

        return (bubbleRow, bubbleCol);
    }

    /// <summary>
    /// Return true if there is a bubble on the row set to the finalRow
    /// </summary>
    /// <returns></returns>
    public bool CheckFinalRow()
    {
        GameObject checkBubble = null;
        for (var col = 0; col < GetNumberOfColumns(); col++)
        {
            checkBubble = GetBubble(finalRow, col);
            if (checkBubble != null) break; 
        }

        return checkBubble != null;

    } 
}