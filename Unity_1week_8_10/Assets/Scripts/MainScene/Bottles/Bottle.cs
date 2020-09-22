using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using Foods;
using Punis;

namespace Bottles{
    public class Bottle : MonoBehaviour
    {
        public const float bottleSize = 6.3f;
        public static Bottle activeBottle;


        private BottleData bottleData;
        private ReactiveProperty<bool> _activeFlag = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> isActive
        {
            get{
                return _activeFlag;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            isActive
            .Where(_ => _ == true)
            .Subscribe(_ => activeBottle = this);
        }


        //シャーレの有効無効
        void setBottleActive(bool flag){
            _activeFlag.Value = flag;
        }


        //データのセット
        public void setData(BottleData data){//データをもとに復元
            this.bottleData = data;

            for(int i = 0; i < data.puniList.Count; i++){
                Puni.makeNewPuni(data.puniList[i],this);
            }

            for(int i = 0; i < data.foodList.Count; i++){
                FoodFactory.factory.makeNewFood(data.foodList[i],this);
            }
        }


        public void addPuni(PuniData data){
            bottleData.puniList.Add(data);
        }


        public void removePuni(PuniData data){
            bottleData.puniList.Remove(data);
        }


        public void addFood(FoodKind kind){
            Food food  = FoodFactory.factory.makeNewFood(kind);
            if(food == null){
                Debug.Log("ご飯がいっぱいです");
                return;
            }

            FoodData data = food.getFoodData();
            bottleData.foodList.Add(data);
        }


        public void removeFood(FoodData data){
            bottleData.foodList.Remove(data);
        }


        public void newBottleInit(){
            this.bottleData = new BottleData();
            bottleData.puniList = new List<PuniData>();
            bottleData.foodList = new List<FoodData>();
        }


        
    }
}

