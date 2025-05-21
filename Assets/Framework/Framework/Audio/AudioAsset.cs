using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    BG_MUSIC = 1,
    CLICK,
    Praise,
    Complete,
    DrawPaint
}
 

[System.Serializable]
public class AudioContent
{
    public AudioType audioType;
    public List<AudioClip> audioClips = new List<AudioClip>(); 
}


[CreateAssetMenu]
public class AudioAsset : ScriptableObject
{
    [System.Serializable]
    public struct AudioData
    {
        public AudioType audioType;
        public AudioClip audioClip;

        public AudioData(AudioType audioType, AudioClip audioClip)
        {
            this.audioType = audioType;
            this.audioClip = audioClip;
        }
    }
    public AudioData[] audioDataSet = new AudioData[0];
    public List<AudioContent> audioRandomAsset = new List<AudioContent>(); 

    private Dictionary<AudioType, AudioClip> dicClips = new Dictionary<AudioType, AudioClip>();


    public void InitDic()
    {
        foreach (var item in audioDataSet)
        {
            if (!dicClips.ContainsKey(item.audioType))
            {
                dicClips.Add(item.audioType, item.audioClip);
            }
        }
    }


    public AudioClip GetClip(AudioType audioType)
    {
        if (dicClips.ContainsKey(audioType))
        {
            return dicClips[audioType];
        }
        Debug.LogErrorFormat("Missing audio type :{0}", audioType);
        return null;
    }


    public AudioClip GetRandomClip(AudioType audioType)
    {
        int index = audioRandomAsset.FindIndex(x => x.audioType == audioType); 
        if(index != -1)
        {
            return audioRandomAsset[index].audioClips[Random.Range(0, audioRandomAsset[index].audioClips.Count)]; 
        }

        return null; 
    }
}
