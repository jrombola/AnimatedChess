using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessHeuristic : MonoBehaviour
{
    private void Start()
    {
        
    }
    private  int[,] bPawnBoard = new int[,]
	  {
		{ 0,   0,   0,   0,   0,   0,   0,   0 },
		{ 5,  10,  15,  20,  20,  15,  10,   5 },
		{ 4,   8,  12,  16,  16,  12,   8,   4 },
		{ 3,   6,   9,  12,  12,   9,   6,   3 },
		{ 2,   4,   6,   8,   8,   6,   4,   2 },
		{ 1,   2,   3, -10, -10,   3,   2,   1 },
		{ 0,   0,   0, -40, -40,   0,   0,   0 },
		{ 0,   0,   0,   0,   0,   0,   0,   0 }
	};
	private  int[,] bKnighBoard = new int[,]
	{
		{ -10, -10, -10, -10, -10, -10, -10, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10, -30, -10, -10, -10, -10, -30, -10 }
	};
	private  int[,] bBishopBoard = new int[,]
	{
		{ -10, -10, -10, -10, -10, -10, -10, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10, -10, -20, -10, -10, -20, -10, -10 }
	};
	private int[,] bKingBoard = new int[,]
	{
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -20, -20, -20, -20, -20, -20, -20, -20 },
		{  0,  20,  40, -20,   0, -20,  40,  20  }
		};

	private int[,] wPawnBoard = new int[,]
	  {
		{ 0,  0,  0,   0,   0,  0,  0,  0 },
		{ 0,  0,  0, -40, -40,  0,  0,  0 },
		{ 1,  2,  3, -10, -10,  3,  2,  1 },
		{ 2,  4,  6,   8,   8,  6,  4,  2 },
		{ 3,  6,  9,  12,  12,  9,  6,  3 },
		{ 4,  8, 12,  16,  16, 12,  8,  4 },
		{ 5, 10, 15,  20,  20, 15, 10,  5 },
		{ 0,  0,  0,   0,   0,  0,  0,  0 }
	};

	private int[,] wKnighBoard = new int[,]
	{
		{ -10, -30, -10, -10, -10, -10, -30, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,  10,  10,   5,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10, -10, -10, -10, -10, -10, -10, -10 }
	};

	private  int[,] wBishopBoard = new int[,]
	{
		{ -10, -10, -20, -10, -10, -20, -10, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   5,   10,  10,  5,   0, -10 },
		{ -10,   0,   5,   10,  10,  5,   0, -10 },
		{ -10,   0,   5,   5,   5,   5,   0, -10 },
		{ -10,   0,   0,   0,   0,   0,   0, -10 },
		{ -10, -10, -10, -10, -10, -10, -10, -10 }
	};

	private  int[,] wKingBoard = new int[,]
	{
	    { 20,  40, -20,   0, -20,  40,  20,    0 },
	    { -20, -20, -20, -20, -20, -20, -20, -20 },
	    { -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 },
		{ -40, -40, -40, -40, -40, -40, -40, -40 }
		};

    public int getValue(GameObject Piece)
    {
		if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "Pawn") {
			if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return wPawnBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
			else if (!Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return bPawnBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
		}
		else if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "Bishop") {
			if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return wBishopBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
			else if (!Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return bBishopBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
		}
		else if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "Rook")
        {
			//not implemented yet
			return 0;
		}


		else if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "Knight")
        {
			if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return wKnighBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
			else if (!Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return bKnighBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
		}


		else if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "Queen")
        {
			//nothing particular
			return 0;
		}

		else if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().pieceName == "King")
        {
			if (Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return wKingBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
			else if (!Piece.GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
				return bKingBoard[Piece.GetComponent<Select_Square>().y, Piece.GetComponent<Select_Square>().x];
		}
		return 0;

	}

}
