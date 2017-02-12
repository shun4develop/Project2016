using UnityEngine;
using System.Collections;
using System;
using MyLibrary;
using MyClass;

public class TitleSceneController : MonoBehaviour {
	public AutoLogin autoLoginController;
	public AdjustableAnimationUI ui;
	// Use this for initialization
	IEnumerator Start () {

		autoLoginController.autoLogin ();

		yield return new WaitWhile(()=> Application.isShowingSplashScreen == true);
		ui.animation ();
		yield return new WaitForSeconds (3f);

		yield return new WaitWhile(()=> autoLoginController.inquiryCompleted == false);

		if (autoLoginController.success) {
			AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Map");
			yield return async;
		} else {
			AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Auth");
			yield return async;
		}
	}
}
