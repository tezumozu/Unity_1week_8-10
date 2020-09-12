using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

using Foods;
using Bottles;
using Players;

public class GiveFoodButton : MonoBehaviour
{   
    Text foodNumText = null;
    static Player player;

    FoodKind kind = FoodKind.KOME;

    void Start(){
        foodNumText = transform.FindChild("Text").gameObject.GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        kind = tagToFoodKind(gameObject.tag);

        Player.foodNums[kind].Subscribe(num => changeText(num));
    }


    public void OnClick(){
        player.useFood(kind);
    }


    public void changeText(int num){
        if(kind == FoodKind.KOME){
            return;
        }
        foodNumText.text = num.ToString();
        //なくなったときに無効にする
        if(num < 0){
            gameObject.GetComponent<Button>().interactable = false;
        }else{
            gameObject.GetComponent<Button>().interactable = true;
        }
    }


    private static FoodKind tagToFoodKind(string tag){
        FoodKind kind = FoodKind.KOME;
        switch(tag){
            case "giveEbi":
                kind = FoodKind.EBI;
                break;
            
            case "giveWakame":
                kind = FoodKind.WAKAME;
                break;

            case "giveSaba":
                kind = FoodKind.SABA;
                break;
            
            case "giveRice":
                kind = FoodKind.KOME;
                break;
        }

        return kind;
    }
}
