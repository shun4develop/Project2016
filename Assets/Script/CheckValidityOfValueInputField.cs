using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(InputField))]
public class CheckValidityOfValueInputField : MonoBehaviour {
	public Text userName;
	public Text log;
	public Image plane;
	private string tmpStr;
	public bool IsChecked{ get; set;}
	public void ValueChange(string str){
		IsChecked = false;
		tmpStr = str;
		StartCoroutine (wait(str));
	}
	private IEnumerator wait(string str){
		yield return new WaitForSeconds (0.8f);
		if (str.Length < 3 && str.Length != 0) {
			log.text = "ユーザ名は3文字以上の長さが必要です";
			plane.color = new Color (255, 0, 0, 0.1f);
			yield break;
		}else if(str.Length == 0){
			log.text = "";
			plane.color = new Color (0, 0, 0, 0);
			yield break;
		}

		if (tmpStr == str) {
			Action<string> find_func = (string text) => {
				log.text = "このユーザ名はすでに使用されています";
				plane.color = new Color (255, 0, 0, 0.1f);
			};
			Action not_find_func = () => { 
				log.text = "このユーザ名は利用可能です";
				plane.color = new Color (0, 255, 0, 0.1f);
				IsChecked = true;
			};
			WebManager.instance.findUserName (find_func, not_find_func, tmpStr);
		}

	}
}
