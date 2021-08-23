using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public int Score;
    public float Time;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string Json)
    {
        JsonUtility.FromJsonOverwrite(Json, this);
    }

}
