using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;


public class NetManager:MonoBehaviour
{
    private Socket ClientSocket;

    private static NetManager instance;
    public static NetManager Instance { get => instance; set => instance = value; }

    private Dictionary<KeyValuePair<long,long>,HandlerBase> ProctoolDic;

    public void Start()
    {
        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        ClientSocket.Connect(ConnectInfo.ipAddress, ConnectInfo.Port);

        instance = this;

        Debug.Log("NetManager初始化完成");
    }

    public void Revceive()
    {
        if (ClientSocket.Connected == false || ClientSocket.Poll(10, SelectMode.SelectRead))
        {
            return;
        }
        else
        {
            //接收协议编号
            byte[] ProtocolNumber_byte = new byte[8];
            ClientSocket.Receive(ProtocolNumber_byte, 0, ProtocolNumber_byte.Length, SocketFlags.None);
            long ProtocolNumber = BitConverter.ToInt64(ProtocolNumber_byte, 0);

            //接收数据大小
            byte[] ClientDataLength = new byte[8];
            ClientSocket.Receive(ClientDataLength, 0, ClientDataLength.Length, SocketFlags.None);
            long ClientDataSize = BitConverter.ToInt64(ClientDataLength, 0);

            byte[] ClientData = new byte[ClientDataSize];
            if (ClientDataSize > 0)
            {
                ClientSocket.Receive(ClientData, 0, ClientData.Length, SocketFlags.None);
            }
            else
            {
                ClientData = new byte[0];
            }
            string content = Encoding.UTF8.GetString(ClientData, 0, (int)ClientDataSize);

            Handler(ProtocolNumber, content);
        }
    }

    public void SendtoServer(long ProtocolNumber, AsyncCallback callback = null)
    {
        byte[] ClientData = new byte[0];

        //包装数据大小
        byte[] ClientDataLength = BitConverter.GetBytes((long)ClientData.Length);
        ClientData = ClientDataLength.Concat(ClientData).ToArray();
        //包装协议编号
        byte[] ProtocolNumber_byte = BitConverter.GetBytes(ProtocolNumber);
        ClientData = ProtocolNumber_byte.Concat(ClientData).ToArray();
        //包装id
        byte[] UserIdNumber_byte = BitConverter.GetBytes(111);
        ClientData = UserIdNumber_byte.Concat(ClientData).ToArray();

        ClientSocket.BeginSend(ClientData, 0, ClientData.Length, SocketFlags.None, callback, null);

        Revceive();
    }

    public void SendtoServer<T>(long ProtocolNumber,T t, AsyncCallback callback = null)
    {
        string json = JsonConvert.SerializeObject(t);
        var ClientData = Encoding.UTF8.GetBytes(json); //字节类型的内容

        //包装数据大小
        byte[] ClientDataLength = BitConverter.GetBytes((long)ClientData.Length);
        ClientData = ClientDataLength.Concat(ClientData).ToArray();
        //包装协议编号
        byte[] ProtocolNumber_byte = BitConverter.GetBytes(ProtocolNumber);
        ClientData = ProtocolNumber_byte.Concat(ClientData).ToArray();
        
        ClientSocket.BeginSend(ClientData, 0, ClientData.Length, SocketFlags.None, callback, null);

        Revceive();
    }

    public void Handler(long ProtocolNumber, string jsonStr)
    {
        HandlerBase handler = null;
        foreach(var it in ProctoolInfo.ProctoolDic)
        {
            long limit_left = it.Key.Key;
            long limit_right = it.Key.Value;

            if (ProtocolNumber>= limit_left && limit_right> ProtocolNumber)
            {
                handler = it.Value;
            }
        }
        if (handler == null)
        {
            Debug.LogError("handler么有找到");
            return;
        }
        handler.Handler(ProtocolNumber, jsonStr);
    }

    public void OnDestroy()
    {
        ClientSocket.Shutdown(SocketShutdown.Both);
        ClientSocket.Close();
    }
}


