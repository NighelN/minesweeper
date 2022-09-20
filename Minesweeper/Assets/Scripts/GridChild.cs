using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridChild : IGridChild
{
    //Met get set kun je de variabelen vullen uitlezen
    public GameObject game_object { get; set; }
    public bool isClicked { get; set; }
    public bool isMine { get; set; }
    public bool isFlag { get; set; }
    public int surroundingBombs { get; set; }

    public List<GridChild> surroundingCells = new List<GridChild>();

    public GridChild(GameObject child)
    {
        game_object = child;
        isClicked = false;
        isMine = false;
        isFlag = false;
        surroundingBombs = 0;
    }

    public void HandleLeftClick(Transform transform, int x, int y)
    {
        if(isFlag || Grid.instance.gameOver)
        {
            return;
        }

        isClicked = true;

        if(!Grid.instance.firstClick)
        {
            Grid.instance.SetupBombs(x, y);
            Grid.instance.firstClick = true;
        }

        if (isMine)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            Grid.instance.DisplayAllBombs();
            Grid.instance.gameOver = true;
        } else
        {
            transform.GetComponent<Image>().sprite = Grid.instance.clickedBrick;
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(3).GetComponent<Text>().text = surroundingBombs > 0 ? "" + surroundingBombs : "";
            if(surroundingBombs < 1)
                Grid.instance.CheckSurrounding(x, y);
        }
    }

    public void HandleRightClick(Transform transform)
    {
        if (Grid.instance.gameOver)
        {
            return;
        }

        if (Grid.instance.useableFlags == 0 && !Grid.instance.allowMinus)
        {
            //output no more flags
            Debug.Log("No more flags to use");
            return;
        }
        isFlag = !isFlag;
        transform.GetChild(2).gameObject.SetActive(isFlag);
        Grid.instance.useableFlags = isFlag ? Grid.instance.useableFlags - 1 : Grid.instance.useableFlags + 1;
    }
}
