﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hardButtonBehavior : MonoBehaviour
{
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {

    }

    public void doStuff()
    {
        ChessRules.current.easyAi = false;
        menu.SetActive(false);
    }
}
