using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerHitbox : MonoBehaviour {
	BoxCollider col;

	//on awake add this event to an events list
	void Awake(){
		col = GetComponent<BoxCollider> ();
	}

}
