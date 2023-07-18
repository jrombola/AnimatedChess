using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easyButtonBehavior : MonoBehaviour
{
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doStuff()
    {
        ChessRules.current.easyAi = true;
        menu.SetActive(false);
    }
}
