using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMenuButton : MonoBehaviour
{
    MenuManager manager;
    public void Start(){
        manager =  GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }
   public void OnClick(){
        manager.setActiveFood(false);
        manager.setActiveNormal(true);
    }
}
