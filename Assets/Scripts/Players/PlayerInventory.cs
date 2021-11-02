using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public string[] inventory = new string[10];

    private int itemCount = 0;

    public void AddItem(string itemName)
    {
        if (itemCount >= 10) itemCount = 0;

        inventory[itemCount] = itemName;
        itemCount++;
    }

    public void RemoveItem(int index)
    {
        if (inventory[index] != null)
        {
            inventory[index] = string.Empty;
            if(inventory[index + 1] != null)
            {
                inventory[index] = inventory[index + 1];
                RemoveItem(index + 1);
            }
        }
    }

    public void DisplayItems()
    {

    }
}
