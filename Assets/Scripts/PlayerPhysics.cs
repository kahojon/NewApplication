using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;

	private BoxCollider collider;
	private Vector3 s;
	private Vector3 c;

	private Vector3 originalSize;
	private Vector3 originalCentre;
	private float colliderScale;

	private int collisionDivisionX = 3;
	private int collisionDivisionY = 10;


	private float skin = 0.015f;

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;

	private Transform platform;
	private Vector3 platformPositionOld;
	private Vector3 deltaPlatformPos;

	Ray ray;
	RaycastHit hit;

	void Start(){
		collider = GetComponent<BoxCollider> ();
		colliderScale = transform.localScale.x;
		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider (originalSize, originalCentre);
	}

	public void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;

		if (platform) {
			deltaPlatformPos = platform.position - platformPositionOld;
		} else {
			deltaPlatformPos = Vector3.zero;
		}

		grounded = false;
		for (int i = 0; i < collisionDivisionX; i++) {
			float dir = Mathf.Sign (deltaY);
			float x = (p.x + c.x - s.x/2) + s.x/(collisionDivisionX-1)*i; //left, centre and then rightmost point of collider
			float y = p.y + c.y + s.y/2 * dir;

			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			if(Physics.Raycast (ray,out hit,Mathf.Abs (deltaY) + skin,collisionMask)){

				platform = hit.transform;
				platformPositionOld = platform.position;
				float dst = Vector3.Distance (ray.origin,hit.point);

				//Stop player's downwards movement after coming within skin width of a collider
				if(dst > skin){
					deltaY = -dst - skin * dir;
				}
				else{
					deltaY = 0;
				}
				grounded = true;
				break;
			}
			else{
				platform = null;
			}

		}

		movementStopped = false;
		// Check collisions left and right
		for (int i = 0; i < collisionDivisionY; i++) {
			float dir = Mathf.Sign (deltaX);
			float x = p.x + c.x + s.x/2 * dir; //left, centre and then rightmost point of collider
			float y = p.y + c.y - s.y/2 + s.y/(collisionDivisionY-1) * i;
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
			if(Physics.Raycast (ray,out hit,Mathf.Abs (deltaX) + skin,collisionMask)){
				
				float dst = Vector3.Distance (ray.origin,hit.point);
				
				//Stop player's downwards movement after coming within skin width of a collider
				if(dst > skin){
					deltaX = -dst - skin * dir;
				}
				else{
					deltaX = 0;
				}
				movementStopped = true;
				break;
			}
			
		}

		if (!grounded && !movementStopped) {
			Vector3 playerDir = new Vector3 (deltaX, deltaY);
			Vector3 o = new Vector3 (p.x + c.x + s.x / 2 * Mathf.Sign (deltaX), p.y + c.y + s.y / 2 * Mathf.Sign (deltaY));
			ray = new Ray (o, playerDir.normalized);

			if (Physics.Raycast (ray, Mathf.Sqrt (deltaX * deltaX + deltaY * deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX + deltaPlatformPos.x, deltaY);
		transform.Translate (finalTransform,Space.World);
	}

	public void SetCollider(Vector3 size, Vector3 centre){
		collider.size = size;
		collider.center = centre;

		s = size * colliderScale;
		c = centre * colliderScale;
	}

	public void ResetCollider(){
		SetCollider (originalSize, originalCentre);
	}
}
