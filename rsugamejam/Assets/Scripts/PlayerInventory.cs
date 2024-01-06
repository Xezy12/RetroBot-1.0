using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<string> inventory = new List<string>();

    // Function to add an item to the inventory
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log("Added " + item + " to the inventory.");
    }

    // Function to check if the inventory contains a specific item
    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }

    // Function to remove an item from the inventory
    public void RemoveItem(string item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            Debug.Log("Removed " + item + " from the inventory.");
        }
        else
        {
            Debug.Log("Item " + item + " not found in the inventory.");
        }
    }
}
