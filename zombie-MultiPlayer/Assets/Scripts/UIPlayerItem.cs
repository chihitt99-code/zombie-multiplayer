using TMPro;
using UnityEngine;

public class UIPlayerItem : MonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text isMasterClientText;
    
    public void Setup(string nickname, bool isMasterClient)
    {
        nicknameText.text = nickname;
        if (isMasterClient)
        {
            isMasterClientText.text = "방장";
            
        }
        else
        {
            isMasterClientText.text = "플레이어";
        }
    }
}
