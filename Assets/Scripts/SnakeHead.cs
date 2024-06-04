using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead
{
    private int[,] gameBoard;       // tracks snake head and body positions

    private GameObject gameObj;     // GameObject that this object references
    private GameLogic gameLogic;    // reference to GameLogic script
    private SnakeBody nextObj;      // reference to next SnakeBody

    private string direction;   // current direction that SnakeHead is facing
    private float oldX;         // SnakeHead old x position
    private float oldY;         // SnakeHead old y position
    private int idxX;           // x position index in gameBoard
    private int idxY;           // y position index in gameBoard

    private float max_x = (float)7.8;       // map boundary in x axis
    private float min_x = (float)-7.8;      // map boundary in x axis
    private float max_y = (float)7.8;       // map boundary in y axis
    private float min_y = (float)-7.8;      // map boundary in y axis

    private float cellSize = (float)0.4;      // how much to move

    // Constructor
    public SnakeHead()
    {
        gameObj = GameObject.Find("SnakeHead");
        nextObj = null;
    }

    // Methods
    public void move(float nextX, float nextY)
    {
        oldX = gameObj.transform.localPosition.x;
        oldY = gameObj.transform.localPosition.y;
        gameLogic.addGameBoard(nextX, nextY);
        gameLogic.removeGameBoard(oldX, oldY);

        gameObj.transform.localPosition = new Vector3(nextX, nextY, 0);
    }

    public bool checkValidBounds(float nextX, float nextY)
    {
        if (nextX > max_x || nextX < min_x || nextY > max_y || nextY < min_y)
        {
            //Debug.Log("Died at: " + gameObj.transform.localPosition.x + "," + gameObj.transform.localPosition.y);
            return false;
        }
        return true;
    }

    public bool checkValidMove(float nextX, float nextY)
    {
        gameBoard = gameLogic.getGameBoard();

        idxX = (int)System.Math.Abs(System.Math.Round((nextX + max_x) / cellSize, 1));
        idxY = (int)System.Math.Abs(System.Math.Round((nextY + min_x) / cellSize, 1));
        if (gameBoard[idxY, idxX] == 1)
        {
            //Debug.Log("Died at: " + gameObj.transform.localPosition.x + "," + gameObj.transform.localPosition.y);
            return false;
        }
        return true;
    }

    public bool checkInvalidMovement(string dir)
    {
        switch (dir)
        {
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
            default:
                return false;
        }
    }

    // Setter methods
    public void setGameBoard(int[,] gb)
    {
        gameBoard = gb;
    }

    public void setNextObj(SnakeBody nObj)
    {
        nextObj = nObj;
    }

    public void setDirection(string newDir)
    {
        direction = newDir;
    }

    public void setGameLogic(GameLogic gl)
    {
        gameLogic = gl;
    }

    // Getter methods
    public float getOldX()
    {
        return oldX;
    }

    public float getOldY()
    {
        return oldY;
    }

    public float getX()
    {
        return gameObj.transform.localPosition.x;
    }

    public float getY()
    {
        return gameObj.transform.localPosition.y;
    }

    public string getDirection()
    {
        return direction;
    }

    public string getOppDirection()
    {
        switch (direction)
        {
            case "up":
                return "down";
            case "down":
                return "up";
            case "left":
                return "right";
            case "right":
                return "left";
            default:
                return "N/A";
        }
    }

    public GameObject getGameObj()
    {
        return gameObj;
    }

    public SnakeBody getNextObj()
    {
        return nextObj;
    }
}
