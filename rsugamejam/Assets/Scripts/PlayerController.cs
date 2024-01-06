using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float tileSize = 1f; // Adjust this value based on your tile size
    private InputManager inputManager;
    private Queue<string> movementQueue = new Queue<string>();
    private bool cd = false; // set cooldown before walk;
    public float walkspeed = 1f;
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        // Check if the Spacebar is pressed
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnqueueMovementHistory();
        }
        */
        
        // Move the player if there are movements in the queue
        MovePlayer();
        
    }
    
    public void EnqueueMovementHistory()
    {
        // Retrieve the movement history from the InputManager
        List<string> movementHistory = inputManager.GetMovementHistory();

        // Enqueue each movement direction
        foreach (string direction in movementHistory)
        {
            movementQueue.Enqueue(direction);
        }
        inputManager.ShowAndResetMovementHistory();
    }

    void MovePlayer()
    {
        if (movementQueue.Count > 0 && !cd)
        {
            // start wait event
            StartCoroutine(Waitdelay());
            // Dequeue the next movement direction
            string direction = movementQueue.Dequeue();
            // Move the player based on the direction
            switch (direction)
            {
                case "MoveUp":
                    transform.Translate(Vector2.up * tileSize);
                    break;
                case "MoveDown":
                    transform.Translate(Vector2.down * tileSize);
                    break;
                case "MoveLeft":
                    transform.Translate(Vector2.left * tileSize);
                    break;
                case "MoveRight":
                    transform.Translate(Vector2.right * tileSize);
                    break;
            }
        }
    }
    IEnumerator Waitdelay()
    {   
        cd = true; // on cooldown time
        yield return new WaitForSeconds(walkspeed); // wait for walkspeed seconds
        cd = false; // cooldown finish
    }
}
