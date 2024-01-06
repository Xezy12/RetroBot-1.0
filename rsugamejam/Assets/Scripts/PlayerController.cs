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
    public PlayerInventory inventory;

    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        // Ensure there is only one instance of PlayerController
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BridgeMakingItem"))
        {
            // Assuming that the item should be added to the player's inventory when touched
            inventory.AddItem("BridgeMakingItem");

            // Optionally, you may want to disable or destroy the GameObject with the "BridgeMakingItem" tag
            // For demonstration purposes, we'll just deactivate it
            other.gameObject.SetActive(false);
        }
    }
}
