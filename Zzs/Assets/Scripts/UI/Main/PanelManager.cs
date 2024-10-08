using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum OpenPanelType
{
    RegiesterUser = 1,
    ItemManage = 2,
    ItemDetailInfo = 3,
}
public class PanelManager : MonoBehaviour
{
    //注册界面
    public GameObject regeisterPanel;
    //成员管理
    public GameObject ItemManagePanel;
    //详情信息
    public GameObject DetailPanel;

    private static Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();

    void Start()
    {
        dic.Add(1, regeisterPanel);
        dic.Add(2, ItemManagePanel);
        dic.Add(3, DetailPanel);
    }

    public static void OpenPanel(OpenPanelType type)
    {
        foreach (var panel in dic)
        {
            if ((int)type != panel.Key)
            {
                if (panel.Value != null)
                {
                    panel.Value.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("请检查界面类型对应的界面，界面类型：" + type);
                }

            }
            else
            {
                panel.Value.gameObject.SetActive(true);
                
            }
        }
    }
    public static void OpenDetailPanel( ItemInfo info )
    {
        var type = OpenPanelType.ItemDetailInfo;
        foreach (var panel in dic)
        {
            if ((int)type != panel.Key)
            {
                if(panel.Value!= null)
                {
                    panel.Value.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("请检查界面类型对应的界面，界面类型：" + type);
                }
               
            }
            else
            {
                ItemDetailPanel.Instance.UpdateItemDetailPanel(info);
                panel.Value.gameObject.SetActive(true);
            }
        }
    }
    
}
