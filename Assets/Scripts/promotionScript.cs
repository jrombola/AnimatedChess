using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class promotionScript : MonoBehaviour
{
    public GameObject promotionMenu;

    public GameObject template;
    public GameObject tile;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick()
    {
        tile = ChessRules.current.promotionTile;
        GameObject newPiece = Instantiate(template);
        if (tile.GetComponent<Select_Square>().piece != null)
        {
            tile.GetComponent<Select_Square>().piece.transform.position = new Vector3(-999, -99, -99);
            tile.GetComponent<Select_Square>().piece.SetActive(false);
            Destroy(tile.GetComponent<Select_Square>().piece);
        }
        tile.GetComponent<Select_Square>().piece = newPiece;
        newPiece.transform.position = tile.transform.position;
        newPiece.transform.rotation = Quaternion.Euler(0, 180, 0);
        newPiece.GetComponent<Piece>().originalRotation = newPiece.transform.rotation;
        newPiece.GetComponent<Piece>().hasMoved = true;
        newPiece.SetActive(true);
        newPiece.GetComponent<Piece>().target = newPiece.transform.position;
       // newPiece.GetComponent<Piece>().navmesh.SetDestination(tile.transform.position);
      //  ChessRules.current.selectedPiece = newPiece;
        Time.timeScale = 1;
        promotionMenu.SetActive(false);
       // ChessRules.current.pieceIsMoving = false;
    }
}
