using Photon.Pun;
using UnityEngine;


public class Woman : MonoBehaviourPun
{
    public float speed = 1f;
    public float rotSpeed = 1f;
    
    private Animator animator;
   
    
    void Start()
    {
        animator = GetComponent<Animator>();
       
       
    }

   
    void Update()
    {

        if (photonView.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
           
            Vector3 playerMove = new Vector3(h, 0, v);
            if (playerMove != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerMove,Vector3.up);
                transform.rotation = newRotation;
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                animator.SetInteger("State",1);
               
              
            }
            else
            {
                animator.SetInteger("State",0);
            }

        }
        
       
       
        
            
        
        
        
    }
}
