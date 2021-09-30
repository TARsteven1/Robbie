using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    SceneFade SceneFade;
   public  List<Orb> orbs;
    public int deathCount;
    Door lockedDoor;
    float gameTime;
    bool gameIsOver = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        orbs = new List<Orb>();
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public static void RegisterDoor(Door door)
    {
        Instance.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFade obj)
    {
        Instance.SceneFade = obj;
    }
    public static void RegisterOrb(Orb orb)
    {
        if (!Instance.orbs.Contains(orb))
        {
            Instance.orbs.Add(orb);
        }
        if (Instance.orbs.Count==0)
        {
            Instance.lockedDoor.Open();
        }
        UIManager.UpdateOrbUI(Instance.orbs.Count);
    }
    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (Instance.orbs.Count==0)
        {
            return;
        }
        Instance.orbs.Remove(orb);
        UIManager.UpdateOrbUI(Instance.orbs.Count);
    }
    public static void PlayerDied()
    {
        Instance.SceneFade.FadeOut();
        //Instance.deathCount++;
        Instance.deathCount++;
        UIManager.UpdateDeathUI(Instance.deathCount);
        Instance.Invoke("RestartScene",1.5f);
    }
    public static void PlayerWon()
    {
        Instance.gameIsOver= true;

       UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
    }
    public static bool GameIsOver()
    {
        return Instance.gameIsOver;
    }
    void RestartScene()
    {
        Instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Start()
    {
       
    }
    void Update()
    {
        if (gameIsOver)
        {
            return;
        }
        gameTime += Time.deltaTime;
       
        UIManager.UpdateTimeUI(((int)gameTime));
    }

}
