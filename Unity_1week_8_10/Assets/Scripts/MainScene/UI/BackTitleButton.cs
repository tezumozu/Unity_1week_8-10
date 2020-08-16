using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTitleButton : MonoBehaviour
{
    private Animator animator;
    public void Start(){
        this.animator = GameObject
                        .Find("Fead").GetComponent<Animator>();
    }
    public void  OnClick(){
        animator.SetTrigger("feadOut");
    }
}
