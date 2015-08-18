using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	private Transform target;
	private float trackSpeed = 15;
	private Camera _camera;
	private new Transform transform;
	private float _targetOrthographicSize;
	private float _preferredOrthographicSize;
	private float _maxOrthographicSize;
	private float _neutralVerticalOffset = 0;

	private float zoom{ get { return _preferredOrthographicSize / _camera.orthographicSize; } }
	private DigitalAverage _heightAverage = new DigitalAverage ();
	private bool _pinnedToTop = false;
	private PlayerController _playerController;

	void Start()
	{
		_camera = Camera.main;
		transform = _camera.transform;
		_preferredOrthographicSize = Screen.height / 2;
		_maxOrthographicSize = _preferredOrthographicSize * 2; 
		_camera.orthographicSize = _preferredOrthographicSize;

		_playerController = player.GetComponent<PlayerController> ();
	}

	public void SetTarget(Transform t){
		target = t;
		transform.position = new Vector3 (t.position.x + 20.0f, t.position.y, transform.position.z);
	}


	void LateUpdate(){
		if (target) {
			float x = IncrementTowards (transform.position.x, target.position.x + 5.0f,trackSpeed);
			float y = IncrementTowards (transform.position.y, target.position.y,trackSpeed);
			transform.position = new Vector3(x,y,transform.position.z);
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
