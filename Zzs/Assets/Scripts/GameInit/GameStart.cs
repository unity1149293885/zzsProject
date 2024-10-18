using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ϸ�����߼�
public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NetManager.InitNet();
    }

    private void OnDestroy()
    {
        NetManager.CloseNet();
    }
}
