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
        //静的
        private static int foodNum = 0; 
        private const int maxFoodNum = 5;
        private static Subject<Food> applyFood = new Subject<Food>(); 
        public static IObservable<Food> rApplyFood
        {
            get{ return applyFood; }
        }

        //インスタンス
        private bool pinchFlag = false;
        private FoodData foodData;
        private IDisposable bottleDispos;
        private Subject<Unit> disappear = new Subject<Unit>();
        public IObservable<Unit> rDisappear
        {
            get{ return disappear; }
        }
        

        void Start()
        {
            foodNum++;
            this.UpdateAsObservable()
                .Where(_ => this.pinchFlag == true)
                .Subscribe(_ => pinching());

            isActive(false);
        }


        public Color eat(){//食べられた時の処理
            foodData.eatableNum--;

            //食べられきったら
            if(foodData.eatableNum <= 0){
                disappear.OnCompleted();
                bottleDispos.Dispose();
                Destroy(gameObject);
                
                return foodData.color;
            }

            //次のスプライトへ変更
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = foodData.sprites[foodData.eatableNum]; 
            
            return foodData.color;
        }


        public void setData(FoodData data){
            this.foodData = data;
            Bottle bottle = Bottle.activeBottle;

            //スプライトの変更
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = foodData.sprites[foodData.eatableNum]; 
            
            //ボトルの表示を監視
            bottleDispos = bottle.isActive
            .Subscribe(flag => this.isActive(flag));

            //表示状況の設定
            isActive(bottle.isActive.Value);

            //ご飯の出現をぷ二に通知
            applyFood.OnNext(this);
        }


        public void setData(FoodData data,Bottle bottle){
            this.foodData = data;

            //スプライトの変更
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = foodData.sprites[foodData.eatableNum]; 
            
            //ボトルの表示を監視
            bottleDispos = bottle.isActive
            .Subscribe(flag => this.isActive(flag));

            //表示状況の設定
            isActive(bottle.isActive.Value);

            //ご飯の出現をぷ二に通知
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
                foodData.pos = mousePos;
            }
        }


        private bool checkOutSide(Vector3 vec){
            if(Math.Abs(vec.y) > 4.5f){
                return false;
            }

            if(Math.Pow( Math.Pow(vec.x,2) + Math.Pow(vec.y,2) ,1/2) < Bottle.bottleSize){
                return false;
            }   

            return true;
        }


        private void isActive(bool flag){
            gameObject.SetActive(flag);
        }


        public FoodData getFoodData(){
            return this.foodData;
        }


        public static bool isFoodMax (){
            if(maxFoodNum >= foodNum){
                return true;
            }
            return false;
        }


        public Vector3 getPos(){
            return foodData.pos;
        }
    }
}

