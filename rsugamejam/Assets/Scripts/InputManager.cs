using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<string> movementHistory = new List<string>();

    private Showlist ShowList;
    
    public bool Clicked;

    void Start(){
        ShowList = FindObjectOfType<Showlist>();
        Clicked = false;
    }
    void Update()
    {
        // HandleInput(); not in use

        // Check if the Spacebar is pressed
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowAndResetMovementHistory();
            ShowList.ClearList();
        }
        */
    }

    /*
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
    }
    */

    // You can use this method to retrieve the movement history from other scripts
    public List<string> GetMovementHistory()
    {
        return movementHistory;
    }

    // Display and reset the movement history
    public void ShowAndResetMovementHistory()
    {
        Debug.Log("Movement History:");

        foreach (string direction in movementHistory)
        {
            Debug.Log(direction);
        }
        // Reset the movement history
        movementHistory.Clear();
        ShowList.ClearList();
    }

    public void Up()
    {
        if(!Clicked){
            movementHistory.Add("MoveUp");
            ShowList.PopulateList();
            }
        
    }

    public void Down()
    {
        if(!Clicked){
            movementHistory.Add("MoveDown");
            ShowList.PopulateList();
        }
        
    }

    public void Left()
    {
        if(!Clicked){
            movementHistory.Add("MoveLeft");
            ShowList.PopulateList();
        }
        
    }

    public void Right()
    {
        if(!Clicked){
            movementHistory.Add("MoveRight");
            ShowList.PopulateList();
        }
        
    }
}
