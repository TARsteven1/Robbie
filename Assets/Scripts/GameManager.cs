using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    SceneFade SceneFade;
    List<Orb> orbs;
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
    }
    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (Instance.orbs.Count==0)
        {
            return;
        }
        Instance.orbs.Remove(orb);
    }
    public static void PlayerDied()
    {
        Instance.SceneFade.FadeOut();
        Instance.Invoke("RestartScene",1.5f);
    }
    void RestartScene()
    {
        Instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
