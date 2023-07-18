using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessRules2 : MonoBehaviour
{
    //UI
    public GameObject checkmateMenu;
    public GameObject template;
    public GameObject promotionMenu;

    //difficulty settings
    public bool easyAi = false;
    public bool simulation = false;
    public bool aiStarted = false;

    int turn = 1;
    bool checkedPin = false;
    public bool checkIfCheck = false;

    //Singleton Pattern GameManager
    public static ChessRules2 current;

    private Select_Square[,] chessBoard;
    private List<GameObject> possibleMoves = new List<GameObject>();

    private Stack<GameObject> LastPiecePosition = new Stack<GameObject>();
    private Stack<GameObject> LastTilePosition = new Stack<GameObject>();
    public bool isSelected = false;
    public Piece selectedPiece;

    //current piece position on board;
    public int currentX;
    private int currentY;

    //checks players piece color 
    private bool isWhite = true;

    //checks if its player turn
    public bool isPlayerTurn;
    public bool win_state = false;

    //check bools
    public bool whiteInCheck = false;
    public bool blackInCheck = false;

    //bool for if piece is moving && is done moving
    public bool pieceIsMoving = false;
    public bool doneMoving = true;

    //bools to handle special moves
    public bool whiteRookAHasMoved = false;
    public bool whiteRookHHasMoved = false;
    public bool whiteKingHasMoved = false;
    public bool blackRookAHasMoved = false;
    public bool blackRookHHasMoved = false;
    public bool blackKingHasMoved = false;

    //References to the kings
    public GameObject whiteKing;
    public GameObject blackKing;

    //holds a list of pinned pices && attacking pieces
    public List<GameObject> pinnedPieces;
    public List<Select_Square> AttackingPiece;

    //used for promotions
    public GameObject promotionTile;
    private bool promoting;

    public bool isWhiteInCheck()
    {
        return whiteInCheck;
    }

    public bool isBlackInCheck()
    {
        return blackInCheck;
    }


    //getters ---------------------------------------------------------------


    public Piece getSelectedPiece()
    {
        return selectedPiece;
    }

    public GameObject getPossibleMoveAtIndexI(int i)
    {
        if (i < possibleMoves.Count)
        {
            return possibleMoves[i];
        }
        else
            return null;
    }
    public Select_Square getTileAtIJ(int i, int j)
    {
        return chessBoard[i, j];
    }
    public bool getIsWhite()
    {
        return isWhite;
    }
    public bool getPlayerTurn()
    {
        return isPlayerTurn;
    }
    public bool getIsSelected()
    {
        return isSelected;
    }

    public List<GameObject> getPossibleMoves()
    {
        return possibleMoves;
    }

    public Select_Square[,] getBoard()
    {
        return chessBoard;
    }

    //setters ----------------------------------------------------------

    public void setPiece(Piece piece, int x, int y)
    {
        selectedPiece = piece;
        currentX = x;
        currentY = y;
    }

    public void setPieceIsMovingTrue()
    {
        pieceIsMoving = true;
    }
    public void setPieceIsMovingFalse()
    {
        pieceIsMoving = false;
    }

    public void setBoard(Select_Square[,] board)
    {
        chessBoard = board;
    }
    public void setChessboard(Select_Square[,] tiles)
    {
        chessBoard = tiles;
    }

    public void setPlayerTurn(bool change)
    {
        isPlayerTurn = change;
    }

    public void setSelectedPiece(Piece selectedPieces)
    {
        selectedPiece = selectedPieces;
    }


    //gets piece + cooridnates and calls to genereates moves + effects
    public void pieceIsSelected(GameObject piece, int x, int y)
    {
        isSelected = true;
        selectedPiece = piece.GetComponent<Piece>();
        currentX = x;
        currentY = y;
        generatePossibleMoves(selectedPiece.gameObject);
        generateEffects();
    }

    //move generaters
    public void generatePossibleMoves(GameObject piece)
    {

        if (!whiteInCheck && !blackInCheck && !easyAi || isPlayerTurn && simulation)
        {
            if (!checkedPin)
            {
                checkForPin();
                checkedPin = true;
            }


            if (selectedPiece.pieceName == "Pawn")
                generatePawnMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Bishop")
                generateBishopsMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Rook")
                generateRookMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Knight")
                generateKnightMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Queen")
                generateQueenMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "King")
                generateKingMoves(currentY, currentX);

            if ((isPlayerTurn && isWhite) ^ (!isPlayerTurn && !isWhite))
            {
                if (selectedPiece.pieceName == "King")
                    checkIfMoveValidCheck(true);

            }
            else if ((isPlayerTurn && !isWhite) ^ (!isPlayerTurn && isWhite))
            {
                if (selectedPiece.pieceName == "King")
                    checkIfMoveValidCheck(false);
            }
        }
        else
        {
            pinnedPieces.Clear();
            AttackingPiece.Clear();


            if (selectedPiece.pieceName == "Pawn")
                generatePawnMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Bishop")
                generateBishopsMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Rook")
                generateRookMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Knight")
                generateKnightMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "Queen")
                generateQueenMoves(currentY, currentX);
            else if (selectedPiece.pieceName == "King")
                generateKingMoves(currentY, currentX);
            if ((isPlayerTurn && isWhite) ^ (!isPlayerTurn && !isWhite))
                checkIfMoveValidCheck(true);
            else if ((isPlayerTurn && !isWhite) ^ (!isPlayerTurn && isWhite))
                checkIfMoveValidCheck(false);
        }

    }

    private void generatePawnMoves(int y, int x)
    {
        int AttackX = -999;
        int AttackY = -999;
        int index = pinnedPieces.IndexOf(chessBoard[currentY, currentX].gameObject);

        if (index != -1)
        {
            AttackX = currentX - AttackingPiece[index].x;
            AttackY = currentY - AttackingPiece[index].y;
        }

        int[] Y = new int[3];
        if (isPlayerTurn)
        {
            Y[0] = 1;
            Y[1] = 1;
            Y[2] = 1;
        }
        else
        {
            Y[0] = -1;
            Y[1] = -1;
            Y[2] = -1;
        }

        int[] X = { 0, 1, -1 };



        if (index == -1 ^ (AttackX == 0))
        {
            if (y == 1 && isPlayerTurn)
            {
                if (chessBoard[y + 2, x].piece == null && chessBoard[y + 1, x].piece == null)
                {
                    possibleMoves.Add(chessBoard[y + 2, x].gameObject);
                    ;
                }
            }
        }
        if (index == -1 ^ (AttackY > 0 && AttackX != 0))
        {
            if (y == 6 && !isPlayerTurn)
            {
                if (chessBoard[y - 2, x].piece == null && chessBoard[y - 1, x].piece == null)
                {
                    possibleMoves.Add(chessBoard[y - 2, x].gameObject);

                }
            }
        }


        for (int i = 0; i < Y.Length; i++)
        {
            if (x + X[i] < 8 && y + Y[i] < 8 && x + X[i] > -1 && y + Y[i] > -1)
            {
                if (chessBoard[y + Y[i], x + X[i]].piece == null)
                {
                    if (X[i] == 0)
                    {
                        if (index == -1 ^ (AttackX == 0))
                            possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);

                    }
                }
                else if ((!chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                {

                    if (X[i] == 1 || X[i] == -1)
                    {
                        if (index == -1 || (X[i] == 1 && AttackX < 0) || (X[i] == -1 && AttackX > 0))
                            possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);
                    }

                }
            }

        }
    }

    private void generateKingMoves(int y, int x)
    {

        int[] Y = { 1, 1, 1, 0, 0, -1, -1, -1 };
        int[] X = { -1, 1, 0, -1, 1, -1, 1, 0 };


        for (int i = 0; i < Y.Length; i++)
        {

            if (x + X[i] < 8 && y + Y[i] < 8 && x + X[i] > -1 && y + Y[i] > -1)
            {
                if (chessBoard[y + Y[i], x + X[i]].piece == null)
                {

                    possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);
                }
                else if ((!chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                {
                    possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);
                }

            }

        }

        //castling
        if (isWhite && isPlayerTurn)
        {
            if (!whiteKingHasMoved)
            {
                if (!whiteRookHHasMoved)
                {
                    if (chessBoard[0, 5].piece == null && chessBoard[0, 6].piece == null)
                    {
                        possibleMoves.Add(chessBoard[0, 6].gameObject);
                    }
                }
                if (!whiteRookAHasMoved)
                {
                    if (chessBoard[0, 3].piece == null && chessBoard[0, 2].piece == null && chessBoard[0, 1].piece == null)
                    {
                        possibleMoves.Add(chessBoard[0, 2].gameObject);
                    }
                }
            }
        }
        else if (!isWhite && !isPlayerTurn)
        {
            if (!whiteKingHasMoved)
            {
                if (!blackRookHHasMoved)
                {
                    if (chessBoard[7, 5].piece == null && chessBoard[7, 6].piece == null)
                    {
                        possibleMoves.Add(chessBoard[7, 6].gameObject);
                    }
                }
                if (!blackRookAHasMoved)
                {
                    if (chessBoard[7, 3].piece == null && chessBoard[7, 2].piece == null && chessBoard[7, 1].piece == null)
                    {
                        possibleMoves.Add(chessBoard[7, 2].gameObject);
                    }
                }
            }
        }

    }

    private void generateQueenMoves(int y, int x)
    {
        int index = pinnedPieces.IndexOf(chessBoard[currentY, currentX].gameObject);
        if (index != -1)
        {
            int X = currentX - AttackingPiece[index].x;
            int Y = currentY - AttackingPiece[index].y;
            if ((Y == 0 && X != 0) || (X == 0 && Y != 0))
                generateRookMoves(y, x);
            else
                generateBishopsMoves(y, x);
        }
        else
        {
            generateBishopsMoves(y, x);
            generateRookMoves(y, x);
        }
    }
    private void generateKnightMoves(int y, int x)
    {
        if (pinnedPieces.Contains(chessBoard[currentY, currentX].gameObject))
        {
        }
        else
        {
            int[] X = { 2, 1, -1, -2, -2, -1, 1, 2 };
            int[] Y = { 1, 2, 2, 1, -1, -2, -2, -1 };
            for (int i = 0; i < X.Length; i++)
            {
                if (x + X[i] < 8 && y + Y[i] < 8 && x + X[i] > -1 && y + Y[i] > -1)
                    if (chessBoard[y + Y[i], x + X[i]].piece == null)
                    {
                        possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);
                    }
                    else if ((!chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + Y[i], x + X[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y + Y[i], x + X[i]].gameObject);
                    }
            }
        }
    }

    private void generateBishopsMoves(int y, int x)
    {
        int AttackX = 0;
        int AttackY = 0;
        int index = pinnedPieces.IndexOf(chessBoard[currentY, currentX].gameObject);

        if (index != -1)
        {
            AttackX = currentX - AttackingPiece[index].x;
            AttackY = currentY - AttackingPiece[index].y;
        }
        int[] XY = { 1, 2, 3, 4, 5, 6, 7 };
        if (index == -1 ^ ((AttackX > 0 && AttackY > 0) || (AttackX < 0 && AttackY < 0)))
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (y + XY[i] < 8 && x + XY[i] < 8)
                    if (chessBoard[y + XY[i], x + XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y + XY[i], x + XY[i]].gameObject);
                    else if ((chessBoard[y + XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y + XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y + XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y + XY[i], x + XY[i]].gameObject);
                        break;
                    }
            }
        }

        if (index == -1 ^ ((AttackX < 0 && AttackY > 0) || (AttackX > 0 && AttackY < 0)))
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (x + XY[i] < 8 && y - XY[i] > -1)
                    if (chessBoard[y - XY[i], x + XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y - XY[i], x + XY[i]].gameObject);
                    else if ((chessBoard[y - XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y - XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y - XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y - XY[i], x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y - XY[i], x + XY[i]].gameObject);
                        break;
                    }
            }
        }

        if (index == -1 ^ ((AttackX < 0 && AttackY < 0) || (AttackX > 0 && AttackY > 0)))
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (y - XY[i] > -1 && x - XY[i] > -1)
                    if (chessBoard[y - XY[i], x - XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y - XY[i], x - XY[i]].gameObject);
                    else if ((chessBoard[y - XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y - XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y - XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ !(!chessBoard[y - XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y - XY[i], x - XY[i]].gameObject);
                        break;
                    }
            }
        }

        if (index == -1 ^ ((AttackX > 0 && AttackY < 0) || (AttackX < 0 && AttackY > 0)))
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (x - XY[i] > -1 && y + XY[i] < 8)
                    if (chessBoard[y + XY[i], x - XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y + XY[i], x - XY[i]].gameObject);
                    else if ((chessBoard[y + XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y + XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y + XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + XY[i], x - XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y + XY[i], x - XY[i]].gameObject);
                        break;
                    }
            }
        }
    }

    private void generateRookMoves(int y, int x)
    {

        int AttackX = 0;
        int AttackY = 0;
        int index = pinnedPieces.IndexOf(chessBoard[currentY, currentX].gameObject);
        if (index != -1)
        {
            AttackX = currentX - AttackingPiece[index].x;
            AttackY = currentY - AttackingPiece[index].y;
        }


        int[] XY = { 1, 2, 3, 4, 5, 6, 7 };
        if (index == -1 ^ (AttackY > 0 && AttackX == 0) || (AttackY < 0 && AttackX == 0))
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (y + XY[i] < 8)
                    if (chessBoard[y + XY[i], x].piece == null)
                        possibleMoves.Add(chessBoard[y + XY[i], x].gameObject);
                    else if ((chessBoard[y + XY[i], x].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y + XY[i], x].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y + XY[i], x].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y + XY[i], x].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y + XY[i], x].gameObject);
                        break;
                    }
            }

            for (int i = 0; i < XY.Length; i++)
            {
                if (y - XY[i] > -1)
                {
                    if (chessBoard[y - XY[i], x].piece == null)
                        possibleMoves.Add(chessBoard[y - XY[i], x].gameObject);
                    else if ((chessBoard[y - XY[i], x].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y - XY[i], x].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y - XY[i], x].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y - XY[i], x].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y - XY[i], x].gameObject);
                        break;
                    }
                }
            }
        }

        if (index == -1 ^ (AttackY == 0 && AttackX > 0) || AttackY == 0 && AttackX < 0)
        {
            for (int i = 0; i < XY.Length; i++)
            {
                if (x + XY[i] < 8)
                    if (chessBoard[y, x + XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y, x + XY[i]].gameObject);
                    else if ((chessBoard[y, x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y, x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y, x + XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y, x + XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y, x + XY[i]].gameObject);
                        break;
                    }
            }



            for (int i = 0; i < XY.Length; i++)
            {
                if (x - XY[i] > -1)
                    if (chessBoard[y, x - XY[i]].piece == null)
                        possibleMoves.Add(chessBoard[y, x - XY[i]].gameObject);
                    else if ((chessBoard[y, x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (!chessBoard[y, x - XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                        break;
                    else if ((!chessBoard[y, x - XY[i]].piece.GetComponent<Piece>().isWhite && isPlayerTurn) ^ (chessBoard[y, x - XY[i]].piece.GetComponent<Piece>().isWhite && !isPlayerTurn))
                    {
                        possibleMoves.Add(chessBoard[y, x - XY[i]].gameObject);
                        break;
                    }
            }
        }
    }

    public List<Select_Square> listAllLivePieces(bool getAllpieces)
    {
        List<Select_Square> livePieces = new List<Select_Square>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[i, j].piece != null)
                {

                    if (getAllpieces)
                    {
                        livePieces.Add(chessBoard[i, j]);
                    }
                    else if (isWhite && !chessBoard[i, j].piece.GetComponent<Piece>().isWhite)
                        livePieces.Add(chessBoard[i, j]);
                    else if (!isWhite && chessBoard[i, j].piece.GetComponent<Piece>().isWhite)
                        livePieces.Add(chessBoard[i, j]);
                }
            }
        }
        return livePieces;
    }

    public List<Move> generateAllLegalMoves()
    {
        List<Select_Square> allPieces = listAllLivePieces(true);
        List<Move> legalMoves = new List<Move>();
        if ((isPlayerTurn && isWhite) ^ (!isPlayerTurn && !isWhite))
        {
            for (int i = 0; i < allPieces.Count; i++)
            {
                clearPossibleMoves();
                pieceIsSelected(allPieces[i].piece, allPieces[i].x, allPieces[i].y);
                for (int j = 0; j < possibleMoves.Count; j++)
                {
                    if (allPieces[i].GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
                    {
                        Move move = new Move();
                        move.addMove(allPieces[i], possibleMoves[j]);
                        legalMoves.Add(move);
                    }
                }
            }

        }
        else if ((isPlayerTurn && !isWhite) ^ (!isPlayerTurn && isWhite))
        {
            for (int i = 0; i < allPieces.Count; i++)
            {
                clearPossibleMoves();
                if (!allPieces[i].GetComponent<Select_Square>().piece.GetComponent<Piece>().isWhite)
                {

                    pieceIsSelected(allPieces[i].GetComponent<Select_Square>().piece, allPieces[i].x, allPieces[i].GetComponent<Select_Square>().y);
                    for (int j = 0; j < possibleMoves.Count; j++)
                    {

                        Move move = new Move();
                        move.addMove(allPieces[i], possibleMoves[j]);
                        legalMoves.Add(move);

                    }
                }
            }
        }
        clearPossibleMoves();
        return legalMoves;

    }



    public int computeRating()
    {
        List<Select_Square> LivePiecess = listAllLivePieces(true);
        int whiteScore = 0;
        int blackScore = 0;
        ChessHeuristic Table = GetComponent<ChessHeuristic>();
        AI2 AI = GetComponent<AI2>();

        for (int i = 0; i < LivePiecess.Count; i++)
        {
            if (LivePiecess[i].piece.GetComponent<Piece>().isWhite)
            {
                whiteScore += LivePiecess[i].piece.GetComponent<Piece>().getValue();
                whiteScore += Table.getValue(LivePiecess[i].gameObject);
                if (simulation)
                {
                    if (AI.lastWhiteMove.Count != 0)
                    {
                        if (AI.lastWhiteMove[0] == AI.currentWhiteMove && AI.lastWhiteMove[1] == AI.currentWhitePiece)
                        {
                            whiteScore -= 100;
                        }
                    }
                }
            }
            else if (!LivePiecess[i].piece.GetComponent<Piece>().isWhite)
            {
                blackScore += LivePiecess[i].piece.GetComponent<Piece>().getValue();
                blackScore += Table.getValue(LivePiecess[i].gameObject);
                if (simulation)
                {
                    if (AI.lastBlackMove.Count != 0)
                    {
                        if (AI.lastBlackMove[1] == AI.currentBlackPiece)
                        {
                            blackScore -= 100;
                        }
                    }
                }
            }

        }


        if ((isPlayerTurn && isWhite) ^ (!isPlayerTurn && !isWhite))
        {
            if (!simulation)
            {
                return blackScore - whiteScore;
            }
            else
            {
                if (AI.MAX_DEPTH == 2)
                {
                    return whiteScore - blackScore;
                }
                else
                {
                    return blackScore - whiteScore;
                }
            }
        }
        else if ((isPlayerTurn && !isWhite) ^ (!isPlayerTurn && isWhite))
        {
            if (!simulation)
                return whiteScore - blackScore;
            else
            {
                if (AI.MAX_DEPTH == 2)
                {
                    return blackScore - whiteScore;


                }
                else
                {
                    return whiteScore - blackScore;

                }

            }

        }

        return 0;

    }


    // highligh effects
    private void generateEffects()
    {
        for (int i = 0; i < getPossibleMoves().Count; i++)
        {
            if (possibleMoves[i] != null)
                possibleMoves[i].GetComponent<Outline1>().enabled = true;
        }
    }

    public void clearEffects()
    {
        for (int i = 0; i < getPossibleMoves().Count; i++)
        {
            if (possibleMoves[i] != null)
                possibleMoves[i].GetComponent<Outline1>().enabled = false;
        }
    }


    //checks if move is valid
    public bool checkIfMoveValid(GameObject Tile)
    {
        for (int i = 0; i < possibleMoves.Count; i++)
        {

            if (possibleMoves[i] == Tile)
            {
                return true;
            }
        }
        return false;
    }

    private void checkIfMoveValidCheck(bool Turn)
    {
        GameObject[,] chessBoardPrime = new GameObject[8, 8];
        List<GameObject> currentPossibleMoves = new List<GameObject>(possibleMoves);
        clearPossibleMoves();
        List<GameObject> validMoves = new List<GameObject>();


        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                chessBoardPrime[i, j] = ChessRules.current.getTileAtIJ(i, j).GetComponent<Select_Square>().piece;
            }
        }

        for (int j = 0; j < currentPossibleMoves.Count; j++)
        {
            updateChessboardMoveCheck(currentPossibleMoves[j].GetComponent<Select_Square>(), chessBoard);
            isPlayerTurn = !isPlayerTurn;
            if (!checkIfInCheck(Turn, chessBoard))
            {
                validMoves.Add(currentPossibleMoves[j]);
            }
            isPlayerTurn = !isPlayerTurn;
            resetTurn(chessBoardPrime);

        }
        resetTurn(chessBoardPrime);
        possibleMoves = validMoves;

    }

    private void checkForPin()
    {

        pinnedPieces.Clear();
        AttackingPiece.Clear();

        int x = -999;
        int y = -999;

        GameObject possiblePin = null;



        int[] X = { 1, -1, 1, 1, 0, 0, -1, -1 };
        int[] Y = { 0, 0, 1, -1, 1, -1, -1, 1 };




        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[i, j].piece != null)
                {

                    if (chessBoard[i, j].piece.GetComponent<Piece>().pieceName == "King" && chessBoard[i, j].piece.GetComponent<Piece>().isWhite && isPlayerTurn)
                    {
                        x = i;
                        y = j;

                        break;
                    }
                    else if (chessBoard[i, j].piece.GetComponent<Piece>().pieceName == "King" && !chessBoard[i, j].piece.GetComponent<Piece>().isWhite && !isPlayerTurn)
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
            }
            if (x != -999)
                break;
        }


        if (x == -999)
            return;
        if (isPlayerTurn)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    if ((x + X[i] * j) < 8 && (x + X[i] * j > -1) && (y + Y[i] * j) < 8 && (y + Y[i] * j > -1))
                    {


                        if (chessBoard[x + X[i] * j, y + Y[i] * j].piece != null)
                        {


                            if (possiblePin == null && chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {
                                possiblePin = chessBoard[x + X[i] * j, y + Y[i] * j].gameObject;
                            }
                            else if (possibleMoves != null && chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {
                                possiblePin = null;
                                break;
                            }
                            else if (possiblePin != null && !chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {
                                if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Rook" && (i == 0 || i == 1 || i == 4 || i == 5))
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);
                                    possiblePin = null;
                                    break;
                                }
                                else if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Bishop" && (i == 2 || i == 3 || i == 6 || i == 7))
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);
                                    possiblePin = null;
                                    break;

                                }
                                else if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Queen")
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);
                                    possiblePin = null;
                                    break;
                                }
                                else
                                {
                                    possiblePin = null;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        possiblePin = null;
                        break;
                    }
                }
            }
        }
        else if (!isPlayerTurn)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    if ((x + X[i] * j) < 8 && (x + X[i] * j > -1) && (y + Y[i] * j) < 8 && (y + Y[i] * j > -1))
                    {



                        if (chessBoard[x + X[i] * j, y + Y[i] * j].piece != null)
                        {

                            if (possiblePin == null && !chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {

                                possiblePin = chessBoard[x + X[i] * j, y + Y[i] * j].gameObject;
                            }
                            else if (possibleMoves != null && !chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {
                                possiblePin = null;
                                break;
                            }
                            else if (possiblePin != null && chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().isWhite)
                            {
                                if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Rook" && (i == 0 || i == 1 || i == 4 || i == 5))
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);

                                    possiblePin = null;
                                    break;
                                }
                                else if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Bishop" && (i == 2 || i == 3 || i == 6 || i == 7))
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);

                                    possiblePin = null;
                                    break;

                                }
                                else if (chessBoard[x + X[i] * j, y + Y[i] * j].piece.GetComponent<Piece>().pieceName == "Queen")
                                {
                                    pinnedPieces.Add(possiblePin);
                                    AttackingPiece.Add(chessBoard[x + X[i] * j, y + Y[i] * j]);

                                    possiblePin = null;
                                    break;
                                }
                                else
                                {
                                    possiblePin = null;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        possiblePin = null;
                        break;
                    }
                }
            }
        }

    }

    private void resetTurn(GameObject[,] chessBoardPrime)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessRules.current.getTileAtIJ(i, j).GetComponent<Select_Square>().piece = chessBoardPrime[i, j];
            }
        }
    }




    public void clearPossibleMoves()
    {
        clearEffects();
        possibleMoves.Clear();
    }

    public void movePiece(Select_Square Tile)
    {

        if (isPlayerTurn)
            whiteInCheck = false;
        else
            blackInCheck = false;
        StartCoroutine(checkMove(Tile.gameObject));


        if (selectedPiece != null)
        {


            if (selectedPiece.pieceName == "Rook")
            {
                if (selectedPiece.isWhite)
                {
                    if (chessBoard[0, 0].piece != null && chessBoard[0, 0].piece.Equals(selectedPiece))
                    {
                        whiteRookAHasMoved = true;
                    }
                    else if (chessBoard[0, 7].piece != null && chessBoard[0, 7].piece.Equals(selectedPiece))
                    {
                        whiteRookHHasMoved = true;
                    }

                }
                else
                {
                    if (chessBoard[7, 0].piece != null && chessBoard[7, 0].piece.Equals(selectedPiece))
                    {
                        blackRookAHasMoved = true;
                    }
                    else if (chessBoard[7, 7].piece != null && chessBoard[7, 7].piece.Equals(selectedPiece))
                    {
                        blackRookHHasMoved = true;
                    }
                }
            }

            if (selectedPiece.pieceName == "Pawn")
            {
                if (isPlayerTurn && Tile.y == 7)
                {

                    /* if (Tile.GetComponent<Select_Square>().piece != null)
                     {
                         selectedPiece.GetComponent<Piece>().wantsToAttack = true;
                         selectedPiece.GetComponent<Piece>().pieceToAttack = Tile.GetComponent<Select_Square>().piece.GetComponent<Piece>();
                         selectedPiece.GetComponent<Piece>().move(Tile);
                         pieceIsMoving = true;
                        // StartCoroutine(checkifPieceisMoving());
                     } else
                     {
                         selectedPiece.GetComponent<Piece>().move(Tile);
                         pieceIsMoving = true;
                        // StartCoroutine(checkifPieceisMoving());
                     }*/


                    promotionTile = Tile.gameObject;
                    if (Tile.piece != null)
                        Tile.piece.SetActive(false);
                    promotionMenu.SetActive(true);
                    Time.timeScale = 0;
                    promoting = true;
                    Debug.Log("continuing");
                    //instantiate selected piece

                    //generate promotion for player
                }
                else if (!isPlayerTurn && Tile.y == 0)
                {
                    GameObject newPiece = Instantiate(template);
                    if (Tile.piece != null)
                    {
                        selectedPiece.killPiece();
                        Tile.piece.transform.position = new Vector3(-999, -99, -99);
                        Tile.piece.SetActive(false);
                        Destroy(Tile.piece);
                    }
                    Tile.piece = newPiece;
                    newPiece.transform.position = Tile.transform.position;
                    newPiece.transform.rotation = Quaternion.Euler(0, -90, 0);
                    newPiece.GetComponent<Piece>().originalRotation = newPiece.transform.rotation;
                    newPiece.GetComponent<Piece>().hasMoved = true;
                    newPiece.SetActive(true);
                    newPiece.GetComponent<Piece>().target = newPiece.transform.position;
                }
            }




            if (selectedPiece.pieceName == "King")
            {

                //check for castle move
                if (!whiteKingHasMoved)
                {
                    if (Tile.y == 0 && Tile.x == 2)
                    {
                        chessBoard[0, 3].piece = chessBoard[0, 0].piece;
                        chessBoard[0, 0].piece.GetComponent<Piece>().move(chessBoard[0, 3].gameObject);
                        chessBoard[0, 0].piece = null;
                        pieceIsMoving = false;
                    }
                    else if (Tile.y == 0 && Tile.x == 6)
                    {

                        chessBoard[0, 5].piece = chessBoard[0, 7].piece;
                        chessBoard[0, 7].piece.GetComponent<Piece>().move(chessBoard[0, 5].gameObject);
                        chessBoard[0, 7].piece = null;
                        pieceIsMoving = false;
                    }
                }
                if (!blackKingHasMoved)
                {
                    if (Tile.y == 7 && Tile.x == 2)
                    {
                        chessBoard[7, 3].piece = chessBoard[7, 0].piece;
                        chessBoard[7, 0].piece.GetComponent<Piece>().move(chessBoard[7, 3].gameObject);
                        chessBoard[7, 0].piece = null;
                        pieceIsMoving = false;
                    }
                    else if (Tile.y == 7 && Tile.x == 6)
                    {
                        chessBoard[7, 5].piece = chessBoard[7, 7].piece;
                        chessBoard[7, 7].piece.GetComponent<Piece>().move(chessBoard[7, 5].gameObject);
                        chessBoard[7, 7].piece = null;
                        pieceIsMoving = false;
                    }
                }
                if (selectedPiece.isWhite)
                    whiteKingHasMoved = true;
                else
                    blackKingHasMoved = true;
            }
        }



        Debug.Log("updating chesboard");
        updateChessboard(Tile, chessBoard, false);
        clearPossibleMoves();


        if ((isPlayerTurn && isWhite))
        {
            if (isWhite)
            {
                blackInCheck = checkIfInCheck(false, chessBoard);
                if (blackInCheck)
                {

                    checkCheckmate();
                }
            }
            else
            {
                whiteInCheck = checkIfInCheck(true, chessBoard);
                if (whiteInCheck)
                {

                    checkCheckmate();
                }
            }
        }
        else
        {
            if (isWhite)
            {
                whiteInCheck = checkIfInCheck(true, chessBoard);
                if (whiteInCheck)
                {
                    checkCheckmate();
                }
            }
            else
            {
                blackInCheck = checkIfInCheck(false, chessBoard);
                if (blackInCheck)
                {
                    checkCheckmate();
                }
            }
        }
        isSelected = false;

        clearPossibleMoves();



        if (turn == 1)
        {
            isPlayerTurn = false;
            turn = 0;
        }
        else
        {
            isPlayerTurn = true;
            turn = 1;
        }

        if (!isPlayerTurn && !simulation)
        {
            if (easyAi == true)
                GetComponent<RandomAi>().pickRandomPiece();
            else
                StartCoroutine(GetComponent<Ai>().execute());

            return;
        }
        else if (simulation)
        {
            StartCoroutine(GetComponent<AI2>().execute());
        }
        //else if(isPlayerTurn)
        // GetComponent<Ai>().execute();
    }

    public void updateChessboard(Select_Square Tile, Select_Square[,] chessBoardPrime, bool aiIsCalculating)
    {
        if (Tile.piece != null && Tile.piece != selectedPiece && !aiIsCalculating)
        {
            selectedPiece.wantsToAttack = true;
            selectedPiece.pieceToAttack = Tile.piece.GetComponent<Piece>();
        }
        Tile.piece = selectedPiece.gameObject;
        chessBoardPrime[currentY, currentX].emptySquare();
        checkedPin = false;
        //LastPiecePosition.Push(chessBoard[currentY, currentX]);
        //LastTilePosition.Push(Tile);
    }

    private void updateChessboardMoveCheck(Select_Square Tile, Select_Square[,] chessBoardPrime)
    {
        Tile.piece = selectedPiece.gameObject;
        chessBoardPrime[currentY, currentX].emptySquare();
    }

    public void undoUpdateChessboard()
    {
        //if(LastTilePosition.Peek().GetComponent<Select_Square>().piece != null)
        //LastPiecePosition.Pop().GetComponent<Select_Square>().piece = LastTilePosition.Peek().GetComponent<Select_Square>().piece;
        //LastTilePosition.Pop().GetComponent<Select_Square>().piece = null;
    }


    public bool checkIfInCheck(bool white, Select_Square[,] chessBoardPrime)
    {
        if (true)
        {
            int whiteKingX = -999;
            int whiteKingY = -999;
            int blackKingX = 999;
            int blackKingY = -999;
            List<Piece> whitePieces = new List<Piece>();
            List<Piece> blackPieces = new List<Piece>();
            List<int> blackxCoords = new List<int>();
            List<int> blackyCoords = new List<int>();
            List<int> whitexCoords = new List<int>();
            List<int> whiteyCoords = new List<int>();
            int i = 0;
            int j = 0;
            while (i <= 7 && j <= 7)
            {
                if (chessBoardPrime[i, j].piece != null)
                {
                    Piece piece = chessBoardPrime[i, j].piece.GetComponent<Piece>();
                    if (piece != null)
                    {
                        if (piece.pieceName == "King")
                        {

                            if (piece.isWhite)
                            {
                                whiteKingX = i;
                                whiteKingY = j;
                            }
                            else if (!piece.isWhite)
                            {
                                blackKingX = i;
                                blackKingY = j;
                            }
                        }
                        else
                        {
                            if (piece.isWhite)
                            {
                                whitePieces.Add(piece);
                                int p = i;
                                int u = j;
                                whitexCoords.Add(p);
                                whiteyCoords.Add(u);

                            }
                            else
                            {
                                blackPieces.Add(piece);
                                blackxCoords.Add(i);
                                blackyCoords.Add(j);
                            }
                        }
                    }
                }
                j++;
                if (j >= 8 && i <= 7)
                {
                    i++;
                    j = 0;
                }
            }
            if (white)
            {

                for (int k = 0; k < blackPieces.Count; k++)
                {
                    if (blackPieces[k].pieceName == "Pawn")
                        generatePawnMoves(blackxCoords[k], blackyCoords[k]);
                    else if (blackPieces[k].pieceName == "Bishop")
                        generateBishopsMoves(blackxCoords[k], blackyCoords[k]);
                    else if (blackPieces[k].pieceName == "Rook")
                        generateRookMoves(blackxCoords[k], blackyCoords[k]);
                    else if (blackPieces[k].pieceName == "Knight")
                        generateKnightMoves(blackxCoords[k], blackyCoords[k]);
                    else if (blackPieces[k].pieceName == "Queen")
                        generateQueenMoves(blackxCoords[k], blackyCoords[k]);
                }

                if (possibleMoves.Contains(chessBoardPrime[whiteKingX, whiteKingY].gameObject))
                {

                    clearPossibleMoves();
                    return true;
                }
                else
                {
                    clearPossibleMoves();
                    return false;

                }
            }
            else
            {

                for (int k = 0; k < whitePieces.Count; k++)
                {
                    if (whitePieces[k].pieceName == "Pawn")
                        generatePawnMoves(whitexCoords[k], whiteyCoords[k]);
                    else if (whitePieces[k].pieceName == "Bishop")
                        generateBishopsMoves(whitexCoords[k], whiteyCoords[k]);
                    else if (whitePieces[k].pieceName == "Rook")
                        generateRookMoves(whitexCoords[k], whiteyCoords[k]);
                    else if (whitePieces[k].pieceName == "Knight")
                        generateKnightMoves(whitexCoords[k], whiteyCoords[k]);
                    else if (whitePieces[k].pieceName == "Queen")
                        generateQueenMoves(whitexCoords[k], whiteyCoords[k]);
                }

                if (possibleMoves.Contains(chessBoardPrime[blackKingX, blackKingY].gameObject))
                {

                    clearPossibleMoves();
                    return true;
                }
                else
                {
                    clearPossibleMoves();
                    return false;
                }
            }


        }

    }
    IEnumerator checkMove(GameObject Tile)
    {
        while (ChessRules.current.pieceIsMoving)
        {
            yield return null; // wait until next frame
        }
        selectedPiece.GetComponent<Piece>().move(Tile);
    }


    public void checkCheckmate()
    {

        checkIfCheck = true;
        isPlayerTurn = !isPlayerTurn;

        List<Select_Square> allPieces = new List<Select_Square>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[i, j].piece != null)
                    allPieces.Add(chessBoard[i, j]);
            }
        }
        clearPossibleMoves();
        if (blackInCheck)
        {
            for (int i = 0; i < allPieces.Count; i++)
            {
                if (!allPieces[i].piece.GetComponent<Piece>().isWhite)
                {
                    setPiece(allPieces[i].piece.GetComponent<Piece>(), allPieces[i].x, allPieces[i].y);
                    generatePossibleMoves(selectedPiece.gameObject);



                    if (possibleMoves.Count > 0)
                    {


                        checkIfCheck = false;
                        clearPossibleMoves();
                        break;
                    }

                }

            }
        }
        else
        if (whiteInCheck)
        {
            for (int i = 0; i < allPieces.Count; i++)
            {
                if (allPieces[i].piece.GetComponent<Piece>().isWhite)
                {
                    pieceIsSelected(allPieces[i].piece, allPieces[i].x, allPieces[i].y);
                    if (possibleMoves.Count > 0)
                    {
                        checkIfCheck = false;
                        clearPossibleMoves();
                        break;
                    }

                }

            }
        }

        isPlayerTurn = !isPlayerTurn;
        if (checkIfCheck)
        {

            win_state = true;
        }
    }


    void Start()
    {
        isPlayerTurn = isWhite;
        Application.targetFrameRate = 1000;
    }
    private void Awake()
    {
        current = this;

    }

    private void Update()
    {
        if (simulation && !aiStarted)
        {
            aiStarted = true;
            StartCoroutine(GetComponent<AI2>().execute());
            Time.timeScale = 4;

        }
    }

    IEnumerator checkifPieceisMoving()
    {
        doneMoving = false;
        yield return new WaitUntil(() => pieceIsMoving == false);
        doneMoving = true;
    }


}
