using UnityEngine;
using System.Collections;

public class WebCameraController : MonoBehaviour {

	public WebCamTexture webcamTexture{ get; set; }

	public int camWidth_px = 1280;
	public int camHeight_px = 720;

	public Camera targetCamera;

	float height;
	float width;

	void Start () {

		// Quad をカメラのサイズに合わせる
		transform.localScale = new Vector3(Screen.width, Screen.height, 1);

		WebCamDevice[] devices = WebCamTexture.devices;
		var euler = transform.localRotation.eulerAngles;

		webcamTexture = new WebCamTexture(devices[0].name);

//		Debug.Log ("screen->"+Screen.height+"/"+Screen.width);

		//webcamTexture = new WebCamTexture(camWidth_px,camHeight_px);

		if(Application.platform == RuntimePlatform.IPhonePlayer||Application.platform == RuntimePlatform.Android){
			transform.localRotation = Quaternion.Euler( euler.x, euler.y, euler.z - 90 );
		}

		GetComponent<MeshRenderer> ().material.mainTexture = webcamTexture;

		webcamTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log ("web->"+webcamTexture.height+"/"+webcamTexture.width);
		targetCamera = Camera.main;

		//transform.localScale = new Vector3(Screen.height, Screen.width, 1);

		height = targetCamera.orthographicSize * 2;
		width = height * targetCamera.aspect;

		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			transform.localScale = new Vector3 (height, width+1, 1);
		} else {
			transform.localScale = new Vector3 (width, height, 1);
		}


	}

	public void Stop(){
		webcamTexture.Stop();
	}
}
