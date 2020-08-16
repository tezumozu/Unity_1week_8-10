using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public void Start(){
        this.animator = GameObject
                        .Find("Fead").GetComponent<Animator>();
    }
    public void  OnClick(){
        animator.SetTrigger("feadOut");
    }
}
