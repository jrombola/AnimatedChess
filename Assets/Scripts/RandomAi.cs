using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class RandomAi : MonoBehaviour
{
    List<GameObject> listOfPieces = new List<GameObject>();
    List<GameObject> PossibleMoves = new List<GameObject>();
    GameObject selectedPiece;
    GameObject[,] chessBoardPrime = new GameObject[8, 8];



    public void pickRandomPiece()
    {
        ChessRules.current.clearPossibleMoves();
        int index;
        getPieces();
        pickPiece();

        //StartCoroutine(ChessRules.current.checkCheckmate());
        if (!ChessRules.current.win_state)
        {
                while (ChessRules.current.getPossibleMoves().Count == 0)
                {
                    for (int i = 0; i < listOfPieces.Count; i++)
                    {
                        if (listOfPieces[i] == selectedPiece)
                            listOfPieces.RemoveAt(i);
                    }  
                    if (listOfPieces.Count == 0)
                        break;
                    pickPiece();
                }
        }
        index = UnityEngine.Random.Range(0, (ChessRules.current.getPossibleMoves().Count - 1));
        ChessRules.current.clearEffects();
       StartCoroutine(Move(ChessRules.current.getPossibleMoves()[index]));
        emptyVariables();
    }


    private IEnumerator Move(GameObject selectedMove)
    {
        while (ChessRules.current.pieceIsMoving)
        {
            yield return null; // wait until next frame 
        }
        ChessRules.current.movePiece(selectedMove);

    }

    private void getPieces()
    {
        List<GameObject> allpieces = ChessRules.current.listAllLivePieces(true);
        for(int i = 0; i < allpieces.Count; i++)
        {
            if (!allpieces[i].GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
            {
                listOfPieces.Add(allpieces[i]);
            }
        }
    }

    private void pickPiece()
    {
        int index;
        index = UnityEngine.Random.Range(0, (listOfPieces.Count - 1));
        selectedPiece = listOfPieces[index];
        ChessRules.current.pieceIsSelected(selectedPiece.GetComponent<Select_Square>().piece, selectedPiece.GetComponent<Select_Square>().x, selectedPiece.GetComponent<Select_Square>().y);
    }
    private void emptyVariables()
    {
        listOfPieces.Clear();
        PossibleMoves.Clear();
        selectedPiece = null;
    }





}
