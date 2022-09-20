using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler instance;

    [SerializeField]
    InputField Width;
    [SerializeField]
    InputField Heigth;
    [SerializeField]
    InputField Bombs;
    [SerializeField]
    public GameObject lobby;
    [SerializeField]
    public GameObject game;

    public Data levelData;
    public int outputWidth;
    public int outputHeight;
    public int outputBombs;

    public void Awake()
    {
        Load();
        //Sets all the input fields with the loaded date
        Width.text = levelData.width.ToString();
        Heigth.text = levelData.height.ToString();
        Bombs.text = levelData.bombs.ToString();

        instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void HandleOptions(int options)
    {
        switch (options)
        {
            case 0:
                StartGame(9, 9, 10);
                break;
            case 1:
                StartGame(16, 16, 40);
                break;
            case 2:
                StartGame(30, 16, 99);
                break;
            case 3:
                //Tries to parse all the input fields to int
                if(int.TryParse(Width.text, out outputWidth) && int.TryParse(Heigth.text, out outputHeight) && int.TryParse(Bombs.text, out outputBombs))
                {
                    //Checks if the width/height are within the min and max
                    if(outputWidth >= 9 && outputWidth <= 60 && outputHeight >= 9 && outputHeight <= 30)
                    {
                        //Grabs the total bombs based on width * height -1
                        int totalBombs = outputWidth * outputHeight - 1;
                        //Checks if the bombs filled in are within range
                        if (outputBombs >= 1 && outputBombs <= totalBombs)
                        {
                            //Starts the game
                            StartGame(outputWidth, outputHeight, outputBombs);
                            //Save the width/height and bombs
                            Save();
                        }
                    }
                    
                }
                break;
        }
    }

    public void StartGame(int width, int height, int bombs)
    {
        lobby.SetActive(false);
        game.SetActive(true);
        Grid.instance.Setup(width, height, bombs);
    }
    
    public struct Data
    {
        public int width;
        public int height;
        public int bombs;

        public Data(int width, int height, int bombs)
        {
            this.width = width;
            this.height = height;
            this.bombs = bombs;
        }
    }

    public void Load()
    {
        try
        {
            //Reads out the save file
            using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Save.txt"))
            {
                string[] outcome = sr.ReadToEnd().Split(':');
                //Converts the read data to a struct data using the width/height and 
                levelData = new Data(int.Parse(outcome[0]), int.Parse(outcome[1]), int.Parse(outcome[2]));
                sr.Close();
            }
        }
        catch (IOException e)
        {
            Debug.Log(String.Format("IOException source: {0}", e.Source));
        }
    }

    public void Save()
    {
        try
        {
            //The path of the save file
            string file = Application.dataPath + "/Resources/Save.txt";
            //Deletes the save file
            File.Delete(file);
            //Writes the new data to the save file
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(outputWidth + ":" + outputHeight + ":" + outputBombs);
                sw.Close();
            }
        }
        catch (IOException e)
        {
            Debug.Log(String.Format("IOException source: {0}", e.Source));
        }
    }
}
