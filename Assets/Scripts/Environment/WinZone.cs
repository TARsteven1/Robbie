using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==playerLayer)
        {
            GameManager.PlayerWon();
        }
    }
}
