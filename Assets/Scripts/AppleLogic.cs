using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppleLogic : MonoBehaviour
{
    // New code using SnakeHeadController and SnakeBodyController

    private GameObject[,] gameBoard;                // tracks snake head and body positions

    private SnakeHeadController snakeController;    // snake head controller script
    private GameLogic gameLogic;                    // game logic script

    private float spawnX;       // apple spawn x position
    private float spawnY;       // apple spawn y position
    private int idxX;           // corresponding index X in gameBoard
    private int idxY;           // corresponding index Y in gameBoard

    private bool exists;        // whether apple currently exists
    private bool spawnValid;    // whether apple is spawning in a valid location

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting gameLogic... (AppleLogic)");
        gameLogic = GameObject.Find("NewGame").GetComponent<GameLogic>();

        Debug.Log("Grabbing gameBoard... (AppleLogic)");
        gameBoard = gameLogic.getBoard();

        Debug.Log("Initializing parameters... (AppleLogic)");
        exists = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (exists == false)
        {
            spawnApple();
            gameLogic.setGrab(false);
        }
    }

    public void spawnApple()
    {
        while (!spawnValid)
        {
            int randomX = Random.Range(-20, 19);
            int randomY = Random.Range(-20, 19);
            spawnX = (float)System.Math.Round(gameLogic.getCellSize() * randomX + gameLogic.getOffset(), 1);
            spawnY = (float)System.Math.Round(gameLogic.getCellSize() * randomY + gameLogic.getOffset(), 1);

            checkSpawnValid(spawnX, spawnY);
        }

        Debug.Log("Spawned apple at: " + spawnX + " " + spawnY);

        this.gameObject.transform.localPosition = new Vector3(spawnX, spawnY);
        exists = true;
        spawnValid = false;

        //gameLogic.setGrab(false);
    }

    public void checkSpawnValid(float x, float y)
    {
        idxX = (int)System.Math.Abs(System.Math.Round((x + gameLogic.getMax()) / gameLogic.getCellSize(), 1));
        idxY = (int)System.Math.Abs(System.Math.Round((y + gameLogic.getMin()) / gameLogic.getCellSize(), 1));
        if (gameBoard[idxY, idxX] == null)
            spawnValid = true;
        else
            spawnValid = false;
    }

    public void setExists(bool cond)
    {
        exists = cond;
    }

    public float getSpawnX()
    {
        return spawnX;
    }

    public float getSpawnY()
    {
        return spawnY;
    }
}