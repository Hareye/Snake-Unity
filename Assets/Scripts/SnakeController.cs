using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    // Need to prevent snake from going up if it's already going down... (setting conditions in if won't work because player can just hit left, then up super fast)
    // Would be better to check whether there is an object behind the head in the List(?)

    // Make a list of GameObjects consisting of snake body. iterate through and translate them maybe(?)

    private GameLogic gameLogic;        // game logic script
    private GameObject theGame;         // the game

    private GameObject body;            // new body gameobject

    private int length;     // length of snake = position of tail
    private bool up;        // is snake looking up
    private bool down;      // is snake looking down
    private bool left;      // is snake looking left
    private bool right;     // is snake looking right

    private float toMove;   // how much to move
    private float x;        // x local position
    private float y;        // y local position

    private GameObject currentTail;     // snake tail
    private float currentTailX;         // snake tail x
    private float currentTailY;         // snake tail y

    private GameObject nextTail;        // before snake tail
    private float nextTailX;            // before snake tail x
    private float nextTailY;            // before snake tail y

    private GameObject currentBody;     // current snake body
    private GameObject nextBody;        // next snake body
    private float nextPosX;             // next snake body x
    private float nextPosY;             // next snake body y
    private float tempX;                // temp x
    private float tempY;                // temp y

    private float max_x = (float)7.8;   // map boundary in x axis
    private float min_x = (float)-7.8;  // map boundary in x axis
    private float max_y = (float)7.8;   // map boundary in y axis
    private float min_y = (float)-7.8;  // map boundary in y axis

    private List<GameObject> snake = new List<GameObject>();    // snake gameobjects

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Grabbing game logic from GameLogic...");
        theGame = GameObject.Find("Game");
        gameLogic = theGame.GetComponent<GameLogic>();

        Debug.Log("Initializing snake in SnakeController...");
        length = 0;
        up = false;
        down = false;
        left = false;
        right = false;

        snake.Add(this.gameObject);

        toMove = (float)System.Math.Round(0.4, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing happens
    }

    private void addLength()
    {
        length++;
        Debug.Log("Length: " + length);
    }

    public void newBody()
    {
        body = Instantiate(GameObject.Find("SnakeBody"), new Vector3(10, 10, 10), Quaternion.identity);
        body.name = "Snake" + length;

        spawnTail(body);

        snake.Add(body);
        addLength();

        if (gameLogic.getSpeed() > 0.10)
        {
            gameLogic.setSpeed((float)System.Math.Round((gameLogic.getSpeed() - 0.01f), 2));
        }
    }

    private void spawnTail(GameObject body)
    {
        currentTail = snake[length];

        if (length != 0)
        {
            // If Snake is more than just a head
            nextTail = snake[length - 1];
        }

        body.transform.parent = gameLogic.transform;

        if (length != 0)
        {
            // If Snake is more than just a head
            // Check which direction the next body part after tail is moving
            if (checkTailUp(currentTail, nextTail))
            {
                // Check where it can spawn
                if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                }
                else if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                }
                else if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                }
            } else if (checkTailDown(currentTail, nextTail))
            {
                // Check where it can spawn
                if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                }
                else if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                }
                else if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                }
            } else if (checkTailRight(currentTail, nextTail))
            {
                // Check where it can spawn
                if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                }
                else if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                }
                else if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                }
            } else if (checkTailLeft(currentTail, nextTail))
            {
                // Check where it can spawn
                if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                }
                else if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                }
                else if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                }
            }
        } else
        {
            // If Snake is only just a head
            // Check which direction snake head is moving
            if (left)
            {
                // Check where it can spawn
                if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                } else if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                } else if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                }
            } else if (right)
            {
                // Check where it can spawn
                if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                } else if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                } else if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                }
            } else if (up)
            {
                // Check where it can spawn
                if (checkDown(currentTail))
                {
                    spawnDown(currentTail, body);
                } else if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                } else if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                }
            } else if (down)
            {
                // Check where it can spawn
                if (checkUp(currentTail))
                {
                    spawnUp(currentTail, body);
                } else if (checkRight(currentTail))
                {
                    spawnRight(currentTail, body);
                } else if (checkLeft(currentTail))
                {
                    spawnLeft(currentTail, body);
                }
            }
        }
        body.transform.localScale = new Vector3(1, 1, 1);
    }

    private bool checkTailUp(GameObject currentTail, GameObject nextTail)
    {
        // Is end part of tail moving up?
        currentTailY = (float)System.Math.Round(currentTail.transform.localPosition.y, 1);
        nextTailY = (float)System.Math.Round(nextTail.transform.localPosition.y, 1);

        if (currentTailY + toMove == nextTailY)
        {
            return true;
        }
        return false;
    }

    private bool checkTailDown(GameObject currentTail, GameObject nextTail)
    {
        // Is end part of tail moving down?
        currentTailY = (float)System.Math.Round(currentTail.transform.localPosition.y, 1);
        nextTailY = (float)System.Math.Round(nextTail.transform.localPosition.y, 1);

        if (currentTailY - toMove == nextTailY)
        {
            return true;
        }
        return false;
    }

    private bool checkTailLeft(GameObject currentTail, GameObject nextTail)
    {
        // Is end part of tail moving left?
        currentTailX = (float)System.Math.Round(currentTail.transform.localPosition.x, 1);
        nextTailX = (float)System.Math.Round(nextTail.transform.localPosition.x, 1);

        if (currentTailX - toMove == nextTailX)
        {
            return true;
        }
        return false;
    }

    private bool checkTailRight(GameObject currentTail, GameObject nextTail)
    {
        // Is end part of tail moving right?
        currentTailX = (float)System.Math.Round(currentTail.transform.localPosition.x, 1);
        nextTailX = (float)System.Math.Round(nextTail.transform.localPosition.x, 1);

        if (currentTailX + toMove == nextTailX)
        {
            return true;
        }
        return false;
    }

    private bool checkUp(GameObject currentTail)
    {
        // Can the body be spawned above currentTail?
        if ((currentTail.transform.localPosition.y + toMove) > max_y)
        {
            return false;
        } else
        {
            return true;
        }
    }

    private bool checkDown(GameObject currentTail)
    {
        // Can the body be spawned below currentTail?
        if ((currentTail.transform.localPosition.y - toMove) < min_y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool checkLeft(GameObject currentTail)
    {
        // Can the body be spawned to the left of currentTail?
        if ((currentTail.transform.localPosition.x - toMove) < min_x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool checkRight(GameObject currentTail)
    {
        // Can the body be spawned to the right of currentTail?
        if ((currentTail.transform.localPosition.x + toMove) > max_x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Setter methods
    public void setLeft()
    {
        left = true;
        up = false;
        down = false;
        right = false;
    }

    public void setRight()
    {
        right = true;
        up = false;
        down = false;
        left = false;
    }

    public void setUp()
    {
        up = true;
        down = false;
        left = false;
        right = false;
    }

    public void setDown()
    {
        down = true;
        up = false;
        left = false;
        right = false;
    }

    // Getter methods
    public bool getLeft()
    {
        return left;
    }

    public bool getRight()
    {
        return right;
    }

    public bool getUp()
    {
        return up;
    }

    public bool getDown()
    {
        return down;
    }

    public int getLength()
    {
        return length;
    }

    public List<GameObject> getSnake()
    {
        return snake;
    }

    // Snake actions
    public void spawnUp(GameObject obj, GameObject obj2)
    {
        x = (float)System.Math.Round(obj.transform.localPosition.x, 1);
        y = (float)System.Math.Round(obj.transform.localPosition.y + toMove, 1);
        obj2.transform.localPosition = new Vector3(x, y, 0);
    }

    public void spawnDown(GameObject obj, GameObject obj2)
    {
        x = (float)System.Math.Round(obj.transform.localPosition.x, 1);
        y = (float)System.Math.Round(obj.transform.localPosition.y - toMove, 1);
        obj2.transform.localPosition = new Vector3(x, y, 0);
    }

    public void spawnLeft(GameObject obj, GameObject obj2)
    {
        x = (float)System.Math.Round(obj.transform.localPosition.x - toMove, 1);
        y = (float)System.Math.Round(obj.transform.localPosition.y, 1);
        obj2.transform.localPosition = new Vector3(x, y, 0);
    }

    public void spawnRight(GameObject obj, GameObject obj2)
    {
        x = (float)System.Math.Round(obj.transform.localPosition.x + toMove, 1);
        y = (float)System.Math.Round(obj.transform.localPosition.y, 1);
        obj2.transform.localPosition = new Vector3(x, y, 0);
    }

    public void moveHeadUp()
    {
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y + toMove, 1);
        this.gameObject.transform.localPosition = new Vector3(x, y, 0);
    }

    public void moveHeadDown()
    {
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y - toMove, 1);
        this.gameObject.transform.localPosition = new Vector3(x, y, 0);
    }

    public void moveHeadLeft()
    {
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x - toMove, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);
        this.gameObject.transform.localPosition = new Vector3(x, y, 0);
    }

    public void moveHeadRight()
    {
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x + toMove, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);
        this.gameObject.transform.localPosition = new Vector3(x, y, 0);
    }

    public void moveBody()
    {
        tempX = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        tempY = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        for (int i = 1; i <= length; i++)
        {
            currentBody = snake[i];
            nextPosX = tempX;
            nextPosY = tempY;
            tempX = (float)System.Math.Round(currentBody.transform.localPosition.x, 1);
            tempY = (float)System.Math.Round(currentBody.transform.localPosition.y, 1);

            currentBody.transform.localPosition = new Vector3(nextPosX, nextPosY, 0);
        }
    }

    public void checkSnake()
    {
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        for (int i = 1; i <= length; i++)
        {
            nextPosX = (float)System.Math.Round(snake[i].transform.localPosition.x, 1);
            nextPosY = (float)System.Math.Round(snake[i].transform.localPosition.y, 1);

            if (x == nextPosX && y == nextPosY)
            {
                gameLogic.setGameOver(true);
                break;
            }
        }
    }

    public bool checkDownHead()
    {
        // Is there a body below the head
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        if (length != 0)
        {
            nextPosX = (float)System.Math.Round(snake[1].transform.localPosition.x, 1);
            nextPosY = (float)System.Math.Round(snake[1].transform.localPosition.y, 1);

            if (((float)System.Math.Round(y - toMove, 1) == nextPosY) &&
                (x == nextPosX))
            {
                return true;
            } else
            {
                return false;
            }
        }
        return false;
    }

    public bool checkUpHead()
    {
        // Is there a body above the head
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        if (length != 0)
        {
            nextPosX = (float)System.Math.Round(snake[1].transform.localPosition.x, 1);
            nextPosY = (float)System.Math.Round(snake[1].transform.localPosition.y, 1);

            if (((float)System.Math.Round(y + toMove, 1) == nextPosY) &&
                (x == nextPosX))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool checkLeftHead()
    {
        // Is there a body to the left of the head
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        if (length != 0)
        {
            nextPosX = (float)System.Math.Round(snake[1].transform.localPosition.x, 1);
            nextPosY = (float)System.Math.Round(snake[1].transform.localPosition.y, 1);

            if (((float)System.Math.Round(x - toMove, 1) == nextPosX) &&
                (y == nextPosY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool checkRightHead()
    {
        // Is there a body to the right of the head
        x = (float)System.Math.Round(this.gameObject.transform.localPosition.x, 1);
        y = (float)System.Math.Round(this.gameObject.transform.localPosition.y, 1);

        if (length != 0)
        {
            nextPosX = (float)System.Math.Round(snake[1].transform.localPosition.x, 1);
            nextPosY = (float)System.Math.Round(snake[1].transform.localPosition.y, 1);

            if (((float)System.Math.Round(x + toMove, 1) == nextPosX) &&
                (y == nextPosY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}