using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour, SnakeBody
{
    private GameObject[,] gameBoard;                    // tracks snake head and body positions

    private SnakeHeadController snakeHeadController;    // snake head controller script
    private GameLogic gameLogic;                        // game logic script

    private GameObject snakeBody;   // new snakeBody gameobject
    private GameObject snakeTail;   // GameObject that is currently the snake tail

    private float newPosX;          // new body x position
    private float newPosY;          // new body y position
    private int idxX;               // snakeBody new board index X
    private int idxY;               // snakeBody new board index Y

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting gameLogic... (SnakeBodyController)");
        gameLogic = GameObject.Find("NewGame").GetComponent<GameLogic>();

        Debug.Log("Getting snakeHead... (SnakeBodyController)");
        snakeHeadController = GameObject.Find("SnakeHead").GetComponent<SnakeHeadController>();

        Debug.Log("Initializing parameters... (SnakeBodyController)");
        snakeTail = GameObject.Find("SnakeHead");
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing happens
    }

    public void resetTail()
    {
        snakeTail = snakeTail = GameObject.Find("SnakeHead");
    }

    public void newBody()
    {
        snakeBody = Instantiate(GameObject.Find("SnakeBody"), new Vector3((float)20.2, (float)0.4, 10), Quaternion.identity);
        snakeBody.name = "SnakeBody" + gameLogic.getSnakeLength();

        snakeBody.transform.parent = gameLogic.transform;
        snakeBody.transform.localScale = new Vector3(1, 1, 1);

        if (snakeTail == GameObject.Find("SnakeHead"))
            initPosNewBody();
        else
            posNewBody();

        if (gameLogic.getSpeed() > 0.05)
            gameLogic.setSpeed((float)System.Math.Round((gameLogic.getSpeed() - 0.01f), 2));
    }

    public void posNewBody()
    {
        GameObject beforeTail = GameObject.Find("SnakeBody" + (gameLogic.getSnakeLength() - 2).ToString());

        if (beforeTail == null)
            beforeTail = GameObject.Find("SnakeHead");

        float firstX = (float)System.Math.Round(beforeTail.transform.localPosition.x, 1);
        float firstY = (float)System.Math.Round(beforeTail.transform.localPosition.y, 1);

        if (firstX == (float)System.Math.Round(snakeTail.transform.localPosition.x + gameLogic.getToMove(), 1))
        {
            // spawn left
            newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x - gameLogic.getToMove(), 1);
            newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y, 1);
        } else if (firstX == (float)System.Math.Round(snakeTail.transform.localPosition.x - gameLogic.getToMove(), 1))
        {
            // spawn right
            newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x + gameLogic.getToMove(), 1);
            newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y, 1);
        } else if (firstY == (float)System.Math.Round(snakeTail.transform.localPosition.y + gameLogic.getToMove(), 1))
        {
            // spawn down
            newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x, 1);
            newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y - gameLogic.getToMove(), 1);
        } else if (firstY == (float)System.Math.Round(snakeTail.transform.localPosition.y - gameLogic.getToMove(), 1))
        {
            // spawn up
            newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x, 1);
            newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y + gameLogic.getToMove(), 1);
        }
        snakeBody.transform.localPosition = new Vector3(newPosX, newPosY, 1);
        snakeTail = snakeBody;
        updateBoard();
    }

    public void initPosNewBody()
    {
        switch (snakeHeadController.getDirection())
        {
            case "left":
                newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x + gameLogic.getToMove(), 1);
                newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y, 1);
                break;
            case "right":
                newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x - gameLogic.getToMove(), 1);
                newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y, 1);
                break;
            case "up":
                newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x, 1);
                newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y - gameLogic.getToMove(), 1);
                break;
            case "down":
                newPosX = (float)System.Math.Round(snakeTail.transform.localPosition.x, 1);
                newPosY = (float)System.Math.Round(snakeTail.transform.localPosition.y + gameLogic.getToMove(), 1);
                break;
        }

        snakeBody.transform.localPosition = new Vector3(newPosX, newPosY, 1);
        snakeTail = snakeBody;
        updateBoard();
    }

    public void updateBoard()
    {
        idxX = (int)System.Math.Abs(System.Math.Round((newPosX + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
        idxY = (int)System.Math.Abs(System.Math.Round((newPosY + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
        gameLogic.setBoard(this.gameObject, idxX, idxY);
        //Debug.Log("Adding body to: " + idxY + "," + idxX);
    }
}
