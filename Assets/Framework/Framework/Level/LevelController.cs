using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{

    //[SerializeField] private Level level;
    public List<LevelAsset> levelAssets;
    public Transform tfLevelContainer;

    public void OnLevelLoad(int levelId,ModeType modeType )
    {
        DestroyCurrentLevel();

        //LevelContent content = levelAsset.GetLevel(levelId);

        //UIManager.Instance.GetScreen<ScreenInGame>().SetData(content.spriteReview); 
        //level = PoolManager.Instance.SpawnObject(content.level.transform, Vector3.zero, Quaternion.identity, tfLevelContainer).GetComponent<Level>();
        //if (level != null)
        //{
        //    level.OnInit();
        //}

        //LevelContent content = levelAssets.Find(x => x.modeType == modeType).GetLevel(levelId);

        ////UIManager.Instance.GetScreen<ScreenInGame>().SetData(content.spriteReview, modeType);
        //level = PoolManager.Instance.SpawnObject(content.level.transform, Vector3.zero, Quaternion.identity, tfLevelContainer).GetComponent<Level>();
        //if (level != null)
        //{
        //    level.OnInit(modeType, levelId);
        //}

    }

    public int GetCount(ModeType modeType)
    {
        return levelAssets.Find(x => x.modeType == modeType).levelsList.Count;
    }


    public void DestroyCurrentLevel()
    {

        //if (level != null)
        //{
        //    level.Clear();
        //    Destroy(level.gameObject);
        //    level = null;
        //}
    }

    public void OnLevelStart()
    {
    }


    #region DEBUG
    #endregion
}
