using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // New code using SnakeHeadController

    private GameObject[,] gameBoard;    // tracks snake head and body positions

    private SnakeHeadController snakeHeadController;    // snake head controller script
    private GameObject snakeHead;                       // snake head

    private SnakeBodyController snakeBodyController;    // snake body controller script

    private AppleLogic appleLogic;          // apple logic script

    private float startX;   // snake head starting X pos
    private float startY;   // snake head starting Y pos
    private int idxX;       // corresponding index X in gameBoard
    private int idxY;       // corresponding index Y in gameBoard

    private float max_x = (float)7.8;       // map boundary in x axis
    private float min_x = (float)-7.8;      // map boundary in x axis
    private float max_y = (float)7.8;       // map boundary in y axis
    private float min_y = (float)-7.8;      // map boundary in y axis

    private float cellSize = (float)0.4;    // cell size
    private float toMove = (float)0.4;      // how much to move (0.4 per cell)
    private float offset = (float)0.2;      // offset in the cell position

    private float snakeSpeed = 0.25f;       // snake speed
    private int snakeLength;                // snake length
    private bool gameOver;                  // whether game is over
    private bool continueCoroutine;         // controls coroutine
    private bool grab;                      // whether snake has grabbed apple
    private bool started;                   // whether game has started

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting snakeHead... (GameLogic)");
        snakeHead = GameObject.Find("SnakeHead");
        snakeHeadController = snakeHead.GetComponent<SnakeHeadController>();

        Debug.Log("Getting appleLogic... (GameLogic)");
        appleLogic = GameObject.Find("Apple").GetComponent<AppleLogic>();

        Debug.Log("Getting snakeBody... (SnakeBodyController)");
        snakeBodyController = GameObject.Find("SnakeBody").GetComponent<SnakeBodyController>();

        Debug.Log("Initializing board... (GameLogic)");
        gameBoard = new GameObject[40, 40];

        // Store snakeHead into corresponding gameBoard index on start
        startX = (float)System.Math.Round(snakeHead.transform.localPosition.x, 1);
        startY = (float)System.Math.Round(snakeHead.transform.localPosition.y, 1);
        idxX = (int)System.Math.Abs(System.Math.Round((startX + max_x) / 0.4, 1));
        idxY = (int)System.Math.Abs(System.Math.Round((startY + min_x) / 0.4, 1));
        gameBoard[idxY, idxX] = snakeHead;

        Debug.Log("Initializing parameters... (GameLogic)");
        gameOver = false;
        continueCoroutine = true;
        grab = false;
        started = false;
        snakeLength = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            snakeHead.transform.localPosition = new Vector3((float)0.2, (float)0.2, 1);
            for (var i = 1; i < snakeLength; i++)
            {
                string name = "SnakeBody" + i.ToString();
                GameObject obj = GameObject.Find(name);
                Destroy(obj);
            }
            snakeBodyController.resetTail();
            snakeSpeed = 0.25f;
            Start();
        }
        if (!gameOver)
        {
            // Key press events
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!snakeHeadController.checkInvalidMovement("up"))
                {
                    snakeHeadController.setDirection("up");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //snakeHeadController.prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!snakeHeadController.checkInvalidMovement("down"))
                {
                    snakeHeadController.setDirection("down");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //snakeHeadController.prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!snakeHeadController.checkInvalidMovement("left"))
                {
                    snakeHeadController.setDirection("left");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //snakeHeadController.prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!snakeHeadController.checkInvalidMovement("right"))
                {
                    snakeHeadController.setDirection("right");
                    if (started == false)
                    {
                        StartCoroutine(moveSnake());
                        started = true;
                    }
                    //snakeHeadController.prepareMove();
                }
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                // Just for development purposes - logs out snake position and whether corresponding array contains the correct object
                startX = (float)System.Math.Round(snakeHead.transform.localPosition.x, 1);
                startY = (float)System.Math.Round(snakeHead.transform.localPosition.y, 1);
                idxX = (int)System.Math.Abs(System.Math.Round((startX + max_x) / 0.4, 1));
                idxY = (int)System.Math.Abs(System.Math.Round((startY + min_x) / 0.4, 1));
                Debug.Log("Head: " + idxY + "," + idxX);

                for (var i = 1; i < getSnakeLength(); i++)
                {
                    string name = "SnakeBody" + i.ToString();
                    GameObject obj = GameObject.Find(name);
                    idxX = (int)System.Math.Abs(System.Math.Round(((float)System.Math.Round(obj.transform.localPosition.x, 1) + getMax()) / getCellSize(), 1));
                    idxY = (int)System.Math.Abs(System.Math.Round(((float)System.Math.Round(obj.transform.localPosition.y, 1) + getMin()) / getCellSize(), 1));
                    Debug.Log("Body" + i + ": " + idxY + "," + idxX);
                    //Debug.Log(gameBoard[idxY, idxX]);
                }
                //Debug.Log(gameBoard[idxY, idxX]);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                // Just for development purposes - adds new snakeBody to snake
                snakeBodyController.newBody();
                snakeLength++;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Length: " + snakeLength);
            }

            // Check if snake grabs apple (checks using same coordinates after snake moves)
            if (appleLogic.getSpawnX() == snakeHeadController.getSnakeHeadX() &&
                appleLogic.getSpawnY() == snakeHeadController.getSnakeHeadY() &&
                grab == false)
            {
                grabApple();
            }
        }
    }

    public void grabApple()
    {
        grab = true;
        snakeBodyController.newBody();
        appleLogic.setExists(false);
        snakeLength++;
    }

    public bool checkValidMove(float nextX, float nextY)
    {
        idxX = (int)System.Math.Abs(System.Math.Round((nextX + max_x) / 0.4, 1));
        idxY = (int)System.Math.Abs(System.Math.Round((nextY + min_x) / 0.4, 1));
        if (gameBoard[idxY, idxX] != null)
        {
            Debug.Log("Died at: " + snakeHead.transform.localPosition.x + "," + snakeHead.transform.localPosition.y);
            gameOver = true;
            return false;
        }
        return true;
    }

    public bool checkValidBounds(float nextX, float nextY)
    {
        if (nextX > max_x || nextX < min_x || nextY > max_y || nextY < min_y)
        {
            Debug.Log("Died at: " + snakeHead.transform.localPosition.x + "," + snakeHead.transform.localPosition.y);
            gameOver = true;
            //snakeHead.SetActive(false);
            return false;
        }
        return true;
    }

    private IEnumerator moveSnake()
    {
        while (continueCoroutine)
        {
            if (gameOver == false)
            {
                snakeHeadController.prepareMove();
                yield return new WaitForSeconds(snakeSpeed);
            }
            else if (gameOver == true)
            {
                Debug.Log("Game Over...");
                continueCoroutine = false;
                StopCoroutine(moveSnake());
            }
        }
    }

    public void setBoard(GameObject obj, int x, int y)
    {
        gameBoard[y, x] = obj;
    }

    public void setGrab(bool cond)
    {
        grab = cond;
    }

    public void setSpeed(float spd)
    {
        snakeSpeed = spd;
    }

    public void removeBoard(int x, int y)
    {
        gameBoard[y, x] = null;
    }

    public GameObject[,] getBoard()
    {
        return gameBoard;
    }

    public float getMax()
    {
        return max_x;
    }

    public float getMin()
    {
        return min_x;
    }

    public float getToMove()
    {
        return toMove;
    }

    public float getCellSize()
    {
        return cellSize;
    }

    public float getOffset()
    {
        return offset;
    }

    public int getSnakeLength()
    {
        return snakeLength;
    }

    public float getSpeed()
    {
        return snakeSpeed;
    }
}
