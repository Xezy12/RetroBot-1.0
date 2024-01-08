using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEffect : MonoBehaviour
{
    public float rotationSpeed = 35f;
    public float shrinkSpeed = 0.3f;
    public float disappearTime = 3f;

    private float elapsedTime = 0f;

    public bool activate = false;

    void Update()
    {
        if(activate){
            // Rotate the sprite
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // Shrink the sprite
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, 0f) * Time.deltaTime;

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Check if it's time to disappear
            if (elapsedTime >= disappearTime){
                // Destroy the GameObject
                Destroy(gameObject);
            }
        }
    }
}
