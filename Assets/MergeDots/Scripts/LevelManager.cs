using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.PlayerPrefs;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelManager instance;

    public int levelNumber
    {
        get => GetInt("MyLevelNumer");
        set => SetInt("MyLevelNumer",value);
    }
    void Awake()
    {
        instance = this;
    }
    
    
}
