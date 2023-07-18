using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiBattleBehavior : MonoBehaviour
{
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {

    }

    public void doStuff()
    {
        ChessRules.current.simulation = true;
        menu.SetActive(false);
    }
}
