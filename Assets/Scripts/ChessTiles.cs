using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTiles : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject[] Row1;
    public GameObject[] Row2;
    public GameObject[] Row3;
    public GameObject[] Row4;
    public GameObject[] Row5;
    public GameObject[] Row6;
    public GameObject[] Row7;
    public GameObject[] Row8;

    public GameObject[] whiteRow1;
    public GameObject[] whiteRow2;

    public GameObject[] blackRow1;
    public GameObject[] blackRow2;



    public GameObject[,] emptyBoard = new GameObject[8, 8];
    public GameObject[,] fullBoard;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (i == 0)
                    emptyBoard[i, j] = Row1[j];
                else if (i == 1)
                    emptyBoard[i, j] = Row2[j];
                else if (i == 2)
                    emptyBoard[i, j] = Row3[j];
                else if (i == 3)
                    emptyBoard[i, j] = Row4[j];
                else if (i == 4)
                    emptyBoard[i, j] = Row5[j];
                else if (i == 5)
                    emptyBoard[i, j] = Row6[j];
                else if (i == 6)
                    emptyBoard[i, j] = Row7[j];
                else if (i == 7)
                    emptyBoard[i, j] = Row8[j];

                //Debug.Log(emptyBoard[i,j].name);
            }
        }
        ChessRules.current.setChessboard(emptyBoard);
    }


}
