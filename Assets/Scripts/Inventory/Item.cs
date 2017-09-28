using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {

    public enum Category
    {
        USABLE,
        WEAPON,
        ARMORY
    }
    public string name;
}
