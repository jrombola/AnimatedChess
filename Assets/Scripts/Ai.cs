using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Ai : MonoBehaviour
{

    int MAX_DEPTH = 3;
    int rating;
    private Move bestMove;
    int value = 0;


    public IEnumerator execute()
    {
        yield return StartCoroutine(minMax());
        StartCoroutine(move());

    }


    public IEnumerator minMax()
    {
        while (ChessRules.current.pieceIsMoving)
        {
            yield return null;
        }
        yield return StartCoroutine(maximizer(MAX_DEPTH, int.MinValue, int.MaxValue));
     
    }

    public IEnumerator maximizer(int depth, int alpha, int beta)
    {

        ChessRules.current.clearPossibleMoves();
        if (depth == 0)
        {
            
            rating = ChessRules.current.computeRating();
            yield break;
        }
        List<Move> legalMoves = ChessRules.current.generateAllLegalMoves();

        for (int i = 0; i < legalMoves.Count; i++)
        {
            GameObject[,] lastBoard = LastBoard();
            ChessRules.current.setPiece(legalMoves[i].getPiece().piece, legalMoves[i].getPiece().x, legalMoves[i].getPiece().y);
            ChessRules.current.updateChessboard(legalMoves[i].getTile(), ChessRules.current.getBoard(), true);
            ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
            value += 1 % 3;
            if (value == 1)
            {
                yield return StartCoroutine(minimizer(depth - 1, alpha, beta));
            }
            else
                StartCoroutine(minimizer(depth - 1, alpha, beta));

            ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
            RevertMove(lastBoard);


            if (rating > alpha)
            {
                alpha = rating;
                if (depth == MAX_DEPTH)
                {
                    bestMove = legalMoves[i];
                }
            }

            if (alpha >= beta)
            {
                rating = alpha;
                yield break;
            }

        }
        rating = alpha;
        yield break;

    }
    public IEnumerator minimizer(int depth, int alpha, int beta)
    {
        ChessRules.current.clearPossibleMoves();
        if (depth == 0)
        {
            
            rating = ChessRules.current.computeRating();
            yield break;
        }
        List<Move> legalMoves = ChessRules.current.generateAllLegalMoves();
        for (int i = 0; i < legalMoves.Count; i++)
        {
            GameObject[,] lastBoard = LastBoard();

            ChessRules.current.setPiece(legalMoves[i].getPiece().piece, legalMoves[i].getPiece().x, legalMoves[i].getPiece().y);
            ChessRules.current.updateChessboard(legalMoves[i].getTile(), ChessRules.current.getBoard(), true);
            ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
            value += 1 % 3;
            if (value == 1)
            {
                yield return StartCoroutine(maximizer(depth - 1, alpha, beta));
            }
            else StartCoroutine(maximizer(depth - 1, alpha, beta));

            ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());
            RevertMove(lastBoard);

            if (rating <= beta)
            {
                beta = rating;
            }

            if (alpha >= beta)
            {
                rating = beta;
                yield break;
            }

        }
        rating = beta;
        yield break;
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

    public IEnumerator move()
    {
        yield return null;
        ChessRules.current.setPiece(bestMove.getPiece().GetComponent<Select_Square>().piece, bestMove.getPiece().GetComponent<Select_Square>().x, bestMove.getPiece().GetComponent<Select_Square>().y);
        ChessRules.current.movePiece(bestMove.getTile());
        ChessRules.current.clearPossibleMoves();
    }


    private void Update()
    {
        if (!ChessRules.current.isPlayerTurn)
        {
            // StartCoroutine(wait()); 
        }


    }

    IEnumerator wait(Move legalMoves, int i)
    {
        yield return null;
        ChessRules.current.setPiece(legalMoves.getPiece().GetComponent<Select_Square>().piece, legalMoves.getPiece().GetComponent<Select_Square>().x, legalMoves.getPiece().GetComponent<Select_Square>().y);
        ChessRules.current.updateChessboard(legalMoves.getTile(), ChessRules.current.getBoard(), true);
        ChessRules.current.setPlayerTurn(!ChessRules.current.getPlayerTurn());

    }
}