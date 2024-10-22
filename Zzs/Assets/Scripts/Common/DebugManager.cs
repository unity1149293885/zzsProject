using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public GameObject DebugPanel;

    public void Start()
    {
        DebugPanel.gameObject.SetActive(GameConfig.isOpenDebug);
    }
}
