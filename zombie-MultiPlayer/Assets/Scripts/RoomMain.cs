using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMain : MonoBehaviour
{
    public UIPlayerList uiPlayerList;
    
    private List<Player> playerList = new List<Player>();

    private void Awake() 
    {
        EventDispatcher.instance.AddEventHandler<Player>(
            (int)EventEnums.EventType.OnPlayerEnteredRoom,
            OnPlayerEnteredRoomEvent);
        
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom,
            OnJoinedRoomEvent);
 
        
    }


    private void OnJoinedRoomEvent(short eventType)
    {
        var me = PhotonNetwork.LocalPlayer; //내꺼

        // 나보다 먼저 들어와 있던 사람들 출력
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p == me) continue; //내꺼면 컨티뉴
            Debug.Log($"[{p.NickName}]님이 입장 했습니다."); // 내가 아닌사람만 남음
            playerList.Add(p);
            
        }
        
        
        foreach (var p in playerList)
        {
            Debug.Log($"<color=yellow>{p.NickName}</color>");
        }
        
        //lim , hong 순서 바꾸기
        playerList.Clear();
        Player[]sorted = new Player[PhotonNetwork.PlayerList.Length];
        PhotonNetwork.PlayerList.CopyTo(sorted, 0);

        Array.Sort(sorted, (a, b) =>
        {
            //a가 b보다 앞에 와야 한다면 음수 
            if (a == PhotonNetwork.MasterClient && b != PhotonNetwork.MasterClient) return -1;
            //뒤에 와야하면 양수
            if (b == PhotonNetwork.MasterClient && a != PhotonNetwork.MasterClient) return 1;
            //같으면 0
            return a.ActorNumber.CompareTo(b.ActorNumber);
        });
        
        playerList.AddRange(sorted);
        foreach (var p in playerList)
        {
            Debug.Log($"<color=lime>{p.NickName}</color>");
        }
        //클라이언트일 경우 이곳에서 UIPlayerItem을 만든다
        uiPlayerList.Init(playerList);



    }
    
    private void OnPlayerEnteredRoomEvent(short eventType, Player newPlayer)
    {
       Debug.Log($"newPlayer: {newPlayer}");
       playerList.Add(newPlayer);
       
       
       //마스터일 경우 이곳에서 PlayerItem을 만든다
     
       foreach (var p in playerList)
       {
           Debug.Log($"<color=yellow>{p.NickName}</color>");
       } 
       //마스터일 경우 이곳에서 UIPlayerItem을 만든다
       uiPlayerList.Init(playerList);

    }

    private void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer);
        
        playerList.Add(PhotonNetwork.LocalPlayer);
        
        
    }
 
}
