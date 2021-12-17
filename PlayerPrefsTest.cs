using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerPrefsTest : MonoBehaviour {
    // Player Prefas Test
    private PlayerData playerData;
    string playerDataStr = String.Empty;
    private void Start() {
        // testing
        //PlayerPrefs.DeleteAll();
        playerDataStr  = PlayerPrefs.GetString("PLAYERDATA_KEY");
        

        if (playerDataStr == String.Empty) {
            print("Empty");
            string jasonStr = JsonUtility.ToJson(new PlayerData{number = 123456,name = "UNKOWN"});
            PlayerPrefs.SetString("PLAYERDATA_KEY" , jasonStr);
            
            string _str = PlayerPrefs.GetString("PLAYERDATA_KEY");
            playerData = JsonUtility.FromJson<PlayerData>(_str);
            
            print(playerData.ToString() + "  - FINAL");
            return;
        }
        
        print(playerDataStr);
        print(JsonUtility.FromJson<PlayerData>(playerDataStr).ToString());
        
    }
}

public class PlayerData {
    public int number;
    public string name;

    public override string ToString() => $"{number},{name}";

    public static PlayerData GetSample(){
        PlayerData data = new PlayerData {number = 12345, name = "UnKown"};
        return data;
    }

    public static PlayerData GetPlayerData(string playerPref) {
        var res = playerPref.Split(',');
        Debug.Log(res[0]  + res[1]);
        PlayerData newData = new PlayerData {number = int.Parse(res[0]), name = res[1]};
        return newData;
    }
}
