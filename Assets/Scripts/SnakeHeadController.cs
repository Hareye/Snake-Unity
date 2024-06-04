using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour
{
    public GameObject game;         // reference to GameObject game

    private SnakeHead snakeHead;    // reference to SnakeHead class object
    private GameLogic gameLogic;    // reference to GameLogic script

    private bool continueCoroutine;         // controls coroutine
    private bool started;                   // whether movement has started

    private float snakeSpeed = 0.25f;       // snake speed
    private float maxSpeed = 0.05f;         // max snake speed
    private float toMove = (float)0.4;      // how much to move

    void Start()
    {
        Debug.Log("Getting gameLogic... (SnakeHeadController)");
        gameLogic = game.GetComponent<GameLogic>();

        Debug.Log("Creating SnakeHead object... (SnakeHeadController)");
        snakeHead = new SnakeHead();
        snakeHead.setGameLogic(gameLogic);

        Debug.Log("Initializing parameters... (SnakeHeadController)");
        continueCoroutine = true;
        started = false;
    }

    void Update()
    {
        if (!gameLogic.getGameOver())
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!snakeHead.checkInvalidMovement("up"))
                {
                    snakeHead.setDirection("up");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!snakeHead.checkInvalidMovement("down"))
                {
                    snakeHead.setDirection("down");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!snakeHead.checkInvalidMovement("left"))
                {
                    snakeHead.setDirection("left");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!snakeHead.checkInvalidMovement("right"))
                {
                    snakeHead.setDirection("right");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //prepareMove();
                }
            }
        }
    }

    public void prepareMove()
    {
        float nextX = 0;
        float nextY = 0;

        switch (snakeHead.getDirection())
        {
            case "up":
                nextX = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.x, 1);
                nextY = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.y + toMove, 1);
                break;

            case "down":
                nextX = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.x, 1);
                nextY = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.y - toMove, 1);
                break;

            case "left":
                nextX = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.x - toMove, 1);
                nextY = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.y, 1);
                break;

            case "right":
                nextX = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.x + toMove, 1);
                nextY = (float)System.Math.Round(snakeHead.getGameObj().transform.localPosition.y, 1);
                break;
        }

        if (snakeHead.checkValidBounds(nextX, nextY) && snakeHead.checkValidMove(nextX, nextY))
        {
            snakeHead.move(nextX, nextY);

            if (snakeHead.getNextObj() != null)
            {
                SnakeBody snakeBodyPtr = snakeHead.getNextObj();

                nextX = snakeHead.getOldX();
                nextY = snakeHead.getOldY();

                while (snakeBodyPtr != null)
                {
                    snakeBodyPtr.move(nextX, nextY);
                    nextX = snakeBodyPtr.getOldX();
                    nextY = snakeBodyPtr.getOldY();
                    snakeBodyPtr = snakeBodyPtr.getNextObj();
                }
            }
        } else
        {
            gameLogic.setGameOver(true);
        }
    }

    private IEnumerator moveSnake()
    {
        while (continueCoroutine)
        {
            if (!gameLogic.getGameOver())
            {
                prepareMove();
                //updateScore();
                yield return new WaitForSeconds(snakeSpeed);
            }
            else if (gameLogic.getGameOver())
            {
                Debug.Log("Game Over...");
                continueCoroutine = false;
                StopCoroutine(moveSnake());
            }
        }
    }

    public void setSpeed(float s)
    {
        if (snakeSpeed > maxSpeed)
            snakeSpeed = s;
    }

    public void setStarted(bool c)
    {
        started = c;
    }

    public void setCoroutine(bool c)
    {
        continueCoroutine = c;
    }

    public SnakeHead getSnakeHead()
    {
        return snakeHead;
    }

    public bool getStarted()
    {
        return started;
    }

    public float getSpeed()
    {
        return snakeSpeed;
    }
}