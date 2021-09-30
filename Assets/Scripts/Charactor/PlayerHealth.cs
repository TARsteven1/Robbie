using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    public GameObject deathShadowPrefab;
    int trapsLayer;
    bool isDeath;
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
        if (collision.gameObject.layer==trapsLayer&&!isDeath)
        {
            isDeath = true;
            //播放粒子效果
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            Instantiate(deathShadowPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(-45,90)));
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDied();
        }

    }
}
