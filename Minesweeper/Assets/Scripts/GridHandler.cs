using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//serialzefield publiceert private fields in unity
public abstract class GridHandler : MonoBehaviour
{
    [SerializeField]
    GameObject button_prefab;
    [SerializeField]
    RectTransform screenUI;
    [SerializeField]
    RectTransform deviderUI;
    [SerializeField]
    Image resetSprite;
    [SerializeField]
    Sprite regularSmiley;
    [SerializeField]
    Sprite deadSmiley;
    [SerializeField]
    public Sprite clickedBrick;
    [SerializeField]
    Sprite correctFlag;
    [SerializeField]
    Text flags;
    [SerializeField]
    Text time;

    public GridChild[,] grid;
    int totalChildren { get; set; }
    int width { get; set; }
    int height { get; set; }
    int totalBombs { get; set; }
    public int useableFlags { get; set; }
    public bool allowMinus { get; set; }
    public float startTime { get; set; }
    public bool gameOver { get; set; }
    public bool firstClick { get; set; }

    public abstract void Awake();

    /// <summary>
    /// Updates the timer and how many flags you have left
    /// </summary>
    public virtual void Update()
    {
        if(!gameOver)
            //hij pakt de tijd van hoelang de programma is begonnen en van de tijd dat er echt word gespeeld.
            //typecasting, hij veranderd een float naar een int.
            time.text = "" + (int)(Time.time - startTime);
        // hoeveel vlaggen je nog kan pakken
        flags.text = "" + useableFlags;
        Quit();
    }

    /// <summary>
    /// Handles closing the application when the escape key is pressed
    /// </summary>
    public void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public abstract void Setup(int width, int height, int totalBombs);

    /// <summary>
    /// Sets up the game
    /// </summary>
    /// <param name="width">The width of the grid</param>
    /// <param name="height">The height of the grid</param>
    /// <param name="totalBombs">How many bombs there are on the grid</param>
    public virtual void SetupGame(int width, int height, int totalBombs)
    {
        //this refrences naar een specifieke object van waar je het opgezet.
        startTime = Time.time;
        this.width = width;
        this.height = height;
        this.totalBombs = totalBombs;
        useableFlags = totalBombs;
        allowMinus = false;
    }

    /// <summary>
    /// Handles setting up the adjustable UI
    /// </summary>
    public virtual void SetupUI()
    {
        //de grid schalen.
        totalChildren = width * height;
        // het is een array van grid child.
        grid = new GridChild[width, height];
        // de echte breedte/hoogte dat het veld/grid moet zijn.
        int realWidth = width * 32;
        int realHeight = height * 32;
        //hij pakt de groote van de top ui en dat voegt hij dan doe aan de grid.
        int totalHeight = (91 + realHeight);
        //hij pakt de component van het object
        RectTransform rectTrans = GetComponent<RectTransform>();
        // de groote van de grid
        rectTrans.sizeDelta = new Vector2(realWidth, realHeight);
        // de breedte van de grid aanpassen
        deviderUI.sizeDelta = new Vector2(realWidth, 16.04999f);
        //pakt de breedte van je scherm en haalt de groote van je grid eraf en deelt het daarna door 2 omdat je evenveel van de linker als rechter kant afhaalt.
        int removeSide = (1920 - realWidth) / 2;
        int removeTop = ((1080 - totalHeight) / 2);
        //hij zet de ui op de goede positie
        screenUI.offsetMin = new Vector2(removeSide, removeTop);
        screenUI.offsetMax = new Vector2(-removeSide, -removeTop);
        //door gebruik van blokjes verschilt het op hoe groot het word gescaled (zodat het er bijv. groter uitziet.
        screenUI.localScale = totalChildren >= 100 && totalChildren <= 256 ? new Vector3(1.5f, 1.5f, 1.5f) : totalChildren < 100 ? new Vector3(2, 2, 2) : new Vector3(1, 1, 1);
        // hij pakt de component van het object en zorgt dat het gelijk staat aan de width
        GetComponent<GridLayoutGroup>().constraintCount = width;
        //hoeveel vlaggen je hebt/hoeveel bommen er zijn
        flags.text = "" + totalBombs;
    }

