using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool lastTakenWhite;
    private bool lastTakenBlack;
    private bool isWhiteTurn;
    private bool capturedWhite=false;
    private bool capturedBlack= false;

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
        if (getCaptured())
        {
            capturedBlack = false;
            capturedWhite = false;
            return;
        }
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

    public bool getCaptured()
    {

        return (capturedBlack || capturedWhite);

    }

    public bool getCaptured(bool isWhite)
    {

        if (isWhite)
        {
            return capturedWhite;
        }
        else
        {
            return capturedBlack;
        }

    }

    public void setCaptured(bool isWhite)
    {
        if (isWhite)
        {
            capturedWhite = true;
        }
        else
        {
            capturedBlack= false;
        }
    }

}
