using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Players;
public class BackTitleButton : MonoBehaviour
{
    private Animator animator;
    public void Start(){
        this.animator = GameObject.Find("Fead").GetComponent<Animator>();
    }
    public void  OnClick(){
        StartCoroutine( exitGame() );
    }

    private IEnumerator exitGame(){
        Player player = GameObject.Find("Player").GetComponent<Player>();
        yield return StartCoroutine( player.dataSave() );
        animator.SetTrigger("feadOut");
        yield break;
    }
}
