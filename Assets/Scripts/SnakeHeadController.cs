using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour, SnakeHead
{

    // New SnakeHeadController script

    private GameLogic gameLogic;        // game logic script
    private GameObject game;            // the game

    private GameObject currBody;    // current snake body

    private string direction;       // direction of snakeHead

    private float nextX;            // snakeHead next x position
    private float nextY;            // snakeHead next y position
    private float currX;            // snakeHead current x position
    private float currY;            // snakeHead current y position

    private int oldIdxX;            // snakeHead old board index X
    private int oldIdxY;            // snakeHead old board index Y
    private int newIdxX;            // snakeHead new board index X
    private int newIdxY;            // snakeHead new board index Y

    private float moveToX;          // x position to move snakeBody
    private float moveToY;          // y position to move snakeBody

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting gameLogic... (SnakeHeadController)");
        game = GameObject.Find("NewGame");
        gameLogic = game.GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing right now
    }

    public void prepareMove()
    {
        switch (direction)
        {
            case "left":
                // Grab next snakeHead pos
                nextX = (float)System.Math.Round(this.gameObject.transform.localPosition.x - gameLogic.getToMove(), 1);
                nextY = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);
                break;

            case "right":
                // Grab next snakeHead pos
                nextX = (float)System.Math.Round(this.gameObject.transform.localPosition.x + gameLogic.getToMove(), 1);
                nextY = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);
                break;

            case "up":
                // Grab next snakeHead pos
                nextX = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
                nextY = (float)System.Math.Round(this.gameObject.transform.localPosition.y + gameLogic.getToMove(), 1);
                break;

            case "down":
                // Grab next snakeHead pos
                nextX = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
                nextY = (float)System.Math.Round(this.gameObject.transform.localPosition.y - gameLogic.getToMove(), 1);
                break;
        }

        checkMove();
    }

    public void checkMove()
    {
        if (gameLogic.checkValidBounds(nextX, nextY) && gameLogic.checkValidMove(nextX, nextY))
        {
            updateBoard();
            moveSnake();
        }
    }

    public void moveSnake()
    {
        currX = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        currY = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);
        this.gameObject.transform.localPosition = new Vector3(nextX, nextY, 0);

        moveBody(currX, currY, 1);
    }

    public void moveBody(float x, float y, int idx)
    {
        string name = "SnakeBody" + idx.ToString();
        currBody = GameObject.Find(name);

        if (currBody != null)
        {
            moveToX = (float)System.Math.Round(currBody.transform.localPosition.x, 1);
            moveToY = (float)System.Math.Round(currBody.transform.localPosition.y, 1);

            oldIdxX = (int)System.Math.Abs(System.Math.Round((moveToX + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
            oldIdxY = (int)System.Math.Abs(System.Math.Round((moveToY + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
            gameLogic.removeBoard(oldIdxX, oldIdxY);

            newIdxX = (int)System.Math.Abs(System.Math.Round((x + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
            newIdxY = (int)System.Math.Abs(System.Math.Round((y + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
            gameLogic.setBoard(currBody, newIdxX, newIdxY);

            currBody.transform.localPosition = new Vector3(x, y, 1);
            moveBody(moveToX, moveToY, idx + 1);
        }
    }

    public void updateBoard()
    {
        // Remove head in old gameBoard pos
        oldIdxX = (int)System.Math.Abs(System.Math.Round((this.gameObject.transform.localPosition.x + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
        oldIdxY = (int)System.Math.Abs(System.Math.Round((this.gameObject.transform.localPosition.y + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
        gameLogic.removeBoard(oldIdxX, oldIdxY);
        //Debug.Log("Removing: " + oldIdxY + "," + oldIdxX);

        // Add head in new gameBoard pos
        newIdxX = (int)System.Math.Abs(System.Math.Round((nextX + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
        newIdxY = (int)System.Math.Abs(System.Math.Round((nextY + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
        gameLogic.setBoard(this.gameObject, newIdxX, newIdxY);
        //Debug.Log("Setting: " + newIdxY + "," + newIdxX);
    }

    public bool checkInvalidMovement(string movement)
    {
        switch (movement)
        {
            case "left":
                if (direction == "right")
                    return true;
                else
                    return false;
            case "right":
                if (direction == "left")
                    return true;
                else
                    return false;
            case "up":
                if (direction == "down")
                    return true;
                else
                    return false;
            case "down":
                if (direction == "up")
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    // Setter methods
    public void setDirection(string dir)
    {
        Debug.Log("Setting " + dir + "...");
        direction = dir;
    }

    // Getter methods
    public string getDirection()
    {
        return direction;
    }

    public float getSnakeHeadX()
    {
        return this.gameObject.transform.localPosition.x;
    }

    public float getSnakeHeadY()
    {
        return this.gameObject.transform.localPosition.y;
    }
}
