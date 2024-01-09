using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float tileSize = 1f;
    private InputManager inputManager;
    private Queue<string> movementQueue = new Queue<string>();
    private bool isMoving = false;
    private bool cd = false;

    public float walkSpeed = 0.3f;
    public PlayerInventory inventory;
    public AudioSource pickupSound;
    public AudioSource[] movementSounds;

    private static PlayerController _instance;

    private Transform playerTransform;
    private BoxCollider2D playerCollider;

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
    }

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
                if (!CheckIfPlayerReachedGoal())
                {
                    resetUI();
                }
            }
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
                break;
            case "MoveDown":
                targetPosition += Vector2.down * tileSize;
                break;
            case "MoveLeft":
                targetPosition += Vector2.left * tileSize;
                break;
            case "MoveRight":
                targetPosition += Vector2.right * tileSize;
                break;
        }

        float elapsedTime = 0f;
        Vector2 startingPosition = transform.position;

        while (elapsedTime < walkSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / walkSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
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
    }
}
