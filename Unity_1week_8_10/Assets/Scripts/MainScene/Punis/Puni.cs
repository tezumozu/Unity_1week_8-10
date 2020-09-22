using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx.Triggers;
using UniRx;

using Foods;
using Bottles;

namespace Punis{
    public enum PuniSize{
        S = 0,
        M = 1,
        L = 2,
    }
    public class Puni : MonoBehaviour,IPinchable
    {
        //static
        private const float punisGrowTime = 60f;//単位秒
        private const int punisMaxFullNum = 7;
        private const float maxSize = 0.04f;
        private const int maxFreeAnimeNum = 3;
        private static GameObject prefab;

        private const float speed = 2f;//単位はワールド座標/秒


        //instance
        private PuniData puniData;
        private bool isPinched = false;
        private IDisposable foodTargetDispos;
        private IDisposable foodsDispos;
        private Food target;
        private Vector3 posion;
        private Vector3 size;
        private IDisposable bottleDispos;
        private Animator animator;

        private float smoothTime; 
        
        void Start()
        {
            prefab  = (GameObject)Resources.Load("Prefab/Puni");
            //アニメータの取得
            animator = gameObject.GetComponent<Animator>();
            //初期化
            posion = gameObject.transform.position;
            isActive(false);

            //餌が現れた時の処理
            foodsDispos = Food.rApplyFood.Subscribe(food => setFoodTarget(food));

            //update
            //つままれている間
            this.UpdateAsObservable()
                .Where(_ => this. isPinched== true)
                .Subscribe(_ => pinching());

            //ターゲットがいる間
            this.UpdateAsObservable()
                .Where(_ => this.target != null && this.isPinched == false)
                .Subscribe(_ => huntFood());

            //何もないとき
            this.UpdateAsObservable()
                .Where(_ => this.target == null && this.isPinched == false)
                .Subscribe(_ => free());
        }


        private void setData(PuniData data){
            this.puniData = data;

            //座標を初期化
            posion = puniData.pos;
            gameObject.transform.position = posion;

            //sizeの調整
            size = new Vector3( maxSize/3 * (int)puniData.size , maxSize/3 * (float)puniData.size , 0f);
            gameObject.transform.localScale = size;

            //Bottleの監視
            bottleDispos = Bottle.activeBottle.isActive
            .Subscribe( flag => isActive(flag) );
        }


        private void setData(PuniData data,Bottle bottle){
            this.puniData = data;

            //座標を初期化
            posion = puniData.pos;
            gameObject.transform.position = posion;

            //sizeの調整
            size = new Vector3( maxSize/3 * (int)puniData.size , maxSize/3 * (float)puniData.size , 0f);
            gameObject.transform.localScale = size;

            //Bottleの監視
            bottleDispos = bottle.isActive
            .Subscribe( flag => isActive(flag) );
        }


        //待機中の処理
        void free(){
            //一定の確率でアニメーションの変化 
            animator.SetInteger("Puni",UnityEngine.Random.Range(0,2000));

            //成長タイマーを進める
            puniData.growTime += Time.deltaTime;
            if(puniData.growTime >= punisGrowTime && puniData.fullNum >= punisMaxFullNum){
                growUp();
            }
        }


        public void finFreeAnimation(){
            animator.SetInteger("Puni",0);//初期値に変更
        }


        void isActive(bool flag){
            gameObject.SetActive(flag);
        }

        
        //食べ物を見つけた時の処理
        void setFoodTarget(Food food){

            if(target != null){//現在のターゲットより遠い場合
                if(Vector3.Distance( food.getPos() , posion ) > Vector3.Distance( target.getPos() , posion )){//現在のターゲットより遠い場合
                    return;   
                }
            }
            target = food;
            
            //以前のターゲットの購読を終了
            foodTargetDispos.Dispose();
            //新しいターゲットの購読
            foodTargetDispos = food.rDisappear.Subscribe(_ => foodDisappear());
            //ターゲットまでの予測時間を割り出す
            smoothTime = Vector3.Distance( posion , target.getPos() ) / speed;
            //時間をかけて移動
            Vector3 velocity = Vector3.zero;
            Vector3.SmoothDamp(posion,target.getPos(),ref velocity,smoothTime);
        }


