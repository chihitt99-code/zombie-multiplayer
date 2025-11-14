using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMain : MonoBehaviourPun //MonoBehaviourPun을 상속받으면 photon 뷰를 바로 쓸수있음
{

    public enum ReadyState
    {
        Start, // 준비시작
        Complete // 준비완료
    }
    
    private ReadyState readyState = ReadyState.Start;
    
    public UIPlayerList uiPlayerList;
    public Button readyButton;
    public Button startButton;
    public Button leaveButton;
    public TMP_Text readyButtonText;
    
    private List<Player> playerList = new List<Player>();

    private void Awake() 
    {
        EventDispatcher.instance.AddEventHandler<Player>(
            (int)EventEnums.EventType.OnPlayerEnteredRoom,
            OnPlayerEnteredRoomEvent);
        
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom,
            OnJoinedRoomEvent);
        
        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnPlayerLeftRoom,OnPlayerLeftRoomEvent);
        leaveButton.onClick.AddListener(() =>
        {
           
            Pun2Manager.instance.LeaveRoom();
   
        });
        
        readyButton.onClick.AddListener(() =>
        {
            
            if (readyState == ReadyState.Start)
            {
                readyState = ReadyState.Complete;
                UpdateReadyButtonByState();
                photonView.RPC("Ready",RpcTarget.MasterClient,PhotonNetwork.LocalPlayer.ActorNumber);
                //PhotonView.RPC (string methodName, PhotonTargets targets, params object[] parameters)
                //이때는 방장의 스타트 버튼이 눌림
               


            }
            else
            {
                readyState = ReadyState.Start;
                
                UpdateReadyButtonByState();
                photonView.RPC("CancelReady",RpcTarget.MasterClient,PhotonNetwork.LocalPlayer.ActorNumber);
            }
        });
        
       
 
        
    }

    private void OnPlayerLeftRoomEvent(short eventType)
    {
        uiPlayerList.UpdateUI( PhotonNetwork.CurrentRoom.Players.Values.ToList());
        
        UpdateReadyAndStartButton();
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
       
       UpdateReadyAndStartButton();

    }

    private void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer);
        
        playerList.Add(PhotonNetwork.LocalPlayer);
        UpdateReadyAndStartButton();

        UpdateReadyButtonByState(); //상태에 따라서 레디버튼을 업데이트한다 라는 메서드

        if (PhotonNetwork.IsMasterClient) //내가 마스터 클라이언트라면
        {
            //내 playerItem 을만든다
            uiPlayerList.UpdateUI(PhotonNetwork.CurrentRoom.Players.Values.ToList());
        }

    }

    private void UpdateReadyButtonByState()
    {
        switch(readyState)
        {
            case ReadyState.Start:
                readyButtonText.text = "Ready Start";
                break;
            case ReadyState.Complete:
                readyButtonText.text = "Ready Complete";
                break;
        }
    }
    
    private void UpdateReadyAndStartButton()
    {
        HideReadyAndStartButton(); //전부 비활성화 시킨 상태에서
        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //나 (마스터) 면 스타트버튼만 보이게
            readyButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            startButton.interactable = false;
        }
        else
        {
            //other 이면 레디만 보이게
            readyButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            
        }

        //방에 한명이상 있어야되고 마스터를 제외한 모든 플레이어가 레디버튼을 눌러야만
        //마스터클라이언트의 스타트 버튼이 interactable 가 true 가 되어야 함
        
        /*// 내가 방장이고 ~ 플레이어가 나혼자라면
        if (PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length <= 1)
        {
            startButton.interactable = false;
        }*/

    }
  

    public void HideReadyAndStartButton()
    {
        readyButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
       EventDispatcher.instance.RemoveEventHandler((int)EventEnums.EventType.OnPlayerEnteredRoom);
       EventDispatcher.instance.RemoveEventHandler((int)EventEnums.EventType.OnJoinedRoom);
    }
    
    [PunRPC]
    public void Ready(int actorNumber)
    {
        var player = PhotonNetwork.CurrentRoom.Players[actorNumber];
        Debug.Log($"{player.NickName}이 준비 했습니다.");
       
    }

    [PunRPC]
    public void CancelReady(int actorNumber)
    {
        var player = PhotonNetwork.CurrentRoom.Players[actorNumber];
        Debug.Log($"{player.NickName}이 준비를 취소했습니다.");
    }
}
