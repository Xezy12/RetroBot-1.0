using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showlist : MonoBehaviour
{
    public Text listItemPrefab; // Reference to a prefab of your list item (Text element)
    public Transform contentPanel; // Reference to the parent transform where the list items will be placed
    public List<string> itemList = new List<string>(); // Your list of items
    public InputManager inputManager;

    [SerializeField] float y = 160f;
    [SerializeField] float x = -80f;
    [SerializeField] float startY;
    [SerializeField] float startX;

    // Set the threshold for starting a new column
    [SerializeField] float columnThresholdY = -180f;

    public void PopulateList()
    {
        ClearList();
        startY = y;
        startX = x; // Reset the X position for each new column
        inputManager = FindObjectOfType<InputManager>();
        itemList = inputManager.GetMovementHistory();
        // Loop through the list and instantiate a Text element for each item
        foreach (string item in itemList)
        {
            Text listItem = Instantiate(listItemPrefab, contentPanel);
            listItem.text = item;

             // Adjust the Y position for the next item
            RectTransform listItemRectTransform = listItem.GetComponent<RectTransform>();
            listItemRectTransform.anchoredPosition = new Vector2(startX, startY);

            // Increment the Y position for the next item
            startY -= listItemRectTransform.sizeDelta.y; // Adjust based on the size of the text element

            if (startY < columnThresholdY)
            {
                startX += listItemRectTransform.sizeDelta.x; // Adjust based on the size of the text element
                startY = y; // Reset the Y position for the new column
            }
        }
    }
    public void ClearList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
