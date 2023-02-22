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
	
	// Speed variables
	private float   speed = 10f,
					speedHalved = 7.5f,
					speedOrigin = 10f;
	
	// Jump!
	private float distToGround;
	void Start()
	{
		anim = GetComponent<CharacterAnimation>(); 
	}
    
    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 
        MouseLook(); 
        PlayerMove(horizontal,vertical); 
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
	
	private void PlayerMove(float h, float v)
	{
		if (h != 0f || v != 0f) // If horizontal or vertical are pressed then continue
		{
			if(h != 0f && v != 0f) // If horizontal AND vertical are pressed then continue
			{
				speed = speedHalved; // Modify the speed to adjust for moving on an angle
			}
			else // If only horizontal OR vertical are pressed individually then continue
			{
				speed = speedOrigin; // Keep speed to it's original value
            }
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (transform.right * h) * speed * Time.deltaTime); 
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (transform.forward * v) * speed * Time.deltaTime); 
			anim._animRun = true;
		}
		else 	
		{
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
