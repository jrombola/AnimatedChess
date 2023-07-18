using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour
{
    public int MAX_DEPTH = 3;
    int rating;
    private Move bestMove;
    public List<GameObject> lastWhiteMove = new List<GameObject>();
    public List<GameObject> lastBlackMove = new List<GameObject>();
    public GameObject currentBlackMove;
    public GameObject currentBlackPiece;
    public GameObject currentWhiteMove;
    public GameObject currentWhitePiece;
    public IEnumerator execute()
    {
        while (ChessRules.current.pieceIsMoving)
        {
            yield return null; // wait until next frame
        }
        while (ChessRules.current.checkIfCheck)
        {
            yield return null; // wait until next frame
        }
            
        minimax(MAX_DEPTH, true, int.MinValue, int.MaxValue);
        move();
    }


    private int minimax(int depth, bool maximizingPlayer, int alpha, int beta)
    {
        if (depth == 0)
           return ChessRules.current.computeRating();

        int rating;
        if (maximizingPlayer)
        {
 
            foreach (Move m in ChessRules.current.generateAllLegalMoves())
            {
                if (ChessRules.current.isPlayerTurn)
                {
                    currentWhiteMove = m.getPiece().gameObject;
                    currentWhitePiece = (m.getPiece().GetComponent<Select_Square>().piece);
                }
                else
                {
                    currentBlackMove = m.getPiece().gameObject;
                    currentBlackPiece = (m.getPiece().GetComponent<Select_Square>().piece);
                }
                GameObject[,] lastBoard = LastBoard();
                ChessRules.current.setPiece(m.getPiece().GetComponent<Select_Square>().piece, m.getPiece().GetComponent<Select_Square>().x, m.getPiece().GetComponent<Select_Square>().y);
                ChessRules.current.updateChessboard(m.getTile(), ChessRules.current.getBoard(), true);
                ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
                rating = minimax(depth - 1, false, alpha, beta);
                ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
                RevertMove(lastBoard);
                if (rating > alpha)
                {
                    alpha = rating;
                    if (depth == MAX_DEPTH)
                    {
                        bestMove = m;
                    }
                }

                if (alpha >= beta)
                {
                    return alpha;  
                }
            }
            return alpha;
        }
        else
        {

            foreach(Move m in ChessRules.current.generateAllLegalMoves())
            {
                if (ChessRules.current.isPlayerTurn)
                {
                    currentWhiteMove = m.getPiece().gameObject;
                    currentWhitePiece = (m.getPiece().GetComponent<Select_Square>().piece);
                }
                else
                {
                    currentBlackMove = m.getPiece().gameObject;
                    currentBlackPiece = (m.getPiece().GetComponent<Select_Square>().piece);
                }
                GameObject[,] lastBoard = LastBoard();
                ChessRules.current.setPiece(m.getPiece().GetComponent<Select_Square>().piece, m.getPiece().GetComponent<Select_Square>().x, m.getPiece().GetComponent<Select_Square>().y);
                ChessRules.current.updateChessboard(m.getTile(), ChessRules.current.getBoard(), true);
                ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
                rating = minimax(depth - 1, true, alpha, beta);
                ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
                RevertMove(lastBoard);
                if(rating <= beta)
                {
                    beta = rating;
                }

                if (alpha >= beta)
                    return beta;
            }
            return beta;
        }
    }

    

    public GameObject[,] LastBoard()
    {
        GameObject[,] lastBoard = new GameObject[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                lastBoard[i, j] = ChessRules.current.getBoard()[i, j].GetComponent<Select_Square>().piece;
            }
        }
        return lastBoard;
    }


    public void RevertMove(GameObject[,] lastBoard)
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessRules.current.getBoard()[i, j].GetComponent<Select_Square>().piece = lastBoard[i, j];
            }
        }
    }


    public void move()
    {
        if (ChessRules.current.isPlayerTurn)
        {
            lastWhiteMove.Clear();
            lastWhiteMove.Add(bestMove.getPiece().gameObject);
            lastWhiteMove.Add(bestMove.getPiece().GetComponent<Select_Square>().piece);

        }
        else
        {
            lastBlackMove.Clear();
            lastBlackMove.Add(bestMove.getPiece().gameObject);
            lastBlackMove.Add(bestMove.getPiece().GetComponent<Select_Square>().piece);
        }


        if (bestMove.getPiece().GetComponent<Select_Square>().piece == null)
            minimax(MAX_DEPTH, true, int.MinValue, int.MaxValue);
        ChessRules.current.setPiece(bestMove.getPiece().GetComponent<Select_Square>().piece, bestMove.getPiece().GetComponent<Select_Square>().x, bestMove.getPiece().GetComponent<Select_Square>().y);
        //Debug.Log(bestMove.getTile().name);
        ChessRules.current.movePiece(bestMove.getTile());
    } 
}
