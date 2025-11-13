using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class UIScrollView : MonoBehaviour
{
    public GameObject thereIsNoRoomGo;
    public Transform content;
    public GameObject uiRoomCellViewPrefab;


    public void Init()
    {
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>((int)EventEnums.EventType.OnRoomListUpdate,
            //포톤이 방 목록을 보내올 때 실행 될 함수를 등록하는 코드
            //이벤트를 종류별로 구분하기 위해 이넘을 사용하고 변화시키면 key 값으로 쓸 수 있다
            (type, data) => //이벤트 종류type 와 이벤트에서 전달된 data (여기서는 방 목록 )
            {
                Debug.Log(data.Count); // 방 개수를 체크하고 
                thereIsNoRoomGo.SetActive(data.Count == 0); // 방 개수가 0이면 방없어요 오브젝트를 활성화
            });
    }
    public void UpdateUI()
    {
        

    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
