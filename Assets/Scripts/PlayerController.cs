using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CharacterAnimation anim;
	private float   rotY,
					rotX,
					sensitivity = 10.0f;

	private float speed = 10f;
					
	private float distToGround;
	private Rigidbody rd;

	void Start()
	{
		anim = GetComponent<CharacterAnimation>();
		rd = GetComponent<Rigidbody>();
	}
    
    void FixedUpdate ()
    {
        
        MouseLook(); 
        PlayerMove(); 
        Jump(); 
    }
    private void MouseLook()
	{
		rotX += Input.GetAxis("Mouse X")*sensitivity; 
		rotY += Input.GetAxis("Mouse Y")*sensitivity; 
		rotY = Mathf.Clamp (rotY, -90f, 90); 
		transform.localEulerAngles = new Vector3(0,rotX,0); 
		Camera.main.transform.localEulerAngles = new Vector3(-rotY,0,0); 
	}
	
	private void PlayerMove()
	{
		float horizontal = Input.GetAxis("Horizontal"); 
		float vertical = Input.GetAxis("Vertical"); 
		
		if (horizontal != 0f || vertical != 0f) // If horizontal or vertical are pressed then continue
		{
			Vector3 movement = transform.forward*vertical + transform.right*horizontal;
			rd.AddForce (movement * speed);
			anim._animRun = true;
		}
		else 	
		{
			rd.velocity = Vector3.zero;
			anim._animRun = false; 
		}
	}

	private void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			if(IsGrounded()) 
			{
				GetComponent<Rigidbody>().velocity += 5f * Vector3.up; 
			}
		}
	}
	
	private bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f); 
	}
	
    
}
