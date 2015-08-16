using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {

	// Player handling
	public float gravity = 20;
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float jumpHeight = 12;
	public float slideDecceleration = 10;

	private float initiateSlideThreshold = 9;

	private float animationSpeed;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	//States
	private bool jumping;
	private bool sliding;
	private bool stopSliding;

	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;



	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
		animator = GetComponent<Animator> ();
		manager = Camera.main.GetComponent<GameManager> ();
		animator.SetLayerWeight (1, 1);
	}
	
	// Update is called once per frame
	void Update () {

		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}

		if(playerPhysics.grounded){
			amountToMove.y = 0;

			if(jumping){
				jumping = false;
				animator.SetBool("Jumping",false);
			}

			if(sliding){
				if(Mathf.Abs (currentSpeed) < .25f || stopSliding){
					stopSliding = false;
					sliding = false;
					animator.SetBool ("Sliding",false);
					playerPhysics.ResetCollider();
				}
			}

			//Jump Input
			if (Input.GetButtonDown ("Jump")) {
				if(sliding){
					stopSliding = true;
				}
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool ("Jumping",true);
			}

			//Slide Input
			if(Input.GetButtonDown ("Slide")){
				if(Mathf.Abs (currentSpeed) > initiateSlideThreshold){
					sliding = true;
					animator.SetBool ("Sliding",true);
					targetSpeed = 0;

					playerPhysics.SetCollider (new Vector3(10.3f,1.5f,3), new Vector3(.35f,.75f,0));
			
				}
			}
		}

		animationSpeed = IncrementTowards (animationSpeed, Mathf.Abs (targetSpeed), acceleration);
		animator.SetFloat("Speed", animationSpeed);
		
		//Input
		
		if (!sliding) {
			targetSpeed = Input.GetAxisRaw ("Horizontal") * walkSpeed;
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);
			
			// Face Direction
			float moveDir = 1;
			if (moveDir != 0) {
				transform.eulerAngles = (moveDir > 0) ? (Vector3.up * 180) : Vector3.zero;
			}
		} 
		else {
			currentSpeed = IncrementTowards (currentSpeed,targetSpeed,slideDecceleration);
		}
	
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);



	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Checkpoint") {
			manager.SetCheckpoint (c.transform.position);
		}
		if (c.tag == "Finish") {
			manager.EndLevel ();
		}

	}

	//Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a){
		if (n == target){
			return n;
		}
		else{
			float dir = Mathf.Sign (target - n); //must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target-n))? n: target; //If n has now passed target then return target, otherwise return n
		}
	}
}
