using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager Instance;
    // Start is called before the first frame update
    public TextMeshProUGUI orbText, timeText, deathText, gameoverText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public static void UpdateOrbUI(int orb)
    {
        Instance.orbText.text = GameManager.Instance.orbs.Count.ToString();
    }
    public static void UpdateDeathUI(int deathCount)
    {
        Instance.deathText.text = GameManager.Instance.deathCount.ToString();
    }
    public static void UpdateTimeUI(int time)
    {
        int minutes = (int)(time / 60);
        int seconds = time % 60;
        Instance.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    public static void DisplayGameOver()
    {
        
        Instance.gameoverText.enabled= true;
    }

}
