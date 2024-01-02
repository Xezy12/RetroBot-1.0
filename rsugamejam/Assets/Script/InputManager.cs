using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<string> movementHistory = new List<string>();

    void Update()
    {
        HandleInput();

        // Check if the Spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowAndResetMovementHistory();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementHistory.Add("MoveUp");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movementHistory.Add("MoveDown");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movementHistory.Add("MoveLeft");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movementHistory.Add("MoveRight");
        }
    }

    // You can use this method to retrieve the movement history from other scripts
    public List<string> GetMovementHistory()
    {
        return movementHistory;
    }

    // Display and reset the movement history
    void ShowAndResetMovementHistory()
    {
        Debug.Log("Movement History:");

        foreach (string direction in movementHistory)
        {
            Debug.Log(direction);
        }

        // Reset the movement history
        movementHistory.Clear();
    }
}
