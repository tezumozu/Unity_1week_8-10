using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    GameObject foodMenu;
    GameObject moveMenu;
    GameObject normalMenu;
    public void Start(){
        this.foodMenu = GameObject.Find("FoodMenu");
        this.moveMenu = GameObject.Find("MoveMenu");
        this.normalMenu = GameObject.Find("NormalMenu");
        foodMenu.SetActive(false);
        moveMenu.SetActive(false);
    }

    public void setActiveFood(bool active){
        foodMenu.SetActive(active);
    }

    public void setActiveNormal(bool active){
        normalMenu.SetActive(active);
    }

    public void setActiveMove(bool active){
        moveMenu.SetActive(active);
    }


}
