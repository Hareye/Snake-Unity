using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SnakeHead
{
    void prepareMove();
    void checkMove();
    void moveSnake();
    void moveBody(float x, float y, int idx);
    void updateBoard();

    bool checkInvalidMovement(string movement);

    void setDirection(string dir);
    string getDirection();
}

public interface SnakeBody
{
    void newBody();
    void posNewBody();
    void initPosNewBody();
    void updateBoard();
}