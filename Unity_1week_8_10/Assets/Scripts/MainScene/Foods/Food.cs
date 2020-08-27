using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

using Punis;
using Bottles;


namespace Foods{
    public enum FoodKind{
        EBI,
        SABA,
        WAKAME,
        KOME,
    }

    public class Food : MonoBehaviour,IEatable,IPinchable
    {
        float bottleSize = 6.3f;
        
        private bool pinchFlag = false;
        // Start is called before the first frame update         
        private FoodData foodData;
        public static Subject<Food> applyFood = new Subject<Food>();  
        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => this.pinchFlag == true)
                .Subscribe(_ => pinching());
        }


        public Color eat(){//食べられた時の処理
            foodData.eatableNum--;
            if(foodData.eatableNum <= 0){
                Destroy(gameObject);
                
            }

            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = foodData.sprites[foodData.eatableNum]; 
            
            return foodData.color;
        }


        public void setData(FoodData data,Bottle bottle){
            this.foodData = data;
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = foodData.sprites[foodData.eatableNum]; 
            
            bottle.isActive
            .Subscribe(flag => this.isActive(flag));

            isActive(bottle.isActive.Value);

            applyFood.OnNext(this);
        }


        public void pinched(){
            pinchFlag = true;
        }


        public void anPinched(){
            pinchFlag = false;
        }


        public void pinching(){
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if(checkOutSide(mousePos)){
                gameObject.transform.position = mousePos;
            }
        }


        private bool checkOutSide(Vector3 vec){
            if(Math.Abs(vec.y) > 4.5f){
                return false;
            }

            if(Math.Pow( Math.Pow(vec.x,2) + Math.Pow(vec.y,2) ,1/2) < bottleSize){
                return false;
            }   

            return true;
        }


        private void isActive(bool flag){
            gameObject.SetActive(flag);
        }
    }
}

