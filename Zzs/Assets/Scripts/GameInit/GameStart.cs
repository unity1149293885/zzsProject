using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ϸ�����߼�
public class GameStart : MonoBehaviour
{
    async void Awake()
    {
        if (GameConfig.isConnectNet)
        {
            NetManager.InitNet();
        }
        else
        {
            await XMLTools.LoadUser();
        }
    }

    private void OnDestroy()
    {
        if (GameConfig.isConnectNet)
        {
            NetManager.CloseNet();
        }
    }
}
