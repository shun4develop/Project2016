using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using MyClass;
using MyLibrary;

public class ConfirmationButton : MonoBehaviour {

	public GameObject detail;
	public AnimationUI Complete;

	public void contentDelete(){
		LoadingManager.run ();
		ViewerController vc = GameObject.Find ("System").GetComponent<ViewerController>();
		Item item = detail.GetComponent<DetailInfoCanvas>().item;

		Action<string> positive_func = (string text) => {
			LoadingManager.stop();
			ItemData.instance.deleteContentById(item.getId());
			vc.contentsUpdate ();
			Complete.popUp();
		};

		Action negative_func = () => {
			LoadingManager.stop();
		};
		WebManager.instance.contentsDump (positive_func, negative_func, item.getId(), item.getTitle());
	}

	public void contentSave(){
		LoadingManager.run ();
		Item item = detail.GetComponent<DetailInfoCanvas>().item;
		Action<string> positive_func = (string text) => {
			LoadingManager.stop();
			ItemData.instance.saveContent(item);
			Complete.popUp();
		};

		Action negative_func = () => {
			LoadingManager.stop();
		};
		WebManager.instance.contentsTaken (positive_func, negative_func, item.getId(), item.getTitle());
	}
}
