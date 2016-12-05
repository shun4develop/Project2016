using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LogoutButton : MonoBehaviour{
	void Start(){
		this.gameObject.GetComponent<Button> ().onClick.AddListener (()=>logout());
	}
	private void logout(){
		PlayerPrefs.DeleteKey ("user_name");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Auth");
	}
}
