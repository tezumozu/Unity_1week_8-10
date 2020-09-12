using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using Foods;
using Bottles;

namespace Players{
    public class Player : MonoBehaviour
    {

        public static Dictionary<FoodKind,ReactiveProperty<int>> foodNums = new Dictionary< FoodKind , ReactiveProperty<int> >();
        private PlayerData playerData;
    // Start is called before the first frame update
        void Start()
        {
            //ゲームの初期化
                //ゲームにセーブデータがあるか
                if(PlayerPrefs.HasKey("SaveData")){
                    //ある場合
                    this.playerData = JsonUtility.FromJson<PlayerData>( PlayerPrefs.GetString("SaveData") );
                }else{
                    //ない場合
                    playerData = makeNewData();
                }
                if(foodNums.Count == 0){
                    foodNums.Add(FoodKind.EBI,new ReactiveProperty<int>(0));
                    foodNums.Add(FoodKind.KOME,new ReactiveProperty<int>(0));
                    foodNums.Add(FoodKind.WAKAME,new ReactiveProperty<int>(0));
                    foodNums.Add(FoodKind.SABA,new ReactiveProperty<int>(0));
                }
                
               
            //データをもとに復元
                //ボトルの生成


            //オートセーブの実装
            StartCoroutine( outSave() );
        }

        private IEnumerator outSave(){
            while(true){
                yield return StartCoroutine( dataSave() );
                yield return new WaitForSeconds(60f);
            }
        }

        public IEnumerator dataSave(){//データをセーブ
            //データの更新
            playerData.foodMap[FoodKind.EBI] = foodNums[FoodKind.EBI].Value;
            playerData.foodMap[FoodKind.KOME] = foodNums[FoodKind.KOME].Value;
            playerData.foodMap[FoodKind.SABA] = foodNums[FoodKind.SABA].Value;
            playerData.foodMap[FoodKind.WAKAME] = foodNums[FoodKind.WAKAME].Value;

            //データを変換
            string saveDataString = JsonUtility.ToJson(this.playerData);

            //データのセーブ
            PlayerPrefs.SetString("SaveData",saveDataString);

            yield break;   
        }

        private PlayerData makeNewData(){
            PlayerData result = new PlayerData();
            result.bottleList = new List<BottleData>();
            
            Dictionary<FoodKind,int> foodMap = new Dictionary< FoodKind , int >();
            foodMap.Add(FoodKind.EBI,0);
            foodMap.Add(FoodKind.KOME,0);
            foodMap.Add(FoodKind.WAKAME,0);
            foodMap.Add(FoodKind.SABA,0);
            result.foodMap = foodMap;

            result.money = 300;
            return result;
        }


        public void addFood(FoodKind kind,int num){
            foodNums[kind].Value += num;
        }


        public void useFood(FoodKind kind){
            if(kind == FoodKind.KOME){
                Bottle.activeBottle.addFood(kind);
                return;
            }
            foodNums[kind].Value -= 1;
            if(0 > foodNums[kind].Value){
                foodNums[kind].Value = 0;
            }

            Bottle.activeBottle.addFood(kind);
        }


        private void Restoration(){

        }
    }

}
