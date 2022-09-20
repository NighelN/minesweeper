using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : GridHandler
{
    //static grid instance is een singleton, een class die 1 instance van zichzelf maakt/creeërt.
    public static Grid instance;
    
    /// <summary>
    /// Sets up the game
    /// </summary>
    /// <param name="width">The width of the grid</param>
    /// <param name="height">The height of the grid</param>
    /// <param name="totalBombs">The total of bombs on the grid</param>
    public override void Setup(int width, int height, int totalBombs)
    {
        //SetUpGame ( width, height, totalBombs)
        SetupGame(width, height, totalBombs);
        SetupUI();
        SetupGrid();
    }

    public override void Awake()
    {
        instance = this;
    }

}
