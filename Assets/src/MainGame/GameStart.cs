using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Awake() { }

    void Start()
    {
        ConfigManager.LoadAllConfig();

        var buff = ConfigManager.GetConfig<BuffAttr>("1022");

        var all_buff = ConfigManager.GetConfig<Sheet1>();

        foreach (var item in all_buff)
        {
            var sheel = item.Value;
            Debug.Log(sheel.Chinese + "-" + sheel.English);
        }
    }


}