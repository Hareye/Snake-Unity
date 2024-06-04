using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody
{
    private GameObject gameObj;     // GameObject that this object references
    private GameLogic gameLogic;    // reference to GameLogic script
    private SnakeBody nextObj;      // reference to next SnakeBody

    private float oldX; // SnakeBody old x position
    private float oldY; // SnakeBody old y position

    // Constructor
    public SnakeBody(GameObject obj)
    {
        gameObj = obj;
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

    public void initialMove(float nextX, float nextY)
    {
        gameObj.transform.localPosition = new Vector3(nextX, nextY, 0);
    }

    // Setter methods
    public void setNextObj(SnakeBody nObj)
    {
        nextObj = nObj;
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

    public GameObject getGameObj()
    {
        return gameObj;
    }

    public SnakeBody getNextObj()
    {
        return nextObj;
    }
}