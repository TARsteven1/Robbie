using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    int openID;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");
       // GameManager.RegisterDoor(this);
        GameManager.Register<Door>(this);
    }
    public void Open()
    {
        anim.SetTrigger(openID);
        AudioManager.PlayDoorOpenAudio();
    }

    // Update is called once per frame
  
}
