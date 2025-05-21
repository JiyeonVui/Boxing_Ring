using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

    public bool IsUnlockColor = false;
    public ModeType currentMode;
    public LevelController levelController;

    public void Startgame(ModeType modeType)
    {
        AudioManager.Instance.PlayMusic(AudioType.BG_MUSIC);
        LoadLevel(modeType);
    }


    public void LoadLevel(ModeType modeType)
    {
        currentMode = modeType;
        LoadCurrentLevel();
    }
   

    public void ReplayLevel()
    {
        IsUnlockColor = false;
        int indexLevel = DataManager.Instance.GetData<DataLevel>().GetCurrentLevel(currentMode);
        LoadLevel(indexLevel);
    }


    public void NextLevel()
    {
        DataManager.Instance.GetData<DataLevel>().UpdateLevel(currentMode);
        LoadCurrentLevel();
    }


    private void LoadCurrentLevel()
    {
        IsUnlockColor = false;
        int indexLevel = DataManager.Instance.GetData<DataLevel>().GetCurrentLevel(currentMode);
        // Debug.LogErrorFormat("current lv: {0}", indexLevel);
        if (PlayerPrefs.GetInt("currentlv", 0) < indexLevel + 1)
        {
            PlayerPrefs.SetInt("currentlv", indexLevel + 1);
            PlayerPrefs.Save();

            //AnalyticsManager.instance.LogLevelStartEvent(indexLevel + 1);
        }


        LoadLevel(indexLevel);
    }


    private void LoadLevel(int level)
    {
        levelController.OnLevelLoad(level, currentMode);
    }

}
