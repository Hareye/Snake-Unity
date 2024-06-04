using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    public GameObject snakeHead;    // reference to GameObject snakeHead
    public GameObject game;         // reference to GameObject game

    private SnakeHeadController snakeHeadController;    // reference to SnakeHeadController script
    private SnakeBody currentTail;                      // reference to current snake tail

    private int length; // snake length
    private float toMove = (float)0.4;      // how much to move

    void Start()
    {
        Debug.Log("Getting snakeHeadController... (SnakeBodyController)");
        snakeHeadController = snakeHead.GetComponent<SnakeHeadController>();

        Debug.Log("Setting parameters... (SnakeBodyController)");
        length = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            newBody();
        }
    }

    public void newBody()
    {
        // Instantiate new GameObject snakeBodyObj and SnakeBody snakeBody 
        GameObject snakeBodyObj = Instantiate(GameObject.Find("SnakeBody"), new Vector3((float)4.2, (float)4.2, 0), Quaternion.identity);
        SnakeBody snakeBody = new SnakeBody(snakeBodyObj);
        snakeBody.setGameLogic(game.GetComponent<GameLogic>());

        float nextX;
        float nextY;

        // Remove script from the new instantiated GameOBject
        Destroy(snakeBodyObj.GetComponent<SnakeBodyController>());

        snakeBodyObj.name = "SnakeBody" + length;
        snakeBodyObj.transform.parent = GameObject.Find("Game").transform;
        snakeBodyObj.transform.localScale = new Vector3(1, 1, 1);

        if (snakeHeadController.getSnakeHead().getNextObj() == null)
        {
            // If only snakeHead exists, move new SnakeBody GameObject to SnakeHead old position
            snakeHeadController.getSnakeHead().setNextObj(snakeBody);
            nextX = snakeHeadController.getSnakeHead().getOldX();
            nextY = snakeHeadController.getSnakeHead().getOldY();
        }
        else
        {
            // If SnakeBody exists, move new SnakeBody GameObject to last SnakeBody old position
            SnakeBody snakeBodyPtr = snakeHeadController.getSnakeHead().getNextObj();
            nextX = snakeBodyPtr.getOldX();
            nextY = snakeBodyPtr.getOldY();

            while (snakeBodyPtr.getNextObj() != null)
            {
                nextX = snakeBodyPtr.getNextObj().getOldX();
                nextY = snakeBodyPtr.getNextObj().getOldY();
                snakeBodyPtr = snakeBodyPtr.getNextObj();
            }

            snakeBodyPtr.setNextObj(snakeBody);
        }

        // Make the initial movement
        snakeBody.initialMove(nextX, nextY);
        snakeHeadController.setSpeed(snakeHeadController.getSpeed() - 0.01f);
        length++;
    }

    public void setLength(int l)
    {
        length = l;
    }
}
