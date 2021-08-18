using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller class that insert a CanonBubble on correct position in the BubbleMatrix
/// based on the CanonBubbles x and y position.
/// </summary>
public class BubbleMatrixHandler
{
    private readonly BubbleMatrix matrix;
    private readonly Dictionary<float, int> rows = new Dictionary<float, int>();
    private readonly Dictionary<float, int> evenColumns = new Dictionary<float, int>();
    private readonly Dictionary<float, int> oddColumns = new Dictionary<float, int>();
    
    public BubbleMatrixHandler(BubbleMatrix matrix)
    {
        this.matrix = matrix;
        SetupRows();
        SetupEvenColumns();
        SetupOddColumns();

    }

    /// <summary>
    /// Insert bubble in the matrix at specific row and column
    /// based on the bubbles x and y position.
    /// </summary>
    /// <param name="bubble"></param>
    /// <param name="y"></param>
    /// <param name="x"></param>
    public void InsertToMatrix(GameObject bubble, float y, float x)
    {
        x = x > 2.5f & x < 3 ? x = 2.5f : x;
        x = x < -2.5f & x > -3 ? x = -3 : x;
        x = ConvertXAndY(x);
        y = ConvertXAndY(y);
        if (rows[y] % 2 == 0 && x >= -2) x -= 0.5f;
        if (rows[y] % 2 == 0 && x >= 0) x += 0.5f;
        x = x >= 0 ? x - 0.5f : x;
        x = x >= 3 ? x - 0.5f : x;
        matrix.Insert(bubble, rows[y], rows[y] % 2 == 0 ? evenColumns[x] : oddColumns[x]);
    }

    /// <summary>
    /// Make sure that the x and y values always are rounded to closes half or hole float.
    /// </summary>
    /// <param name="y"></param>
    /// <returns>float</returns>
    private float ConvertXAndY(float f)
    {
        f = f > 4.5 & f < 5 ? 5 : f;
        f = f > 5 & f < 5.5 ? 5.5f : f;
        return (float) Math.Round(f * 2, MidpointRounding.AwayFromZero) / 2;
    }
    
    /// <summary>
    /// Return the BubbleMatrix attached to the object
    /// </summary>
    /// <returns></returns>
    public BubbleMatrix GetBubbleMatrix()
    {
        return matrix;
    }

    /// <summary>
    /// Setup the rows for the matrix
    /// </summary>
    private void SetupRows()
    {
        var y = 6f;
        for (var row = 0; row < matrix.GetNumberOfRows(); row++)
        {
            rows[y] = row;
            y -=0.5f;
        }
    }

    /// <summary>
    /// Set up the columns for where the row number is even
    /// </summary>
    private void SetupEvenColumns()
    {
        var x = -3.5f;
        for (var column = 0; column < matrix.GetNumberOfColumns(); column++)
        {
            evenColumns[x] = column;
            x += 0.5f;
        }
    }

    /// <summary>
    /// Set up the columns for where the row number is odd
    /// </summary>
    private void SetupOddColumns()
    {
        var x = -3f;
        for (var column = 0; column < matrix.GetNumberOfColumns(); column++)
        {
            oddColumns[x] = column;
            x += 0.5f;
        }
    }
}