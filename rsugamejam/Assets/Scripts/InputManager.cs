using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<string> movementHistory = new List<string>();

    private Showlist ShowList;

    [SerializeField] private int ghostdefault = 1;
    [SerializeField] private int punchdefault = 1;
    [SerializeField] private int walkdefault = 10;
    private int walklimit;
    private int punchnum;
    private int ghostnum;
    
    public bool Clicked;

    void Start(){
        walklimit = walkdefault;
        punchnum = punchdefault;
        ghostnum = ghostdefault;
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
        walklimit = walkdefault;
        punchnum = punchdefault;
        ghostnum = ghostdefault;
    }

    public void Up()
    {
        if(!Clicked && walklimit > 0){
            movementHistory.Add("MoveUp");
            ShowList.PopulateList();
            walklimit -= 1;
            }
        
    }

    public void Down()
    {
        if(!Clicked && walklimit > 0){
            movementHistory.Add("MoveDown");
            ShowList.PopulateList();
            walklimit -= 1;
        }
        
    }

    public void Left()
    {
        if(!Clicked && walklimit > 0){
            movementHistory.Add("MoveLeft");
            ShowList.PopulateList();
            walklimit -= 1;
        }
        
    }

    public void Right()
    {
        if(!Clicked && walklimit > 0){
            movementHistory.Add("MoveRight");
            ShowList.PopulateList();
            walklimit -= 1;
        }
    }

    public void Ghost()
    {
        if(!Clicked && ghostnum > 0){
            movementHistory.Add("Ghost");
            ShowList.PopulateList();
            ghostnum -= 1;
        }
    }
    
    public void Punch()
    {
        if(!Clicked && punchnum > 0){
            movementHistory.Add("Punch");
            ShowList.PopulateList();
            punchnum -= 1;
        }
    }
}
