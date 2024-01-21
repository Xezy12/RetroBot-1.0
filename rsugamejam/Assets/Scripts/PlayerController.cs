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
    private RaycastHit2D hit;

    public float walkSpeed = 0.3f;
    public PlayerInventory inventory;
    public AudioSource pickupSound;
    public AudioSource[] movementSounds;
    private SpriteRenderer spriteRenderer;
    private static PlayerController _instance;

    private Transform playerTransform;
    private BoxCollider2D playerCollider;
    private bool win = false;
    private bool isleft = false;
    public LaserController lasersw;
    private string lastdir = "Right";
    [SerializeField]private GameObject Camera;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        lasersw = FindObjectOfType<LaserController>(); 
    }

    void Update()
    {
        MovePlayer();
        
    }

    public void EnqueueMovementHistory()
    {
        Camera.GetComponent<CameraController>().enabled = true;
        if(!inputManager.Clicked){
            List<string> movementHistory = inputManager.GetMovementHistory();
        
            foreach (string direction in movementHistory)
            {
                movementQueue.Enqueue(direction);
            }
            inputManager.Clicked = true;
            inputManager.ShowAndResetMovementHistory();
        }
        
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
                //inputManager.Clicked = true;
                StartCoroutine(checkwin());
            }
        }
    }

    IEnumerator checkwin(){
        yield return new WaitForSeconds(0.5f);
        if (!win)
        {
            resetUI();
        }
        else
        {
            string currentLevelName = SceneManager.GetActiveScene().name;
            // Set the current level as cleared
            PlayerPrefs.SetInt(currentLevelName + "_Cleared", 1);
            PlayerPrefs.Save(); // Save PlayerPrefs to make changes permanent
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
                hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize*2, LayerMask.GetMask("Obstruct2"));
                if (hit.collider == null)
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
                else{
                    hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize*2, LayerMask.GetMask("Obstruct"));
                    if (hit.collider == null)
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
                }
                break;
            case "Punch":
                switch (lastdir){
                    case "Up":
                        targetPosition += Vector2.up * tileSize;
                        break;
                    case "Down":
                        targetPosition += Vector2.down * tileSize;
                        break;
                    case "Left":
                        targetPosition += Vector2.left * tileSize;
                        break;
                    case "Right":
                        targetPosition += Vector2.right * tileSize;
                        break;
                }
                hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize, LayerMask.GetMask("Obstruct"));
                if(hit.collider.CompareTag("Destroyable")){
                    Destroy(hit.collider.gameObject);
                }
                targetPosition = transform.position;
                break;
            
        }

        hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize, LayerMask.GetMask("Obstruct"));
        if (hit.collider == null)
        {
            hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, tileSize, LayerMask.GetMask("Obstruct2"));
            if(hit.collider == null){
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
        lasersw.switching();
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
            Debug.Log("Success");
            if(inventory.HasItem("Keyitem")){
                win = true;
            }
            movementQueue.Clear();
        }
    }

    void flip(){
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
