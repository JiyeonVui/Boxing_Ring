using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelAsset", fileName = "LevelAsset")]
public class LevelAsset : ScriptableObject
{
    public ModeType modeType;

    public List<LevelContent> levelsList = new List<LevelContent>();

    public LevelContent GetLevel(int levelIndex)
    {
        //if(levelIndex > levelsList.Count -1)
        //{
        //    int index = RandomLevel();
        //    DataManager.Instance.GetData<DataLevel>().UpdateCurrentLevel( ModeType.DEFAULT, index); 
        //    return levelsList[index];
        //}
        DataManager.Instance.GetData<DataLevel>().UpdateCurrentLevel(modeType, levelIndex);
        return levelsList[levelIndex];
    }

    public int GetCount()
    {
        return levelsList.Count;
    }

    #region DEBUG

#if UNITY_EDITOR
    // private void OnValidate()
    // {
    //     bonusIndexList.Clear();
    //     normalIndexList.Clear();
    //     foreach (var item in levelsList)
    //     {
    //         if (item.isBonusLevel)
    //         {
    //             bonusIndexList.Add(levelsList.IndexOf(item) + 1);
    //         }
    //         else
    //         {
    //             normalIndexList.Add(levelsList.IndexOf(item) + 1);
    //         }
    //     }
    // }
#endif

    #endregion
}


[System.Serializable]
public class LevelContent
{
    //public Level level;
    public Sprite spriteReview;
}
