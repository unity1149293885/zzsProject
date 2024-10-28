using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.UI;

// 检测更新并下载资源
public class CheckUpdateAndDownload : MonoBehaviour
{
    /// <summary>
    /// 显示下载状态和进度
    /// </summary>
    public Text updateText;

    /// <summary>
    /// 重试按钮
    /// </summary>
    public Button retryBtn;

    void Start()
    {
        retryBtn.gameObject.SetActive(false);
        retryBtn.onClick.AddListener(() =>
        {
            StartCoroutine(DoUpdateAddressadble());
        });

        // 默认自动执行一次更新检测
        //StartCoroutine(DoUpdateAddressadble());

        checkDown();
    }

    public void checkDown()
    {
        // 检查是否有新版资源目录可用并进行自动更新
        Addressables.CheckForCatalogUpdates(true).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (handle.Result.Count > 0)
                {
                    var updateHandle = Addressables.UpdateCatalogs(handle.Result, true);


                    if (updateHandle.Status != AsyncOperationStatus.Succeeded)
                    {
                        OnError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                        return;
                    }

                    // 更新列表迭代器
                    List<IResourceLocator> locators = updateHandle.Result;
                    foreach (var locator in locators)
                    {
                        List<object> keys = new List<object>();
                        keys.AddRange(locator.Keys);
                        // 获取待下载的文件总大小
                        var sizeHandle = Addressables.GetDownloadSizeAsync(keys.GetEnumerator());

                        if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                        {
                            OnError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                            return;
                        }

                        long totalDownloadSize = sizeHandle.Result;
                        updateText.text = updateText.text + "\ndownload size : " + totalDownloadSize;
                        Debug.Log("download size : " + totalDownloadSize);
                        if (totalDownloadSize > 0)
                        {
                            // 下载
                            var downloadHandle = Addressables.DownloadDependenciesAsync(keys, true);
                            while (!downloadHandle.IsDone)
                            {
                                if (downloadHandle.Status == AsyncOperationStatus.Failed)
                                {
                                    OnError("DownloadDependenciesAsync Error\n" + downloadHandle.OperationException.ToString());
                                    return;
                                }
                                // 下载进度
                                float percentage = downloadHandle.PercentComplete;
                                Debug.Log($"已下载: {percentage}");
                                updateText.text = updateText.text + $"\n已下载: {percentage}";
                                
                            }
                            if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                            {
                                Debug.Log("下载完毕!");
                                updateText.text = updateText.text + "\n下载完毕";
                            }
                        }
                    }
                }
                else
                {
                    updateText.text = updateText.text + "\n没有检测到更新";
                }
                // 进入游戏
                EnterGame();
            }
            else if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Catalog update failed: " + handle.OperationException.Message);
            }
        };
    }
    IEnumerator DoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // 检测更新
        var checkHandle = Addressables.CheckForCatalogUpdates(true);
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            OnError("CheckForCatalogUpdates Error\n" + checkHandle.OperationException.ToString());
            yield break;
        }

        
    }

    // 异常提示
    private void OnError(string msg)
    {
        updateText.text = updateText.text + $"\n{msg}\n请重试! ";
        // 显示重试按钮
        retryBtn.gameObject.SetActive(true);
    }


    // 进入游戏
    void EnterGame()
    {
        // TODO
        Debug.LogError("加载完成");
    }
}
