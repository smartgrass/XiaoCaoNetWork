using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField IDInput;
    public InputField PasswordInput;

    public Button LoginBtn;

    private void Start()
    {
        IDInput.contentType = InputField.ContentType.IntegerNumber;
        LoginBtn.onClick.AddListener(OnLogin);
        IDInput.text = "123";
        PasswordInput.text = "123";
    }

    private void OnLogin()
    {
        int PlayerId = int.Parse(IDInput.text);
        string pass = PasswordInput.text;

        //TcpClientTool tcpClient = new TcpClientTool(PlayerId.ToString(), PlayerId);
        TcpClientTool tcpClient =  gameObject.AddComponent<TcpClientTool>();
        PlayerManager.Instance.selfClient = tcpClient;
        //�����߳�ֻ������һ��
        tcpClient.Login(PlayerId, pass);

    }
}
