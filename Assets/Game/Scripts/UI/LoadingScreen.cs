using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoSingleton<LoadingScene>
{
    [SerializeField] protected Image bar;
    //[SerializeField] protected RectTransform logo;
    //[SerializeField] protected RectTransform image;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private GameObject _board;
    private string sceneName = String.Empty;
    private int sceneIndex;

    private void OnEnable()
    {
        Vector3 vectorLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 vectorRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        //image.DOAnchorPosX(vectorLeft.x - image.sizeDelta.x / 2, 0f);
        //logo.DOAnchorPosX(vectorRight.x + logo.sizeDelta.x / 2, 0f);
    }

    private void Start()
    {
        //image.DOAnchorPosX(0, 1f);
        //logo.DOAnchorPosX(0, 1f);
        DataUser dataUser = DataManager.Instance.GetData<DataUser>();
        AudioManager.Instance.SetSound(dataUser.HasSound());
        AudioManager.Instance.SetMusic(dataUser.HasMusic());
        LoadScene("GameScene", 4f);
    }
    public void LoadScene(string name, float time = 1, Action callback = null)
    {
        sceneName = name;
        Loading(callback, time);
    }
    public void LoadScene(int index, float time = 1, Action callback = null)
    {
        sceneIndex = index;
        Loading(callback, time);
    }
    private void Loading(Action callback, float time)
    {
        bar.fillAmount = 0;
        AsyncOperation asyncOperation;
        if (String.IsNullOrEmpty(sceneName))
            asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        else
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        bar.DOFillAmount(1, time).OnComplete(() => StartCoroutine(CompleteSlider(callback, asyncOperation))).OnUpdate(() =>
        {
            percentText.text = $"{(int)(bar.fillAmount * 100)}%";

        }).OnComplete(() =>
        {
            asyncOperation.allowSceneActivation = true;
        });

    }
    IEnumerator CompleteSlider(Action callback, AsyncOperation asyncOperation)
    {
        _board.gameObject.SetActive(false);
        yield return new WaitUntil(() => asyncOperation.progress >= 0.9f);
        //AppOpenAdManager.instance.ShowAdIfAvailable(() =>
        //{
        //    AdsManager.instance.HideMRECs();

        //    AdsManager.instance.SetDone();
        //    AppOpenAdManager.instance.isStart = false;
        //    asyncOperation.allowSceneActivation = true;
        //    callback?.Invoke();
        //    // _screenIntroduce.SetHideAction(() =>
        //    // {
        //    GoogleMobileAdsManager.instance.LoadBannerAds(GoogleMobileAds.Api.AdPosition.Bottom);
        //    // });
        //});
    }

}
