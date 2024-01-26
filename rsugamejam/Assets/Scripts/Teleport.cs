using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]private GameObject Teleport2;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Debug.Log(Teleport2.transform.position);
            StartCoroutine(teleport(other));
        }
    }
    IEnumerator teleport(Collider2D others)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed
        others.transform.position = Teleport2.transform.position;
    }
}
