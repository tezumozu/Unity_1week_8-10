using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenuButton : MonoBehaviour
{
    
    MenuManager manager;
    public void Start(){
        manager =  GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }
   public void OnClick(){
        manager.setActiveNormal(true);
        manager.setActiveMove(false);
    }
}
