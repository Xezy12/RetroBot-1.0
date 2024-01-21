using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private List<string> movementHistory = new List<string>();

    private Showlist ShowList;

    [SerializeField] private int ghostdefault = 1;
    [SerializeField] private int punchdefault = 1;
    [SerializeField] private int walkdefault = 10;
    [SerializeField] private TMP_Text ghostie;
    [SerializeField] private TMP_Text walkie;
    [SerializeField] private TMP_Text punchie;

    private int walklimit;
    private int punchnum;
    private int ghostnum;
    
    public bool Clicked;
    public Animator transition;

    void Start(){
        walklimit = walkdefault;
        punchnum = punchdefault;
        ghostnum = ghostdefault;
        ShowList = FindObjectOfType<Showlist>();
        Clicked = false;
    }
    void Update()
    {
        ghostie.text = ghostnum.ToString();
        punchie.text = punchnum.ToString();
        walkie.text = walklimit.ToString();
    }

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
        if(!Clicked){
            walklimit = walkdefault;
            punchnum = punchdefault;
            ghostnum = ghostdefault;
        }
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

    public void BacktoMenu()
    {
        StartCoroutine(LoadLevel(3));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        float transitionTime = 2f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        // Check if the scene index is valid
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogError("Invalid scene index: " + levelIndex);
        }
    }
}
