﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foods;
using Bottles;
namespace Players{
    public struct PlayerData
    {
        public List<BottleData> bottleList;
        public Dictionary<FoodKind,int> foodMap;
        public int money;

    }
}

