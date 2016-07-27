using UnityEngine;
using System.Collections;
using System;

public enum ItemNumber
{
    None = 0,
    _2,
    _4,
    _8,
    _16,
    _32,
    _64,
    _128,
    _256,
    _512,
    _1024,
    _2048,
    _4096
};


[Serializable]
public class Item
{
    public ItemNumber Number { get; set; }
    public Color TextColor { get; set; }
    public Color ItemColor { get; set; }
    public bool CanMerge { get; set; }
}
