using UnityEngine;
using System.Collections;
//Example Script for motion (Walk, jump and dying), for dying press 'k'...
public class Example_Motion_Attack_Controller : MonoBehaviour {
	private float maxspeed; //walk speed
	Animator anim;
	private bool faceright; //face side of sprite activated
	private bool jumping=false;
	private bool isdead=false;
	//public bool run=false;
	private bool attack=false;
	private string aux="";
	//--
	void Start () {
		maxspeed=2f;//Set walk speed
		faceright=true;//Default right side
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("walk", false);//Walking animation is deactivated
		anim.SetBool ("dead", false);//Dying animation is deactivated
		anim.SetBool ("jump", false);//Jumping animation is deactivated
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		//if (coll.gameObject.tag == "Ground"){//################Important, the floor Tag must be "Ground" to detect the collision!!!!
			jumping=false;
			anim.SetBool ("jump", false);
		//}
	}
	
	void Update () {
		//-- Attack animation off
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("attacking")) {
			
		} else {
			anim.SetBool ("attack", false);
		}
		//--
		//Debug.Log ("+---- " + aux);

		//--
		if(isdead==false){
			//--DYING
			if(Input.GetKey ("k")){//###########Change the dead event, for example: life bar=0
				anim.SetBool ("dead", true);
				isdead=true;
			}
			//--END DYING

			//--JUMPING
			if (Input.GetMouseButtonDown(0)){
					anim.SetBool ("attack", true);
					anim.Play("attacking", -1, 0f);
			}
			if (Input.GetButtonDown("Jump")){
				if(jumping==false){//only once time each jump
					GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,200));
					jumping=true;
					anim.SetBool ("jump", true);
				}
			}
			//--END JUMPING
			
			//--WALKING
			float move = Input.GetAxis ("Horizontal");
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxspeed, GetComponent<Rigidbody2D>().velocity.y);
			//--

			if(move>0){//Go right
				anim.SetBool ("walk", true);//Walking animation is activated
				if(faceright==false){
					Flip ();
				}
			}
			if(move==0){//Stop
				anim.SetBool ("walk", false);
			}			
			if((move<0)){//Go left
				anim.SetBool ("walk", true);
				if(faceright==true){
					Flip ();
				}
			}
			//END WALKING
		}
	}
	
	void Flip(){
		faceright=!faceright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


			


}
