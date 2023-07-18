using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move 
{
    private Select_Square movePiece;
    private GameObject tile;

    public void addMove(Select_Square Piece, GameObject Tile)
    {

        movePiece = Piece;
        tile = Tile;
    }

    public Select_Square getPiece()
    {
        return movePiece;
    }
    
    public GameObject getTile()
    {
        return tile;
    }

}
