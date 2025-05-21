using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public int indexLevel;
    public int currentLevel;
    public ModeType modeType;

    public LevelData (ModeType modeType , int index, int currentLevel)
    {
        this.modeType = modeType;
        this.indexLevel = index;
        this.currentLevel = currentLevel; 
    }
}



[System.Serializable]
public class LevelSave
{
    public List<LevelData> levelDatas = new List<LevelData>();

    public LevelSave()
    {
        levelDatas = new List<LevelData>();
    }
}




public class DataLevel : GameData
{


    [SerializeField]
    private LevelSave levelSave;


    public LevelSave LevelSave { get => levelSave; set => levelSave = value; }



    public override void SaveData()
    {
        DataManager.Instance.SaveData<LevelSave>(GetType().FullName, LevelSave);
    }

    public override void LoadData()
    {
        levelSave = DataManager.Instance.LoadData<LevelSave>(GetType().FullName);
    }

    public override void NewData()
    {
        levelSave = new LevelSave();
        //List<ModeContent> ListModes = DataManager.Instance.GetAsset<ModeAsset>().GetModes();

        //for (int i = 0; i < ListModes.Count; i++)
        //{
        //    ModeContent data = ListModes[i];
        //    if (data != null)
        //    {
        //        levelSave.levelDatas.Add(new LevelData(data.modeType, 0, 0)); 
        //    }
        //}

        
        SaveData();
    }

    public int GetLevel(ModeType modeType)
    {
        int index = levelSave.levelDatas.FindIndex(x => x.modeType == modeType);
        return index != -1 ? levelSave.levelDatas[index].indexLevel : -1;
    }


    public int GetCurrentLevel(ModeType modeType)
    {
        int index = levelSave.levelDatas.FindIndex(x => x.modeType == modeType);
        return index != -1 ? levelSave.levelDatas[index].currentLevel : -1;
    }


    public void UpdateLevel(ModeType modeType)
    {
        int index = levelSave.levelDatas.FindIndex(x => x.modeType == modeType);
        if(index != -1)
        {
            int currentLevel = levelSave.levelDatas[index].currentLevel < LevelController.Instance.GetCount(modeType) - 1 ?
                ++levelSave.levelDatas[index].currentLevel :
                RandomLevel(levelSave.levelDatas[index].currentLevel, modeType);
            levelSave.levelDatas[index].indexLevel++;
            levelSave.levelDatas[index].currentLevel = currentLevel; 
            SaveData();
        }

    }

    private int RandomLevel(int currentLevel, ModeType modeType)
    {

        int level = currentLevel;
        int index = 0;
        do
        {
            index = UnityEngine.Random.Range(0, LevelController.Instance.GetCount(modeType));
        } while (index == level);

        return index;
    }

    public void UpdateCurrentLevel(ModeType modeType , int level)
    {
        int index = levelSave.levelDatas.FindIndex(x => x.modeType == modeType);
        if (index != -1)
        {
            levelSave.levelDatas[index].currentLevel = level;
            SaveData();
        }
    }
}