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
//		#if UNITY_ANDROID
//		Debug.Log("ANDROID");
//		#elif UNITY_IOS
//		Debug.Log("IOS");
//		#elif UNITY_EDITOR
//		Debug.Log("UNITY");
//		#endif

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
	
	// Update is called once per frame
	void Update () {

	}

	public void Stop(){
		webcamTexture.Stop();
	}
}
