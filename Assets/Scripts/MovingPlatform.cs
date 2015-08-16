using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	public float speed = 2;

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * speed * Time.deltaTime);
	}
}
