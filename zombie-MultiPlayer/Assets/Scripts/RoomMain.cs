using System;
using Photon.Pun;
using UnityEngine;

public class RoomMain : MonoBehaviour
{
    public UIPlayerList uiPlayerList;

    private void Awake() //룸에 조인을 했다면 이벤트를 받아서
    {
        // 방 입장 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom,
            OnJoinedRoomEvent);
    }
    
    private void OnJoinedRoomEvent(short eventType) //처리한다
    {
        Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
        //PhotonNetwork.LocalPlayer.NickName
   
    }
}
