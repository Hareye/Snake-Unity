using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private SnakeController snakeController;    // snake controller script
    private GameObject theSnake;                // the snake
    private AppleLogic appleLogic;              // apple logic script
    private GameObject theApple;                // the apple

    // formula = cells * cell_size - offset
    // cells = 20, cell_size = 0.4, offset = 0.2
    private float max_x = (float)7.8;  // map boundary in x axis
    private float min_x = (float)-7.8; // map boundary in x axis
    private float max_y = (float)7.8;  // map boundary in y axis
    private float min_y = (float)-7.8; // map boundary in y axis

    private float snakeSpeed = 0.25f;
    private bool gameOver;
    private bool grab;
    private bool continueCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting snake in GameLogic...");
        theSnake = GameObject.Find("Snake");
        snakeController = theSnake.GetComponent<SnakeController>();

        Debug.Log("Getting apple in GameLogic...");
        theApple = GameObject.Find("Apple");
        appleLogic = theApple.GetComponent<AppleLogic>();

        Debug.Log("Initializing game logic...");
        gameOver = false;
        grab = false;
        continueCoroutine = true;

        StartCoroutine(moveSnake());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            // If snake grabbed apple (checked using same coordinates)
            if ((theApple.transform.localPosition.x == theSnake.transform.localPosition.x) &&
                (theApple.transform.localPosition.y == theSnake.transform.localPosition.y) &&
                 grab == false)
            {
                grabApple();
            }

            // Key Press Events
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!snakeController.checkUpHead())
                {
                    Debug.Log("Up");
                    snakeController.setUp();
                } else
                {
                    Debug.Log("Cannot do that (up)");
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!snakeController.checkDownHead())
                {
                    Debug.Log("Down");
                    snakeController.setDown();
                } else
                {
                    Debug.Log("Cannot do that (down)");
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!snakeController.checkLeftHead())
                {
                    Debug.Log("Left");
                    snakeController.setLeft();
                } else
                {
                    Debug.Log("Cannot do that (left)");
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!snakeController.checkRightHead())
                {
                    Debug.Log("Right");
                    snakeController.setRight();
                } else
                {
                    Debug.Log("Cannot do that (right)");
                }
            }
        }
    }

    public void setGrab(bool state)
    {
        grab = state;
    }

    public void setGameOver(bool state)
    {
        gameOver = state;
    }

    public void setSpeed(float newSpeed)
    {
        Debug.Log("Changing speed to: " + newSpeed);
        snakeSpeed = newSpeed;
    }

    public float getSpeed()
    {
        return (float)System.Math.Round(snakeSpeed, 2);
    }

    private void grabApple()
    {
        /*  Flow
         *  1. Set grab to true so that this function only runs once
         *  2. Add length to snake (act as score)
         *  3. Set values so that we are ready to spawn a new snake body at the coordinates where snake grabbed the apple
         *  4. Set exists to false so that new apple will spawn with new coordinates
         */
        grab = true;
        snakeController.newBody();
        appleLogic.setExists(false);
    }

    private IEnumerator moveSnake()
    {
        while (continueCoroutine)
        {
            if (gameOver == false)
            {

                // Move snake
                if (snakeController.getUp())
                {
                    snakeController.moveBody();
                    snakeController.moveHeadUp();
                }
                else if (snakeController.getDown())
                {
                    snakeController.moveBody();
                    snakeController.moveHeadDown();
                }
                else if (snakeController.getLeft())
                {
                    snakeController.moveBody();
                    snakeController.moveHeadLeft();
                }
                else if (snakeController.getRight())
                {
                    snakeController.moveBody();
                    snakeController.moveHeadRight();
                }

                snakeController.checkSnake();   // check whether snake has hit his own body
                checkBoundaries();              // check whether snake has hit the wall
                yield return new WaitForSeconds(snakeSpeed);

            } else if (gameOver == true)
            {
                Debug.Log("Game Over...");
                continueCoroutine = false;
                StopCoroutine(moveSnake());
            }
        }
    }

    private void checkBoundaries()
    {
        Vector3 currentPos = theSnake.transform.localPosition;

        if (currentPos.x > max_x)
        {

            Debug.Log("Died at: " + currentPos.x);
            gameOver = true;
            theSnake.SetActive(false);  // hide snake

        } else if (currentPos.x < min_x)
        {

            Debug.Log("Died at: " + currentPos.x);
            gameOver = true;
            theSnake.SetActive(false);  // hide snake

        } else if (currentPos.y > max_y)
        {

            Debug.Log("Died at: " + currentPos.y);
            gameOver = true;
            theSnake.SetActive(false);  // hide snake

        } else if (currentPos.y < min_y)
        {

            Debug.Log("Died at: " + currentPos.y);
            gameOver = true;
            theSnake.SetActive(false);  // hide snake

        }
    }
}
