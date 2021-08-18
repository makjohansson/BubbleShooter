using System.Collections.Generic;
using UnityEngine;

public class BubblePositionController
{
    private const float leftBorder = -3.4f;
    private const float rightBorder = 3.4f;
    private const float addX = BubbleController.AddX;
    private const float subtractY = BubbleController.SubtractY;
    private const float RadiusToCheck = 0.3f;
    private int CheckCounter;
    private const bool toTheRight = true;
    private readonly GameObject CanonBubble;
    private readonly Vector3 centerPosition;
    private readonly BubbleController controller;
    private BubbleMatrix matrix;
    private readonly BubbleMatrixHandler matrixHandler;
    private BubblePopper bubblePopper;
    private List<GameObject> bubblesToPop;
    
    
    public BubblePositionController(GameObject CanonBubble, Vector3 centerPosition)
    {
        this.CanonBubble = CanonBubble;
        this.centerPosition = centerPosition;
        controller = GameObject.Find("BubbleController").GetComponent<BubbleController>();
        matrix = controller.GetMatrix();
        matrixHandler = new BubbleMatrixHandler(matrix);
        PositionBubble();
    }

    /// <summary>
    /// Position the bubble in the BubbleMatrix
    /// </summary>
    private void PositionBubble()
    {
        var pos = CheckPosition();
        switch (pos)
        {
            case 1:
                CanonBubble.transform.position = DownRight();
                UpdateMatrix();
                break;
            case 2:
                CanonBubble.transform.position = DownLeft();
                UpdateMatrix();
                break;
            case 3:
                CanonBubble.transform.position = CheckAllPossiblePositions(toTheRight);
                UpdateMatrix();
                break;
            case 4:
                CanonBubble.transform.position = CheckAllPossiblePositions(!toTheRight);
                UpdateMatrix();
                break;
            default:
                Debug.Log("Return was zero");
                break;
        }
    }
    
    /// <summary>
    /// Check all possible position on the right of this bubble if bool param is true
    /// if false it checks the left side of this bubble. 
    /// </summary>
    /// <param name="sideToCheck"></param>
    /// <returns>Vector 3</returns>
    private Vector3 CheckAllPossiblePositions(bool sideToCheck)
    {
        CheckCounter = 0;
        var whereToCheck = sideToCheck ? 2 : 0;
        var wasOnPosition = true;
        while (wasOnPosition && CheckCounter < 3)
        {
            wasOnPosition = IsBubbleOnPosition(whereToCheck++);
            CheckCounter++;
        }


        return sideToCheck ? PositionOnTheRight(--CheckCounter) : PositionOnTheLeft(--CheckCounter);
    }
    

    /// <summary>
    /// Positions the CanonBubble to the left of this bubble
    /// Position is depending on the param
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Vector 3</returns>
    private Vector3 PositionOnTheLeft(int pos)
    {
        switch (pos)
        {
            case 0:
                return Left();
            case 1:
                return TopLeft();
        }

        return DownLeft();
    }

    /// <summary>
    /// Positions the CanonBubble to the right of this bubble
    /// Position is depending on the param
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Vector 3</returns>
    private Vector3 PositionOnTheRight(int pos)
    {
        switch (pos)
        {
            case 0:
                return Right();
            case 1:
                return TopRight();
        }

        return DownRight();
    }

    /// <summary>
    /// Check the position where the CanonBubble hit this bubble.
    /// Return 1 if position is lower right side.
    /// Return 2 if position is lower left side.
    /// Return 3 if position is top right side.
    /// Return 4 if position is top left side. 
    /// </summary>
    /// <returns></returns>
    private int CheckPosition()
    {
        if (CanonBubble.transform.position.x > centerPosition.x &&
            CanonBubble.transform.position.y < centerPosition.y)
            return 1;
        if (CanonBubble.transform.position.x < centerPosition.x &&
            CanonBubble.transform.position.y < centerPosition.y)
            return 2;
        if (CanonBubble.transform.position.x > centerPosition.x &&
            CanonBubble.transform.position.y > centerPosition.y)
            return 3;
        if (CanonBubble.transform.position.x < centerPosition.x &&
            CanonBubble.transform.position.y > centerPosition.y)
            return 4;

        return 0;
    }

