using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ui List",menuName = "SO/ Ui List")]
public class UIContainerSO : ScriptableObject{

    public List<UI> keyList;
    public List<GameObject> uiList;

    
    public enum UI{
     SettingUI,
    }

    public Dictionary<UI, GameObject> uiListD;

    /// <summary>
    /// Fills uiList Dictionary
    /// </summary>
    public void init(){
        short _itr = 0;
        foreach(UI ui in Enum.GetValues(typeof(UI))){
            uiListD[ui] = uiList[_itr];
            _itr++;
        }
    }
}
