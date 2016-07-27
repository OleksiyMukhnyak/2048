using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum SquareTypes
{
    None = 0,
    Empty,
    NoEmpty
};

[Serializable]
public class SquareHelper : MonoBehaviour
{
    public Item Item { get { return _item; } }
    public int row;
    public int col;
    public SquareTypes type = SquareTypes.Empty;

    public Text Number;


    private Item _item;

    public void AddItem(Item item)
    {
        if (item.Number == ItemNumber.None)
        {
            type = SquareTypes.Empty;
            Number.text = "";
        }
        else
        {
            Number.text = Mathf.Pow(2, (int)item.Number).ToString();
            type = SquareTypes.NoEmpty;
        }

        Number.color = item.TextColor;
        GetComponent<Image>().color = item.ItemColor;

        _item = item;
    }
}
