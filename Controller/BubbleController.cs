using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour class that instantiate all the bubbles in the bubble matrix and keep the matrix updated during
/// a game.
/// </summary>
public class BubbleController : MonoBehaviour
{
    public static int idCounter;
    private const float Y = 5.67f;
    public const float SubtractY = 0.47f;
    public const float AddX = 0.55f;
    private const float XEvenRow = -3.3f;
    public const float XOddRow = -3f;
    private const string topTag = "TopBubble";
    private FillMatrixWithBubbles filledMatrix;
    private BubbleMatrix matrix;
    private GameController gameController;
    private int cols;
    private float x;
    private float y = Y;
    private const int startScore = 10;
    public int scorePerBubble = startScore;
    private int power = 2; 
    
    public GameObject[] prefabBubbles;
    public int rows;
    public int columns;
    public ParticleSystem bubbleBurst;
    public GameObject powerUp;
    public string popSound;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        filledMatrix = new FillMatrixWithBubbles(prefabBubbles, rows, columns);
        matrix = filledMatrix.GetBubbleMatrix();
        cols = columns;
        InstantiateBubble();
    }
    
    /// <summary>
    /// Instantiate the bubbles in the matrix at right position(x,y) and make each bubble unique.
    /// </summary>
    private void InstantiateBubble()
    {
        idCounter = 0;
        for (var row = 0; row < rows; row++)
        {
            SetupRow(row);
            for (var col = 0; col < columns; col++)
            {
                var bubble = matrix.GetBubble(row, col);
                var obj = Instantiate(bubble, new Vector3(x, y, 0f), bubble.transform.rotation);
                obj.GetComponent<Rigidbody>().isKinematic = true;
                obj.tag = topTag;
                obj.AddComponent<BubbleCollider>();
                obj.name = bubble.name + "." + idCounter++;
                matrix.Insert(obj, row, col);
                x += AddX;
            }
            y -= SubtractY;
        }
    }

    /// <summary>
    /// Will instantiate the powerUp animation and ply the powerUp sfx.
    /// Score per bubble will be doubled  
    /// </summary>
    public void runPowerUP()
    {
        if (this.powerUp == null) return;
        var powerUp = Instantiate(this.powerUp, new Vector3(0, -1, -4), this.powerUp.transform.rotation);
        powerUp.GetComponent<TextMesh>().text = "x" + power;
        scorePerBubble = startScore * power;
        power += 2;
        AudioManager.instance.Play("PowerUp");
    }

    /// <summary>
    /// The x position from the start in every row differ if the row number is even or odd.
    /// If the row is even the x position is moved -0.3 in x
    /// </summary>
    /// <param name="i">row number</param>
    private void SetupRow(int i)
    {
        columns = cols;
        columns = i % 2 == 0 ? columns : columns - 1;
        x = i % 2 == 0 ? x = XEvenRow : x = XOddRow;
    }

    /// <summary>
    /// Return the BubbleMatrix
    /// </summary>
    /// <returns></returns>
    public BubbleMatrix GetMatrix()
    {
        return matrix;
    }

    public bool BubbleOnfinalRow()
    {
        return matrix.CheckFinalRow();
    }

    public int getScorePerBubble()
    {
        return scorePerBubble;
    }
    
    /// <summary>
    /// Update the BubbleMatrix
    /// </summary>
    /// <param name="matrix">Matrix to use for update</param>
    public void UpdateMatrix(BubbleMatrix matrix)
    {
        this.matrix = matrix;
    }

    /// <summary>
    /// Update the players score
    /// </summary>
    /// <param name="score"></param>
    public void UpdatePlayerScore(int score)
    {
        gameController.UpdatePlayerScore(score);
    }
    

    /// <summary>
    /// Pop effect on  bubble that will be destroyed
    /// </summary>
    /// <param name="position"></param>
    public void PopBubble(Vector3 position)
    {
        var burst = Instantiate(bubbleBurst, position, Quaternion.identity);
        AudioManager.instance.Play(popSound);
        Destroy(burst, 0.5f);
        
    }
}