public enum EventType
{
    //登录事件
    UpdateLoginState = 1,//更新登录状态

    //更新通用文本弹窗
    UpdateMessageBox = 10,

    //打开物品详情界面
    OpenItemInfo = 20,

    //更新主界面
    UpdateMainPanel = 30,

    //上下架更新
    ChangeItemState = 40,

    //产品种类加载完毕
    LoadedItemType = 50,
}
