using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ÓÎÏ·Æô¶¯Âß¼­
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
