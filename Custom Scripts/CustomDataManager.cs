using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CustomControls {
    public static class CustomDataManager {
     // Data
     public static ControlType controlType {
         get=> controlType;
         set {
             controlType = value;
             Save();
         }
     }

     public static SteerPos steerPos {
         get => steerPos;
         set {
             steerPos = value;
             Save();
         }
     }
     
     private  const string PLAYERPREF_KEY = "CUSTOMDATA";

     public static Data GetData() {
         Data data = new Data();
         string data_str = PlayerPrefs.GetString(PLAYERPREF_KEY);
         if (data_str == String.Empty) return Data.DefaultData();
         data = JsonUtility.FromJson<Data>(data_str);
         return data;
     }

     private static void Save() {
         Data data = new Data {controlType = controlType, steerPos = steerPos};
         string str= JsonUtility.ToJson(data);
         PlayerPrefs.SetString(PLAYERPREF_KEY,str);
         return;
     }
    
    }

    public class Data {
        public ControlType controlType;
        public SteerPos steerPos;

        public static Data DefaultData() => new Data 
            {controlType = ControlType.SteeringControl, steerPos = SteerPos.LeftSide};
    }
}
