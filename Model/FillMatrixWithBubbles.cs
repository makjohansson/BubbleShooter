using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Fill Bubble with bubbles
/// </summary>
public class FillMatrixWithBubbles
{
    private readonly GameObject[] prefabBubbles;
    private readonly int rows;
    private const int totalRows = 20;
    private int columns;
    private readonly int cols;
    private readonly BubbleMatrix matrix;

    /// <summary>
    /// Take an array of prefab bubbles and fill the BubbleMatrix using them using the method FillMatrix().
    /// </summary>
    /// <param name="prefabBubbles"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    public FillMatrixWithBubbles(GameObject[] prefabBubbles, int rows, int columns)
    {
        this.prefabBubbles = prefabBubbles;
        this.rows = rows;
        this.columns = columns;
        matrix = new BubbleMatrix(totalRows, columns);
        cols = columns;
        FillMatrix();
    }

    /// <summary>
    /// Fill BubbleMatrix
    /// </summary>
    private void FillMatrix()
    {
        for (var i = 0; i < rows; i++)
        {
            SetupRow(i);
            for (var j = 0; j < columns; j++)
            {
                var bubble = prefabBubbles[RandomBubble()];
                matrix.Insert(bubble, i, j);
            }
        }
    }

    /// <summary>
    /// Fill different depending on even or odd row
    /// </summary>
    /// <param name="i"></param>
    private void SetupRow(int i)
    {
        columns = cols; 
        columns = i % 2 == 0 ? columns : columns - 1;
    }

    /// <summary>
    /// Return a random int used to pick prefab bubble
    /// </summary>
    /// <returns></returns>
    private int RandomBubble()
    {
        return Random.Range(0, prefabBubbles.Length);
    }
    
    /// <summary>
    /// Return BubbleMatrix
    /// </summary>
    /// <returns></returns>
    public BubbleMatrix GetBubbleMatrix()
    {
        return matrix;
    }
}