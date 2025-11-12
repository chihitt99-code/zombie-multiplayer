using System.Numerics;
using Photon.Pun;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Woman : MonoBehaviourPun
{
    public float speed = 1f;
    public float rotSpeed = 1f;
    
    private Animator animator;
    private CharacterController characterController;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
       
    }

   
    void Update()
    {

        if (!photonView.IsMine)
            return;
        
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
