using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool lastTakenWhite;
    private bool lastTakenBlack;
    private bool isWhiteTurn;

    public bool getTaken(bool white)
    {
        if (white) return lastTakenWhite;
        return lastTakenBlack;
    }

    public void setTaken(bool white)
    {
        if (white)  lastTakenWhite=true;
        else lastTakenBlack=true;
    }

    public void resetTaken()
    {
        lastTakenBlack = false;
        lastTakenWhite = false;
    }

    public void ToogleTurn()
    {
        if (isWhiteTurn)
        {
            isWhiteTurn = false;
            return;
        }
        else
        {
            isWhiteTurn = true;
        }
    }

    public bool getWhiteTurn(bool isWhite)
    {
        return isWhiteTurn == isWhite;
    }

}