        //ターゲットが消えた時の処理
        void foodDisappear(){
            target = null;
        }


        //餌を食べるための処理
        void huntFood(){
            //十分に近づいた
            if( Vector3.Distance( target.getPos() , posion ) < size.x ){
                //食べるモーションを流す
                animator.SetTrigger("eatFood");

                //餌を食べ,
                puniData.puniColor = target.eat() + puniData.puniColor;
                puniData.puniColor.a = 100;
                //ターゲットを消す
                target = null;
                foodTargetDispos.Dispose();
            }
        }


        //つままれる
        public void pinched(){
            this.isPinched = true;
            finFreeAnimation();
        }


        //はなされる
        public void anPinched(){
            this.isPinched = false;
        }


        //つままれている間の処理
        public void pinching(){
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if(checkOutSide(mousePos)){
                return;
            }

            posion.x = mousePos.x;
            posion.y = mousePos.y;

        }


        //ボトルの移動
        private PuniData changeBottle(Bottle bottle){
            //購読の終了
            bottleDispos.Dispose();

            //新しいボトルの購読開始
            bottleDispos = bottle.isActive
            .Subscribe( flag => isActive(flag) );

            return this.puniData;
        }


        //puniが消滅するときの処理
        private void destoryPuni(){
            //購読終了
            foodsDispos.Dispose();
            bottleDispos.Dispose();
            //food側をとめるので不要？
            foodTargetDispos.Dispose();
        }


        //Puniが成長する
        private void growUp(){
            puniData.growTime = 0f;
            puniData.fullNum = 0;

            //サイズが最大なら一匹増やす
            if(puniData.size == PuniSize.L){
                increasePuni();
            }
                //サイズの変更
            puniData.size = (PuniSize)( ((int)puniData.size + 1) % System.Enum.GetValues(typeof(PuniSize)).Length );
            
            size.x = maxSize/3 * (int)puniData.size;
            size.y = maxSize/3 * (int)puniData.size;
        }


        //puniの増殖
        private void increasePuni(){
            //ぷにデータの生成
            PuniData newData;
            newData.puniColor = new Color(puniData.puniColor.r , puniData.puniColor.g , puniData.puniColor.b , puniData.puniColor.a);

            //増やしても大丈夫な座標を求める
            float tan = puniData.pos.y / puniData.pos.x;
            double aTan = Math.Atan(tan);
            float r = 10;
            double x = r * Math.Cos(aTan);
            double y = r * Math.Sin(aTan);

            Vector3 newPos = new Vector3((float)x,(float)y,1f);
            newData.pos = newPos;
            newData.fullNum = 0;
            newData.growTime = 0f;
            newData.size = PuniSize.S;

            makeNewPuni(newData);

            Bottle.activeBottle.addPuni(newData);
        }


        private bool checkOutSide(Vector3 pos){
            //上下がはみ出しているので
            if(Math.Abs(pos.y) > 4.5f){
                return true;
            }

            //シャーレ内に収まっているか
            if(Math.Pow( Math.Pow(pos.x,2) + Math.Pow(pos.y,2) ,1/2) < Bottle.bottleSize){
                return true;
            }   

            return false;
        }


        public PuniData getPuniData(){
            return puniData;
        }

        //シャーレを指定しないで生成
        public static void makeNewPuni(PuniData data){
            //プレハブの生成
            Puni newPuni = GameObject.Instantiate(prefab).GetComponent<Puni>();
            newPuni.setData(data);
        } 

        //シャーレを指定して生成
        public static void makeNewPuni(PuniData data,Bottle bottle){
            //プレハブの生成
            Puni newPuni = GameObject.Instantiate(prefab).GetComponent<Puni>();
            newPuni.setData(data,bottle);
        } 
    }
}

