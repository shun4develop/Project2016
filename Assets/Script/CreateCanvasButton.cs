using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyLibrary;
using MyClass;
[RequireComponent(typeof(Button))]

/// <summary>
/// ボタンにクリックイベントを登録
/// </summary>

public class CreateCanvasButton : MonoBehaviour {
	
	CanvasCreatorBase cc;

	void Start(){
		cc = GetComponent<CanvasCreatorBase> ();
		// ボタンの処理を追加
		gameObject.GetComponent<Button>().onClick.AddListener (() => {
			if(GetComponent<ContentsViewerBase>().showCompleted){
				Item item = GetComponent<ContentsViewerBase>().Item;
				cc.create(item);
			}
		});
	}
}
