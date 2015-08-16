using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	public GameObject ragdoll;

	public void TakeDamage(float damage){
		health -= damage;
		if (health <= 0) {
			Die ();
		}
	}

	public void Die(){
		Ragdoll r = (Instantiate (ragdoll,transform.position,transform.rotation)as GameObject).GetComponent<Ragdoll>();
		r.CopyPose (this.transform);
		Destroy (this.gameObject);
	}
}
