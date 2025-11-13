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
            isMasterClientText.text = "Master";
            
        }
        else
        {
            isMasterClientText.text = "Client";
        }
    }
}
