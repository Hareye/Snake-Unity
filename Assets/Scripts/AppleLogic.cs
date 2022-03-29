using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppleLogic : MonoBehaviour
{
    private SnakeController snakeController;    // snake controller script
    private GameObject theSnake;                // the snake
    private GameLogic gameLogic;                // game logic script
    private GameObject theGame;                 // the game

    private float currentX;                     // current x position
    private float currentY;                     // current y position

    private bool exists;

    private List<GameObject> snake;             // list of snake pos
    private int length;
    private bool spawnValid;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting snake in GameLogic...");
        theSnake = GameObject.Find("Snake");
        snakeController = theSnake.GetComponent<SnakeController>();

        Debug.Log("Getting game logic from GameLogic...");
        theGame = GameObject.Find("Game");
        gameLogic = theGame.GetComponent<GameLogic>();

        Debug.Log("Initializing apple in AppleLogic...");
        exists = false;
        spawnValid = false;
    }

    // Update is called once per frame
    void Update()
    {
        spawnApple();
    }

    public void setExists(bool state)
    {
        exists = state;
    }

    public void spawnApple()
    {
        if (exists == false)
        {
            while (!spawnValid)
            {
                var randomX = Random.Range(-20, 19);
                var randomY = Random.Range(-20, 19);
                currentX = (float)System.Math.Round(0.4 * randomX + 0.2, 1);
                currentY = (float)System.Math.Round(0.4 * randomY + 0.2, 1);

                if (checkSpawnValid())
                {
                    spawnValid = true;
                }
            }

            Debug.Log("Spawned apple at: " + currentX + " " + currentY);

            this.gameObject.transform.localPosition = new Vector3(currentX, currentY);
            exists = true;
            spawnValid = false;

            gameLogic.setGrab(false);
        }
    }

    public bool checkSpawnValid()
    {
        snake = snakeController.getSnake();
        length = snakeController.getLength();

        for (int i = 0; i <= length; i++)
        {
            if ((float)System.Math.Round(snake[i].transform.localPosition.x, 1) == currentX &&
                (float)System.Math.Round(snake[i].transform.localPosition.y, 1) == currentY)
            {
                return false;
            }
        }
        return true;
    }
}