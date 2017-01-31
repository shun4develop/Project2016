using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//禁止ワードを制限することになったら使うやーつ

[RequireComponent(typeof(InputField))]
public class CheckProhibitedWordInputField: MonoBehaviour {
	private InputField inputField;
	public bool isPass{ 
		get{ 
			return isPass&&isCheck;
		} 
		set{ 
		}
	} 
	private bool isCheck;

	public static string[] words = {"死ね","天野","殺す","コロ助なり"};

	void Start(){
		isPass = false;
		isCheck = false;
		inputField = GetComponent <InputField> ();
	}

	public void OnValueChange()
	{
		isCheck = false;
		foreach(string word in words){
			if (!inputField.text.Contains (word)) {
				isPass = false;
				break;
			}
		}
	}
}
