using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bottles;
namespace Foods{
    public class FoodFactory
    {
        private static GameObject prefab;
        private static FoodFactory factoryInstance;
        public static FoodFactory factory
        {
            get{
                if(factoryInstance == null){
                    factoryInstance = new FoodFactory();
                }

                return factoryInstance;
            }
        }
        
        static private Dictionary<FoodKind,Sprite[]> spriteMap = new Dictionary<FoodKind,Sprite[]>();
        static private Dictionary<FoodKind,Color> colorMap = new Dictionary<FoodKind,Color>();
        private FoodFactory(){
            prefab = (GameObject)Resources.Load("Prefab/Food");

            //データの追加
            spriteMap.Add(FoodKind.EBI,getSprite(FoodKind.EBI));
            spriteMap.Add(FoodKind.WAKAME,getSprite(FoodKind.WAKAME));
            spriteMap.Add(FoodKind.SABA,getSprite(FoodKind.SABA));
            spriteMap.Add(FoodKind.KOME,getSprite(FoodKind.KOME));

            colorMap.Add(FoodKind.EBI,new Color(0.1f,0,0,0));
            colorMap.Add(FoodKind.WAKAME,new Color(0,0.1f,0,0));
            colorMap.Add(FoodKind.SABA,new Color(0,0,0.1f,0));
            colorMap.Add(FoodKind.KOME,new Color(0,0,0,0));
        }

        private static Sprite[] getSprite(FoodKind kind){
            Dictionary<FoodKind,string> pathMap = new Dictionary<FoodKind,string>();
            pathMap.Add(FoodKind.EBI,"Shrimp");
            pathMap.Add(FoodKind.WAKAME,"Wakame");
            pathMap.Add(FoodKind.SABA,"Saba");
            pathMap.Add(FoodKind.KOME,"Kome");

            Sprite[] result = new Sprite[3];
            return result;
        }

        public Food makeNewFood(FoodKind kind){
            //foodの上限に達しているか
            if(Food.isFoodMax()){
                return null;
            }
            FoodData data;
            data.stuff = 2.0f;
            data.color = (Color)colorMap[kind];
            data.eatableNum = 3;
            data.sprites = (Sprite[])spriteMap[kind];
            data.pos = new Vector3(0,0,0);

            //プレハブ生成

            Food result = GameObject.Instantiate(prefab).GetComponent<Food>();
            //データをセット
            result.setData(data);

            return result;
        }

        public Food makeNewFood(FoodData data,Bottle bottle){
            Food food = GameObject.Instantiate(prefab).GetComponent<Food>();
            food.setData(data,bottle);

            return food;
        }
    }

}
