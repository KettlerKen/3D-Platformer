using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class WalkController : Controller {

	//The camera used for viewing
	public Transform cam;

	//movement information
	Vector3 walkVelocity;
	Vector3 prevWalkVelocity;
	Quaternion camForward;
	float adjVertVelocity;
	float adjBumpVelocity;
	float jumpPressTime;
	float buttonPressTime;
	float camPressTime;
	float x;
	float y;
	float horizontal;

	//settings
	public float walkSpeed = 3.0f;
	public float jumpSpeed = 6f;
	public float bumpSpeed = 3f;
	public float turningSpeed = 500.0f;

	void Start() {
		cam = GetComponent<Transform>();
	}

	public override void ReadInput (InputData data)
	{
		prevWalkVelocity = walkVelocity;
		ResetMovementToZero ();

		//set vertical movemenet based on input
		if (data.axes [0] != 0f) {
			walkVelocity += transform.forward * data.axes [0] * walkSpeed;
			newInput = true;
		}

		//set horizontal rotation based on input
		if (data.axes [1] != 0f) {
			if(buttonPressTime != 0){
				horizontal += data.axes[1] * turningSpeed * Time.deltaTime;
			}
			newInput = true;
		}

		//set horizontal movemenet based on input
		if (data.axes [2] != 0f) {
			walkVelocity += transform.right * data.axes [2] * walkSpeed;
			newInput = true;
		}

		//BUTTON 0
		//set vertical jump
		if (data.buttons [0] == true) {
			if (jumpPressTime == 0f) {
				if (Grounded()) {
					adjVertVelocity = jumpSpeed;
				}
			}
			jumpPressTime += Time.deltaTime;
			newInput = true;
		} else {
			jumpPressTime = 0f;
		}

		//BUTTON 1
		//ActionCam disable to look around
		if (data.buttons [1] == true) {
			buttonPressTime += Time.deltaTime;
			newInput = true;
		}
		else {
			buttonPressTime = 0f;
		}
	}


	//method that look below character and chekc for collider
	//uses multiple rays for all corners of chars hitbox, better detection
	bool Grounded(){
		bool isGrounded = false;
		Vector3 forwardL = new Vector3(coll.bounds.extents.x,0,coll.bounds.extents.z);
		Vector3 forwardR = new Vector3(coll.bounds.extents.x,0,coll.bounds.extents.z * -1f);
		Vector3 backL = new Vector3(coll.bounds.extents.x * -1f,0,coll.bounds.extents.z);
		Vector3 backR = new Vector3(coll.bounds.extents.x * -1f,0,coll.bounds.extents.z * -1f);

		isGrounded = isGrounded || Physics.Raycast (transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
		isGrounded = isGrounded || Physics.Raycast (transform.position + forwardL, Vector3.down, coll.bounds.extents.y + 0.1f);
		isGrounded = isGrounded || Physics.Raycast (transform.position + forwardR, Vector3.down, coll.bounds.extents.y + 0.1f);
		isGrounded = isGrounded || Physics.Raycast (transform.position + backL, Vector3.down, coll.bounds.extents.y + 0.1f);
		isGrounded = isGrounded || Physics.Raycast (transform.position + backR, Vector3.down, coll.bounds.extents.y + 0.1f);

		return isGrounded;
	}

	void LateUpdate(){
		if(!newInput){
			prevWalkVelocity = walkVelocity;
			ResetMovementToZero ();
			jumpPressTime = 0f;
			buttonPressTime = 0f;
			camPressTime = 0f;
		}

		//Get camera rotation and update player
		if(newInput){
			if (camPressTime == 0f) {
				Vector3 relativePos = cam.position - transform.position;
				rb.transform.rotation = Quaternion.LookRotation (relativePos);
				camPressTime += 1f;
			} else {
				camPressTime += 1f;
				horizontal += Input.GetAxis ("Mouse X") * 120.0f * 5.0f * 0.02f;
			}
		}

		//Apply other transformations
		rb.transform.Rotate(0, horizontal, 0);
		rb.velocity = (new Vector3 (walkVelocity.x, rb.velocity.y + adjVertVelocity, walkVelocity.z + adjBumpVelocity));
		newInput = false;
	}

	void ResetMovementToZero(){
		horizontal = 0f;
		walkVelocity = Vector3.zero;
		adjVertVelocity = 0f;
		adjBumpVelocity = 0f;
	}

}
