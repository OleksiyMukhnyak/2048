using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Level 
{
    public int number;
    public string imageName;
    public int height;
    public int weight;
    public ItemNumber needToWin;
    public float squareSize;
    public string lvlDescrip;
    public Sprite lvlImage;

    public Level(int number = 0, string imageName = "Tiny", int height = 3, int weight = 3, ItemNumber needToWin = ItemNumber._2048, float squareSize = 1.0f, string lvlDescrip = "Tiny(3x3)")
    {
        this.number = number;
        this.imageName = imageName;
        this.height = height;
        this.weight = weight;
        this.needToWin = needToWin;
        this.squareSize = squareSize;
        this.lvlDescrip = lvlDescrip;
        this.lvlImage = null;
    }
}
