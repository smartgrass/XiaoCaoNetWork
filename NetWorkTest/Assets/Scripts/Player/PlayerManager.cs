﻿
using UnityEngine;
using Player;
using System.Collections.Generic;

public class PlayerManager : MonoSingleton<PlayerManager>,IGameUpdate
{
    public AllPosMsg allPosMsgLast = new AllPosMsg();
    public  AllPosMsg allPosMsg = new AllPosMsg();

    public bool IsUpdatePos = false;
    public PlayerModel selfPlayer;
    public TcpClientTool selfClient;
    public Dictionary<int, PlayerModel> allPlayerDic = new Dictionary<int, PlayerModel>();
    

    public void SetAllPlayer(AllPosMsg _allPosMsg)
    {
        allPosMsgLast = _allPosMsg;
        allPosMsg = _allPosMsg;
    }


    public void CreatSelfPlayer(int playerID)
    {
        GameObject game = Instantiate(Resources.Load(PrefabsPath.PlyaerPrefab) as GameObject);
        Debug.Log("Creat player");
        selfPlayer = game.AddComponent<PlayerComponent>().Init(playerID);
        selfPlayer.client = selfClient;
        allPlayerDic.Add(playerID, selfPlayer);
        IsUpdatePos = true;
    }
    public int CreatOtherPlayer(int playerID)
    {
        GameObject game = Instantiate(Resources.Load(PrefabsPath.PlyaerPrefab) as GameObject);
        game.name = ("otherPlayer"+ playerID);
        Debug.Log("Creat other player");
        var otherPlayer = game.AddComponent<PlayerComponent>().Init(playerID);
        allPlayerDic.Add(playerID, otherPlayer);
        return playerID;
    }


    public void GameUpdate()
    {
        if (IsUpdatePos)
        {
            int len = allPosMsg.PosPlayerMsgList.Count;
            Debug.Log("yns  all pos len " + len);
            for (int i = 0; i < len; i++)
            {
                var pos = allPosMsg.PosPlayerMsgList[i];
                var lastPos = allPosMsgLast.PosPlayerMsgList[i];
                if (allPlayerDic.ContainsKey(pos.PlayerId))
                {
                    if (selfPlayer.playerID != pos.PlayerId)
                        allPlayerDic[pos.PlayerId].SetPos(pos.ToVec3(), lastPos.ToVec3());
                }
                else
                {
                    //添加新玩家
                    CreatOtherPlayer(pos.PlayerId);
                    allPlayerDic[pos.PlayerId].SetPos(pos.ToVec3(), lastPos.ToVec3());

                }
            }
            selfPlayer.SendPos();
        }
    }

}