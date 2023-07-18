
using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // this namespace makes the magic, tho
using UnityEngine.EventSystems;

public class Select_Square : MonoBehaviour
{
    public GameObject Tiles;
    public GameObject piece;
    public int x;
    public int y;
    public bool possibleMove;
    public bool isSelected;

    void Start()
    {
        GetComponent<Outline1>().enabled = false;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() )
        {
            if (ChessRules.current.isPlayerTurn)
            {

                if (!ChessRules.current.pieceIsMoving)
                {
                 
                    if (ChessRules.current.getIsSelected())
                    {
                        if (ChessRules.current.checkIfMoveValid(this.gameObject))
                            ChessRules.current.movePiece(this.gameObject);
                        else
                        {
                            ChessRules.current.clearPossibleMoves();
                            if (piece != null && piece.GetComponent<Piece>().isWhite)
                                ChessRules.current.pieceIsSelected(piece, x, y);
                        }
                    }
                    else if (piece != null && piece.GetComponent<Piece>().isWhite)
                    {
                        
                            
                            ChessRules.current.pieceIsSelected(piece, x, y);
                    }
                }
            }
        }
    }

    public void emptySquare()
    {

        piece = null;
    }

    private void Update()
    {

    }

}