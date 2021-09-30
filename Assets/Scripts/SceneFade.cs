using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    Animator anim;
    int fadeID;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fadeID = Animator.StringToHash("Fade");
        GameManager.RegisterSceneFader(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeOut()
    {
        anim.SetTrigger(fadeID);
    }
}
