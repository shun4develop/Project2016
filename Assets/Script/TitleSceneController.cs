using UnityEngine;
using System.Collections;
using System;
using MyLibrary;
using MyClass;

public class TitleSceneController : MonoBehaviour {
	public AutoLogin autoLoginController;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (3f);

		autoLoginController.autoLogin ();

		while (!autoLoginController.inquiryCompleted) {
			yield return new WaitForSeconds (0.5f);
		}
		if (autoLoginController.success) {
			AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Map");
			yield return async;
		} else {
			AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Auth");
			yield return async;
		}
	}
}