    /// <summary>
    /// Resets the field
    /// </summary>
    public virtual void ResetField()
    {
        ChangeSmiley(regularSmiley);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        firstClick = false;
        gameOver = false;
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public virtual void Reset()
    {
        ResetField();
        grid = new GridChild[width, height];
        SetupGame(width, height, totalBombs);
        SetupGrid();
    }

    /// <summary>
    /// Setup the grid with all the grid childs
    /// </summary>
    public virtual void SetupGrid()
    {   //als y lager is dan de hoogte komt er één bij
        for (int y = 0; y < height; y++)
        {   //als x lager is dan de breedte komt er één bij
            for (int x = 0; x < width; x++)
            {
                GameObject gridChild = Instantiate(button_prefab) as GameObject;
                gridChild.transform.SetParent(transform);
                gridChild.name = "GridChild:" + x + ":" + y;
                gridChild.transform.localScale = new Vector3(1, 1, 1);
                grid[x, y] = new GridChild(gridChild);
            }
        }
    }

    /// <summary>
    /// Handles setuping up the bomb and make sure you dont place a bomb on the first x/y you clicked
    /// </summary>
    /// <param name="clickedX">The x coordinate you clicked</param>
    /// <param name="clickedY">The y coordinate you clicked</param>
    public virtual void SetupBombs(int clickedX, int clickedY)
    {

        bool bombCheating = false;
        int placedBombs = 0;
        do
        {   //randomX staat gelijk aan tot een random range tussen 0 en de breedte, randomY staat gelijk aan een random range tussen 0 en de hoogte
            int randomX = UnityEngine.Random.Range(0, width), randomY = UnityEngine.Random.Range(0, height);
            //als de randomX gelijk is aan clickedX of als randomY gelijk is aan clickedY kun je verder.
            if (randomX == clickedX && randomY == clickedY) continue;
            //plaatst random bommen
            GridChild child = grid[randomX, randomY];

            if (child.isMine) continue;

            child.isMine = true;

            UpdateNumbers(randomX, randomY, 1);
            //dit is om te testen om te kijken waar alle bommen zitten
            if(bombCheating) child.game_object.transform.GetChild(1).gameObject.SetActive(true);
            //plaatst steeds meer bommen/telt steeds bommen bij op
            placedBombs++;
        } while (placedBombs < totalBombs);
    }

    public virtual void DisplayAllBombs()
    {
        ChangeSmiley(deadSmiley);
        //als x lager is dan de breedte komt er één bij
        for (int x = 0; x < width; x++)
        {
            // als y lager is dan de hoogte komt er één bij
            for (int y = 0; y < height; y++)
            {
                
                GridChild child = grid[x, y];
                if (!child.isMine) continue;

                if(child.isFlag)
                {
                    
                    child.game_object.transform.GetChild(2).GetComponent<Image>().sprite = correctFlag;
                }
                //normale blokjes veranderen naar een andere sprite nadat je erop hebt geklikt.
                child.game_object.transform.GetComponent<Image>().sprite = clickedBrick;
                child.game_object.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Changes the smiley sprite using the parameter
    /// </summary>
    /// <param name="sprite">The happy or dead smiley</param>
    public virtual void ChangeSmiley(Sprite sprite)
    {
        resetSprite.sprite = sprite;
    }

    /// <summary>
    /// Updates all the numbers for grid childs that are around a bomb
    /// </summary>
    /// <param name="currentX">The x coordinate of a bomb</param>
    /// <param name="currentY">The y coordinate of a bomb</param>
    /// <param name="radius">The radius of the checking</param>
    public void UpdateNumbers(int currentX, int currentY, int radius = 1)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                int newX = currentX + x;
                int newY = currentY + y;
                if (newX < 0 || newX >= width || newY < 0 || newY >= height) continue;

                GridChild child = grid[newX, newY];
                child.surroundingBombs++;
            }
        }
    }

    List<GridChild> checkedGrids = new List<GridChild>();

    /// <summary>
    /// Checks the surrounding for empty childs
    /// </summary>
    /// <param name="x">The x coordinate you clicked</param>
    /// <param name="y">TThe y coordinate you clicked</param>
    public void CheckSurrounding(int x, int y)
    {
        checkedGrids.Clear();
        CheckSurroundingRecursive(x, y);

        foreach (GridChild child in checkedGrids)
        {
            child.game_object.transform.GetComponent<Image>().sprite = Grid.instance.clickedBrick;
            child.game_object.transform.GetChild(3).gameObject.SetActive(true);
            child.game_object.transform.GetChild(3).GetComponent<Text>().text = child.surroundingBombs > 0 ? "" + child.surroundingBombs : "";
        }
    }

    /// <summary>
    /// Handles the recursion for checking the surroundings
    /// </summary>
    /// <param name="currentX">The x coordinate of a child</param>
    /// <param name="currentY">The y coordinate of a child</param>
    /// <param name="radius">The radius of the checking</param>
    public void CheckSurroundingRecursive(int currentX, int currentY, int radius = 1)
    {
        try
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int newX = currentX + x;
                    int newY = currentY + y;
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height) continue;

                    GridChild child = grid[newX, newY];

                    if (checkedGrids.Contains(child)) continue;

                    checkedGrids.Add(child);
                    if (child.surroundingBombs < 1)
                    {
                        CheckSurroundingRecursive(newX, newY);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log(String.Format("IOException source: {0}", e.Source));
        }
    }
}