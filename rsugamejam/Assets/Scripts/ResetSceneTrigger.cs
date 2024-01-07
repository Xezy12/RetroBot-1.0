using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;

public class ResetSceneTrigger : MonoBehaviour
{
    public GameObject popupAfterTheEnd;
    public Button playAgainButton;
    public Button quitButton;
    public Animator popupAfterTheEndAnimator;
    public Animator transition;
    public AudioSource BridgingSFX;
    public AudioSource FallingSFX;

    public Tilemap bridgeTilemap; // Reference to your Tilemap
    public TileBase bridgeTile; // Reference to your bridge tile

    private bool hasBridgeMakingItem = false;
    private PlayerInventory playerInventory;

    void Start()
    {
        popupAfterTheEnd.SetActive(false);
        // Find the PlayerController in the scene and get its PlayerInventory component
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerInventory = playerController.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    void Update()
    {
        // Check for the bridge-making-item in the player's inventory
        if (playerInventory != null)
        {
            hasBridgeMakingItem = playerInventory.HasItem("BridgeMakingItem");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject != null && other.CompareTag("Player"))
        {
            if (!hasBridgeMakingItem)
            {
                // Trigger the "ShowPopup" animation
                FallingSFX.Play();
                popupAfterTheEnd.SetActive(true);
                popupAfterTheEndAnimator.SetTrigger("ShowPopup");

                // Assign functions to buttons
                playAgainButton.onClick.AddListener(PlayAgain);
                quitButton.onClick.AddListener(QuitGame);
            }
            // If the player has the bridge-making-item, let them walk through the trap without triggering the popup
            else
            {
                // Implement the logic to allow the player to walk through the trap
                // For example, you can disable the collider of the trap or perform other actions
                // For demonstration purposes, we'll just print a message to the console
                Debug.Log("Player has the bridge-making-item. Walking through the trap without triggering the popup.");
                Vector3Int tilePosition = bridgeTilemap.WorldToCell(transform.position);
                bridgeTilemap.SetTile(tilePosition, bridgeTile);
                BridgingSFX.Play();
            }
        }
    }

    // Function to restart the scene
    public void PlayAgain()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    // Function to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        float transitionTime = 2f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
