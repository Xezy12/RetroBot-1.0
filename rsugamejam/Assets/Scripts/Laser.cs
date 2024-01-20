using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private PlayerController playercontrol;
    [SerializeField]private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        playercontrol = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playercontrol.lasersw){
            gameObject.SetActive(!active)
            playercontrol.lasersw = false;
        }
    }
}
