using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foods;
using Bottles;

namespace Players{
    public class Player : MonoBehaviour
    {

        PlayerData playerData;
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
            //データをもとに復元
                //ボトルの生成


            
        }
        public void dataSave(){//データをセーブ
            string saveDataString = JsonUtility.ToJson(this.playerData);
            PlayerPrefs.SetString("SaveData",saveDataString);
        }

        private PlayerData makeNewData(){
            PlayerData result = new PlayerData();
            result.bottleList = new List<BottleData>();
            
            Dictionary<FoodKind,int> foodMap = new Dictionary<FoodKind,int>();
            foodMap.Add(FoodKind.EBI,0);
            foodMap.Add(FoodKind.KOME,0);
            foodMap.Add(FoodKind.WAKAME,0);
            foodMap.Add(FoodKind.SABA,0);
            result.foodMap = foodMap;

            result.money = 300;
            return result;
        }

        public void addFood(FoodKind kind,int num){
            playerData.foodMap[kind] += num;
        }

        public void useFood(FoodKind kind){
            playerData.foodMap[kind] -= 1;
            if(0 > playerData.foodMap[kind]){
                playerData.foodMap[kind] = 0;
            }
        }

    }

}
