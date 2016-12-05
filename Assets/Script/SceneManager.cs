using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public void ChangeScene(string sceneName){
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
	}
}