    /// <summary>
    /// Position the CanonBubble on the lower right side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 DownRight()
    {
        return new Vector3(centerPosition.x + (addX / 2),
            centerPosition.y - subtractY);
    }

    /// <summary>
    /// Position the CanonBubble on the lower left side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 DownLeft()
    {
        return new Vector3(centerPosition.x - (addX / 2),
            centerPosition.y - subtractY);
    }

    /// <summary>
    /// Position the CanonBubble on the right side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 Right()
    {
        return new Vector3(centerPosition.x + addX,
            centerPosition.y);
    }

    /// <summary>
    /// Position the CanonBubble on the left side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 Left()
    {
        return new Vector3(centerPosition.x - addX,
            centerPosition.y);
    }

    /// <summary>
    /// Position the CanonBubble on the top right side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 TopRight()
    {
        return new Vector3(centerPosition.x + (addX / 2),
            centerPosition.y + BubbleController.SubtractY);
    }

    /// <summary>
    /// Position the CanonBubble on the top left side of this bubble
    /// </summary>
    /// <returns>Vector 3</returns>
    private Vector3 TopLeft()
    {
        return new Vector3(centerPosition.x - (addX / 2),
            centerPosition.y + subtractY);
    }

    /// <summary>
    /// Check if there is another bubble on specific position
    /// return true is there is a bubble on position else false.
    /// </summary>
    /// <param name="posToCheck"></param>
    /// <returns>bool</returns>
    private bool IsBubbleOnPosition(int posToCheck)
    {
        var posToCheckLeft =
            new Vector3(centerPosition.x - addX,
                centerPosition.y);
        var posToCheckRight =
            new Vector3(centerPosition.x + addX,
                centerPosition.y);
        var posToCheckTopLeft =
            new Vector3(centerPosition.x - (addX / 2),
                centerPosition.y + subtractY);
        var posToCheckTopRight =
            new Vector3(centerPosition.x + (addX / 2),
                centerPosition.y + subtractY);
        switch (posToCheck)
        {
            case 0:
                return CheckSphere(posToCheckLeft, RadiusToCheck);
            case 1:
                return CheckSphere(posToCheckTopLeft, RadiusToCheck);
            case 2:
                return CheckSphere(posToCheckRight, RadiusToCheck);
            case 3:
                return CheckSphere(posToCheckTopRight, RadiusToCheck);
            default:
                return false;
        }
    }

    /// <summary>
    /// Check specific sphere if there is a bubble in it.
    /// Returns true if there is a bubble in the sphere else false
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <param name="radius">radius of sphere to check</param>
    /// <returns>bool</returns>
    private bool CheckSphere(Vector3 position, float radius)
    {
        var hitCollider = Physics.OverlapSphere(position, radius);
        if (hitCollider.Length == 0) return false;
        foreach (var bubble in hitCollider)
        {
            if (bubble.gameObject.Equals(CanonBubble) && hitCollider.Length == 1)
                return false;
        }

        return true;
    }


    /// <summary>
    /// Update the bubble matrix with CanonBubble at specific row and column.
    /// </summary>
    private void UpdateMatrix()
    {
        BorderCheck();
        matrixHandler.InsertToMatrix(CanonBubble, CanonBubble.transform.position.y,
            CanonBubble.transform.position.x);
        matrix = matrixHandler.GetBubbleMatrix();
        controller.UpdateMatrix(matrix);
    }

    /// <summary>
    /// Palace Canon bubble in correct position if the CanonBubble is outside of the game areas border.
    /// </summary>
    private void BorderCheck()
    {
        if (CanonBubble.transform.position.x < leftBorder)
            CanonBubble.transform.position = new Vector3(BubbleController.XOddRow,
                CanonBubble.transform.position.y);
        if (CanonBubble.transform.position.x > rightBorder)
            CanonBubble.transform.position = new Vector3(System.Math.Abs(BubbleController.XOddRow),
                CanonBubble.transform.position.y);
    }
    
    /// <summary>
    /// Use the class BubblePopper to get a list of what bubbles should be popped.
    /// </summary>
    public void PopBubbles()
    {
        matrix = controller.GetMatrix();
        bubblePopper = new BubblePopper(matrix, CanonBubble);
        bubblesToPop = bubblePopper.GetBubblesToPop();
    }

    /// <summary>
    /// Return the list of bubbles to be popped
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetBubblesToPop()
    {
        return bubblesToPop;
    }
}