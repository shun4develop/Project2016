using UnityEngine;
using System.Collections;

public class Gyro : MonoBehaviour {

	Camera mainCamera;

	// Use this for initialization
	void Start () {
		gyroOn ();
	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.AngleAxis(90.0f,Vector3.right)*Input.gyro.attitude*Quaternion.AngleAxis(180.0f,Vector3.forward);

	}

	public void gyroOn(){
		Input.gyro.enabled = true;
	}

	public void gyroOff(){
		Input.gyro.enabled = false;
	}



}
