using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public void ChangeScene(string sceneName){
		StartCoroutine (changeScene(sceneName));
	}
	IEnumerator changeScene(string sceneName){
		AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
		yield return async;
	}
}
