using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    MenuManager manager;
    public void Start(){
        manager =  GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }
    public void OnClick(){
        manager.setActiveNormal(false);
        manager.setActiveFood(true);
    }
}
