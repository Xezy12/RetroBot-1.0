using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float tileSize = 1f;
    private InputManager inputManager;
    public Queue<string> movementQueue = new Queue<string>();
    private bool isMoving = false;
    private bool cd = false;

    public float walkSpeed = 0.3f;
    public PlayerInventory inventory;
    public AudioSource pickupSound;
    public AudioSource[] movementSounds;

    private static PlayerController _instance;

    private Transform playerTransform;
    private BoxCollider2D playerCollider;
    private bool win = false;
    private bool isleft = false;
    private string lastdir;

    public GameObject popupAfterTheEnd;
    public Button playAgainButton;
    public Button quitButton;
    public Animator popupAfterTheEndAnimator;
    public Animator transition;

    public static PlayerController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
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
        if(!inputManager.Clicked){
            MovePlayer();
        }
    }

    public void EnqueueMovementHistory()
    {
        List<string> movementHistory = inputManager.GetMovementHistory();

        foreach (string direction in movementHistory)
        {
            movementQueue.Enqueue(direction);
        }
        inputManager.ShowAndResetMovementHistory();
    }

    /*
    private bool CheckIfPlayerReachedGoal()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        playerTransform = GetComponent<Transform>();
        Vector2 playerSize = playerCollider.size;

        Vector3 newPosition = playerTransform.position;
        newPosition.x += 1f;
        playerTransform.position = newPosition;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(playerTransform.position, playerSize, 0f, LayerMask.GetMask("Goal"));

        Debug.Log("Player position: " + playerTransform.position);
        Debug.Log("Player collider size: " + playerSize);
        Debug.Log("Number of colliders detected: " + colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider detected: " + collider.name + " with tag: " + collider.tag + " on layer: " + LayerMask.LayerToName(collider.gameObject.layer));
            if (collider.CompareTag("Goal"))
            {
                return true;
            }
        }

        return false;
    }*/

    private void resetUI()
    {
        popupAfterTheEnd.SetActive(true);
        popupAfterTheEndAnimator.SetTrigger("ShowPopup");

        // Assign functions to buttons
        playAgainButton.onClick.AddListener(PlayAgain);
        quitButton.onClick.AddListener(QuitGame);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        float transitionTime = 2f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void PlayAgain()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void MovePlayer()
    {
        if (movementQueue.Count > 0 && !isMoving && !cd)
        {
            string direction = movementQueue.Dequeue();
            StartCoroutine(MoveToPosition(direction));

            // Play a random movement sound
            if (movementSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, movementSounds.Length);
                movementSounds[randomIndex].Play();
            }
            if(movementQueue.Count <= 0){
                inputManager.Clicked = true;
                StartCoroutine(checkwin());
            }
        }
    }

    IEnumerator checkwin(){
        yield return new WaitForSeconds(0.7f);
        if (!win)
        {
            resetUI();
        }
        else{
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
        }
    }

    IEnumerator MoveToPosition(string direction)
    {
        isMoving = true;
        Vector2 targetPosition = transform.position;

        switch (direction)
        {
            case "MoveUp":
                targetPosition += Vector2.up * tileSize;
                lastdir = "Up";
                break;
            case "MoveDown":
                targetPosition += Vector2.down * tileSize;
                lastdir = "Down";
                break;
            case "MoveLeft":
                if(!isleft){
                    isleft = !isleft;
                    flip();
                }
                targetPosition += Vector2.left * tileSize;
                lastdir = "Left";
                break;
            case "MoveRight":
                if(isleft){
                    isleft = !isleft;
                    flip();
                }
                targetPosition += Vector2.right * tileSize;
                lastdir = "Right";
                break;
            case "Ghost":
                switch (lastdir){
                    case "Up":
                        targetPosition += Vector2.up * tileSize * 2;
                        break;
                    case "Down":
                        targetPosition += Vector2.down * tileSize * 2;
                        break;
                    case "Left":
                        targetPosition += Vector2.left * tileSize * 2;
                        break;
                    case "Right":
                        targetPosition += Vector2.right * tileSize * 2;
                        break;
                }
                transform.position = targetPosition;
                break;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize, LayerMask.GetMask("Obstruct"));


        if (hit.collider == null || !hit.collider.CompareTag("Obstruct"))
        {
            float elapsedTime = 0f;
            Vector2 startingPosition = transform.position;

            while (elapsedTime < walkSpeed)
            {
                transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / walkSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
        else
        {
            // There is an obstacle, do not move
            Debug.Log("Obstacle detected, cannot move!" + hit.collider.tag);
        }
        isMoving = false;

        StartCoroutine(WaitDelay());
    }

    IEnumerator WaitDelay()
    {
        cd = true;
        yield return new WaitForSeconds(0.7f); // Adjust the delay time as needed
        cd = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BridgeMakingItem"))
        {
            inventory.AddItem("BridgeMakingItem");
            pickupSound.Play();
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag("Goal")){
            win = true;
            Debug.Log("Success");
            movementQueue.Clear();
        }
    }

    void flip(){
        // Get the current scale of the object
        Vector3 scale = transform.localScale;
        // Flip the x-axis
        scale.x *= -1;
        // Apply the new scale to the object
        transform.localScale = scale;
    }
}
