using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebCameraController : MonoBehaviour {

	public WebCamTexture webcamTexture{ get; set; }

	public int camWidth_px = 1280;
	public int camHeight_px = 720;

	public Camera targetCamera;

	float height;
	float width;

	string s;

	public GameObject text;

	void Start () {
		#if UNITY_ANDROID
		s = "Aondroid";
		webCamera();
		#endif
		#if UNITY_IOS
		s = IosCamera.test();
		iosTester();
		webCamera();
		#endif
		#if UNITY_EDITOR
		s = "webCam";
		webCamera();
		#endif
	}

	void iosTester(){
		text.GetComponent<Text> ().text = s;
	}

	void webCamera(){

		text.GetComponent<Text> ().text = s;

		// Quad をカメラのサイズに合わせる
		transform.localScale = new Vector3(Screen.width, Screen.height, 1);

		targetCamera = Camera.main;
		height = targetCamera.orthographicSize * 2;
		width = height * targetCamera.aspect;

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			transform.localScale = new Vector3 (height, (width + 1) * -1, 1);
		} else if (Application.platform == RuntimePlatform.Android) {
			transform.localScale = new Vector3 (height, width + 1, 1);
		} else {
			transform.localScale = new Vector3 (width, height, 1);
		}

		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length < 1) {
			return;
		}

		var euler = transform.localRotation.eulerAngles;

		webcamTexture = new WebCamTexture(devices[0].name);

		if(Application.platform == RuntimePlatform.IPhonePlayer||Application.platform == RuntimePlatform.Android){
			transform.localRotation = Quaternion.Euler( euler.x, euler.y, euler.z - 90 );
		}

		GetComponent<MeshRenderer> ().material.mainTexture = webcamTexture;

		webcamTexture.Play();
	}

	public void Stop(){
		webcamTexture.Stop();
	}
}
