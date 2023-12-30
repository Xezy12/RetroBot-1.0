using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float tileSize = 1f; // Adjust this value based on your tile size
    private InputManager inputManager;
    private Queue<string> movementQueue = new Queue<string>();

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        // Check if the Spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnqueueMovementHistory();
        }

        // Move the player if there are movements in the queue
        MovePlayer();
    }

    void EnqueueMovementHistory()
    {
        // Retrieve the movement history from the InputManager
        List<string> movementHistory = inputManager.GetMovementHistory();

        // Enqueue each movement direction
        foreach (string direction in movementHistory)
        {
            movementQueue.Enqueue(direction);
        }
    }

    void MovePlayer()
    {
        if (movementQueue.Count > 0)
        {
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
}
