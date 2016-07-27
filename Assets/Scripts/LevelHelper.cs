using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelHelper : MonoBehaviour
{

    public Text DescriptionLvl;
    public Image ImageLvl;
    public Level CurrentLvl { get { return _currentLvl; } }

    private XmlDocument xmlDoc = new XmlDocument();
    private XmlNodeList xmlList;

    private List<Level> lvlList = new List<Level>();
    private Level _currentLvl;


    void Start()
    {
        LoadFromXML();
        if (lvlList.Count == 0)
        {
            Level lvl = new Level();
            lvlList.Add(lvl);
        }

        LoadSrite();

        _currentLvl = lvlList[0];

        ShowDescrip();
    }

    void LoadFromXML()
    {
        TextAsset xmlFile = Resources.Load<TextAsset>("Levels");
        xmlDoc.LoadXml(xmlFile.text);
        xmlList = xmlDoc.GetElementsByTagName("lvl");

        foreach (XmlNode xmlData in xmlList)
        {
            XmlNodeList lvlInfo = xmlData.ChildNodes;
            Level lvl = new Level();

            foreach (XmlNode data in lvlInfo)
            {
                switch (data.Name)
                {
                    case "number":
                        lvl.number = int.Parse(data.InnerText);
                        break;
                    case "imageName":
                        lvl.imageName = data.InnerText;
                        break;
                    case "height":
                        lvl.height = int.Parse(data.InnerText);
                        break;
                    case "weight":
                        lvl.weight = int.Parse(data.InnerText);
                        break;
                    case "needToWin":
                        lvl.needToWin = (ItemNumber) int.Parse(data.InnerText);
                        break;
                    case "squareSize":
                        lvl.squareSize = float.Parse(data.InnerText);
                        break;
                    case "lvlDescrip":
                        lvl.lvlDescrip = data.InnerText;
                        break;
                }
            }
            lvlList.Add(lvl);
        }
    }
    void LoadSrite()
    {
        foreach (Level lvl in lvlList)
        {
            Sprite sprt = Resources.Load<Sprite>("GameSprites/" + lvl.imageName);
            lvl.lvlImage = sprt;
        }
    }
    void ShowDescrip()
    {
        DescriptionLvl.text = _currentLvl.lvlDescrip;
        ImageLvl.sprite = _currentLvl.lvlImage;
    }

    public void MoveLeft()
    {
        int id = _currentLvl.number;
        if (id == 0)
            _currentLvl = lvlList[lvlList.Count - 1];
        else
            _currentLvl = lvlList[id - 1];

        ShowDescrip();
    }

    public void MoveRight()
    {
        int id = _currentLvl.number;
        if (id == (lvlList.Count - 1))
            _currentLvl = lvlList[0];
        else
            _currentLvl = lvlList[id + 1];

        ShowDescrip();
    }
}
