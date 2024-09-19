using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpenPanelType
{
    RegiesterUser = 1,
    ItemManage = 2,
}
public class PanelManager : MonoBehaviour
{
    public GameObject regeisterPanel;
    public GameObject ItemManagePanel;

    private static Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();

    void Start()
    {
        dic.Add(1, regeisterPanel);
        //dic.Add(2, ItemManagePanel);
    }

    public static void OpenPanel(OpenPanelType type)
    {
        foreach(var t in dic)
        {
            if ((int)type != t.Key)
            {
                t.Value.gameObject.SetActive(false);
            }
            else
            {
                t.Value.gameObject.SetActive(true);
            }
        }
    }
    
}
