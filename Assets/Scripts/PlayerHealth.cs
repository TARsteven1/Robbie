using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrafab;
    int trapsLayer;
    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==trapsLayer)
        {
            //播放粒子效果
            Instantiate(deathVFXPrafab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
