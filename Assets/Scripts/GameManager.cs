using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool lastTakenWhite;
    private bool lastTakenBlack;
    private bool isWhiteTurn;
    private bool capturedWhite=false;
    private bool capturedBlack= false;
    private bool saveWhite = false;
    private bool saveBlack = false;
    private bool ended = false;
    [SerializeField] private Text text;
    [SerializeField] private GameObject endPanel;

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
        if (getSave())
        {
            saveBlack = false;
            saveWhite = false;
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
            capturedBlack= true;
        }
    }

    public bool getSave()
    {

        return (saveBlack || saveWhite);

    }

    public bool getSave(bool isWhite)
    {

        if (isWhite)
        {
            return saveWhite;
        }
        else
        {
            return saveBlack;
        }

    }

    public void setSave(bool isWhite)
    {
        if (isWhite)
        {
            saveWhite = true;
        }
        else
        {
            saveBlack = true;
        }
    }

    internal void EndGame(bool isWhite)
    {
        endPanel.SetActive(true);
        ended = true;
        if (isWhite)
        {
            text.text = "White wins";
        }
        else
        {
            text.text = "Black wins";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ended==false)
        {
            endPanel.SetActive(!endPanel.activeSelf);
        }
    }
}
