using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;

public class ItemStyle : MonoBehaviour
{
    public List<Item> AllItemStyle = new List<Item>();

    private List<ItemNumber> _itemNum;

    private XmlDocument xmlDoc = new XmlDocument();
    private XmlNodeList xmlList;

    void Start()
    {
        GetStyle();
    }

    void GetStyle()
    {
        _itemNum = new List<ItemNumber>((ItemNumber[])Enum.GetValues(typeof(ItemNumber)));

        TextAsset xmlFile = Resources.Load<TextAsset>("ItemStyle");
        xmlDoc.LoadXml(xmlFile.text);
        xmlList = xmlDoc.GetElementsByTagName("Item");

        foreach (XmlNode xmlData in xmlList)
        {
            XmlNodeList itemInfo = xmlData.ChildNodes;
            Item item = new Item();

            foreach (XmlNode data in itemInfo)
            {
                switch (data.Name)
                {
                    case "Number":
                        item.Number = (ItemNumber)int.Parse(data.InnerText);
                        break;
                    case "TextColor":
                        item.TextColor = CalculationColor(data.InnerText);
                        break;

                    case "ItemColor":
                        item.ItemColor = CalculationColor(data.InnerText);
                        break;
                }
            }
            AllItemStyle.Add(item);
        }

        if (AllItemStyle.Count != _itemNum.Count)
            Debug.LogError("Не співпадає кількість стилів в ItemStyle.xml та ItemNumber");
    }

    Color CalculationColor(string hex)
    {
        uint HexColor = 0;
        //= uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out HexColor))
            HexColor = 0;
        //;

        float r = ((HexColor & 0xff0000) >> 16) / 255f;
        float g = ((HexColor & 0xff00) >> 8) / 255f;
        float b = ((HexColor & 0xff)) / 255f;

        return new Color(r, g, b);
    }
}
