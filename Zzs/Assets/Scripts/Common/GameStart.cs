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

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        NetManager.CloseNet();
    }
}
