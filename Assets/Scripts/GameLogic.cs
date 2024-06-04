using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public GameObject snakeHead;    // reference to GameObject snakeHead
    public GameObject snakeBody;    // reference to GameObject snakeBody
    public GameObject apple;        // reference to GameObject apple

    private SnakeHeadController snakeHeadController;    // reference to SnakeHeadController script
    private SnakeBodyController snakeBodyController;    // reference to SnakeBodyController script
    private AppleLogic appleLogic;                      // reference to AppleLogic script

    private int[,] gameBoard;

    private float max = (float)7.8;         // max map boundary
    private float min = (float)-7.8;        // max map boundary

    private float cellSize = (float)0.4;    // how much to move
    private float offset = (float)0.2;      // offset in the cell position

    private bool gameOver;                  // whether game is over
    private bool grab;                      // whether snake has grabbed apple

    void Start()
    {
        Debug.Log("Getting AppleLogic... (GameLogic)");
        appleLogic = apple.GetComponent<AppleLogic>();

        Debug.Log("Getting SnakeHeadController... (GameLogic)");
        snakeHeadController = snakeHead.GetComponent<SnakeHeadController>();

        Debug.Log("Getting SnakeBodyController... (GameLogic)");
        snakeBodyController = snakeBody.GetComponent<SnakeBodyController>();

        Debug.Log("Initializing gameBoard... (GameLogic)");
        gameBoard = new int[40, 40];

        float startX = (float)System.Math.Round(snakeHead.transform.localPosition.x, 1);
        float startY = (float)System.Math.Round(snakeHead.transform.localPosition.y, 1);
        addGameBoard(startX, startY);

        Debug.Log("Initializing parameters... (GameLogic)");
        grab = false;
        gameOver = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Print out current index of entire snake
            SnakeHead sh = snakeHeadController.getSnakeHead();
            SnakeBody sb;

            int idxX = (int)System.Math.Abs(System.Math.Round((snakeHead.transform.localPosition.x + max) / cellSize, 1));
            int idxY = (int)System.Math.Abs(System.Math.Round((snakeHead.transform.localPosition.y + min) / cellSize, 1));

            Debug.Log(idxX + "," + idxY);

            if (sh.getNextObj() != null)
            {
                sb = sh.getNextObj();

                while (sb != null)
                {
                    GameObject go = sb.getGameObj();

                    idxX = (int)System.Math.Abs(System.Math.Round((go.transform.localPosition.x + max) / cellSize, 1));
                    idxY = (int)System.Math.Abs(System.Math.Round((go.transform.localPosition.y + min) / cellSize, 1));

                    Debug.Log(idxX + "," + idxY);

                    sb = sb.getNextObj();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            restartGame();
        }

        checkGrab();
    }
    
    public void restartGame()
    {
        gameOver = false;
        grab = false;

        // Reset all snakeHead parameters
        snakeHeadController.getSnakeHead().move((float)0.2, (float)0.2);
        snakeHeadController.getSnakeHead().setDirection("");
        snakeHeadController.setCoroutine(true);
        snakeHeadController.setStarted(false);
        snakeHeadController.setSpeed(0.25f);

        // Destroy all SnakeBody GameObjects and remove reference from snakeHead
        if (snakeHeadController.getSnakeHead().getNextObj() != null)
        {
            SnakeBody snakeBodyPtr = snakeHeadController.getSnakeHead().getNextObj();

            while (snakeBodyPtr != null)
            {
                SnakeBody temp = snakeBodyPtr;
                snakeBodyPtr = snakeBodyPtr.getNextObj();
                Destroy(temp.getGameObj());
            }

            snakeHeadController.getSnakeHead().setNextObj(null);
        }

        // Reset snakeBody parameters
        snakeBodyController.setLength(1);

        // Respawn apple
        appleLogic.spawnApple();
    }

    public void addGameBoard(float x, float y)
    {
        int idxX = (int)System.Math.Abs(System.Math.Round((x + max) / cellSize, 1));
        int idxY = (int)System.Math.Abs(System.Math.Round((y + min) / cellSize, 1));
        gameBoard[idxY, idxX] = 1;
    }

    public void removeGameBoard(float x, float y)
    {
        int idxX = (int)System.Math.Abs(System.Math.Round((x + max) / cellSize, 1));
        int idxY = (int)System.Math.Abs(System.Math.Round((y + min) / cellSize, 1));
        gameBoard[idxY, idxX] = 0;
    }

    public void checkGrab()
    {
        if (snakeHeadController.getSnakeHead().getX() == appleLogic.getSpawnX() &&
            snakeHeadController.getSnakeHead().getY() == appleLogic.getSpawnY() &&
            appleLogic.getExists() && grab == false)
        {
            grabApple();
        }
    }

    public void grabApple()
    {
        Debug.Log("Grabbed apple...");
        grab = true;
        appleLogic.setExists(false);
        snakeBodyController.newBody();
    }

    public void setGrab(bool c)
    {
        grab = c;
    }

    public void setGameOver(bool c)
    {
        gameOver = c;
    }

    public int[,] getGameBoard()
    {
        return gameBoard;
    }

    public bool getGameOver()
    {
        return gameOver;
    }

    public float getCellSize()
    {
        return cellSize;
    }

    public float getOffset()
    {
        return offset;
    }

    public float getMax()
    {
        return max;
    }

    public float getMin()
    {
        return min;
    }
}
