using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Master;
using UnityEngine;

public class SavedDataHandler : Singleton<SavedDataHandler>
{
    [Header("Data Credentials")] public string password;

    [Header("Current Save Data")] public SaveData _saveData;

    [Header("Default Data")] public SaveData _DefaultSaveData;

    public override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(this);
        _saveData = SaveGameData.Load(_DefaultSaveData, password);
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGameData.Save(_saveData, password);
        }
        else
        {
            _saveData = SaveGameData.Load(_DefaultSaveData, password);
        }
    }
    [EasyButtons.Button]
    void SaveData()
    {
        SaveGameData.Save(_saveData, password);
    }
    [EasyButtons.Button]
    public void ResetToDefault()
    {
        _saveData = SaveGameData.Clear(_DefaultSaveData, password);
    }

    public void SetFirstLaunch()
    {
        if (!_saveData.isFirstLaunch)
            _saveData.isFirstLaunch = true;
    }

    public void AddSculp(string num, string title , string introId)
    {
        _saveData.mySculptures.Add(new MySculpture(num,title, introId));
    }
    public void AddTrail(string num, string title, string introId)
    {
        _saveData.myTrails.Add(new MyTrail(num, title, introId));
    }
}

[Serializable]
public class SaveData
{
    public bool isFirstLaunch;
    public bool isSoundOn;
    public bool isMusicOn;
    public int currentLanguage;
    public string currentLanguageId;
    public List<MySculpture> mySculptures;
    public List<MyTrail> myTrails;

    public SaveData()
    {
        mySculptures = new List<MySculpture>();
        myTrails = new List<MyTrail>();
    }
}

[Serializable]
public class MySculpture
{
    public string Num;
    public string Title;
    public string IntroId;
    public bool IsVisited;
    public MySculpture(string num,string title,string introId)
    {
        Num = num;
        Title = title;
        IntroId = introId;
        IsVisited = false;
    }
}

[Serializable]
public class MyTrail
{
    public string Num;
    public string Title;
    public string IntroId;
    public bool IsVisited;
    public MyTrail(string num, string title, string introId)
    {
        Num = num;
        Title = title;
        IntroId = introId;
        IsVisited = false;
    }
}