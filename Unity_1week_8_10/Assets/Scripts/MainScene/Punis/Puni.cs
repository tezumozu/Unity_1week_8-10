using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx.Triggers;
using UniRx;

using Foods;
using Bottles;

namespace Punis{
    enum PuniState{
        MOVE,
        EAT,
        PINCH,
        NOMAL,
    }
    public class Puni : MonoBehaviour,IPinchable
    {
        //static



        //instance
        PuniData puniData;
        bool isPinched = false;
        IDisposable foodTargetDispos;
        IDisposable foodsDispos;
        Food target;
        
        void Start()
        {
            isActive(false);

            //餌が現れた時の処理
            Food.rApplyFood.Subscribe(food => setFoodTarget(food));

            //update
        }


        public void setData(PuniData data,Bottle bottle){
            this.puniData = data;
        }


        void isActive(bool flag){
            gameObject.isActive(flag);
        }

        
        void setFoodTarget(Food food){
            if(target != null){
                if(Vector3.Distance( food.getPos() , puniData.pos ) < Vector3.Distance( target.getPos() , puniData.pos )){
                    target = food;
                }
            }else{
                
            }
            food.rDisAppear.Subscribe(_ => DebugLog());
        }


        void foodDisappear(){

        }


        public void pinched(){
            this.isPinched = true;
        }


        public void anPinched(){
            this.isPinched = false;
        }
    }
}

