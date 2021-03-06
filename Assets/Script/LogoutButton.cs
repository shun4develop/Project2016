﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyManagers;
[RequireComponent(typeof(Button))]
public class LogoutButton : MonoBehaviour{
	void Start(){
		this.gameObject.GetComponent<Button> ().onClick.AddListener (()=>logout());
	}
	private void logout(){
		SaveDataManager.deleteData ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Auth");
	}
}
