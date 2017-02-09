using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(RectTransform))]
public class ScrollControllWithElementSizeOfChildren : MonoBehaviour{

	public RectTransform[] contents;

	private RectTransform rect;
	private bool move = false;


	void Start(){
		rect = GetComponent<RectTransform> ();
	}
	//スクロール位置をコントロールするためのもの
	//シーン上のスクロールビューは無限にスクロールする設定にしておいて
	//実際はこのメソッドを呼び出すことでスクロールを制限している。
	public void fit(){
		float posY = rect.localPosition.y;
		float sizeOfChildElement = getContentsSizeDelta ();

		if (posY < -1 || sizeOfChildElement < 0) {
			scroll (0);
		} else if (posY > sizeOfChildElement+1) {
			scroll (sizeOfChildElement);
		} 
	}
	//サイズが可変である子要素のサイズの合計
	private float getContentsSizeDelta(){
		float size = 0;
		foreach(RectTransform rt in contents){
			size += rt.sizeDelta.y;
		}
		return size;
	}
	//指定の位置にスクロールする
	private void scroll(float val){
		rect.localPosition = new Vector2(0,val);
	}
	//バグるので使うなら頑張って。
//	public void fitWithAnimation ()
//	{
//		if (GetComponent<iTween> () == null && !move) {
//			if (rect.localPosition.y < 0) {
//				move = true;
//				Hashtable hash = new Hashtable ();
//				hash.Add ("from", rect.anchoredPosition.y);
//				hash.Add ("to", 0);
//				hash.Add ("time", 0.2f);
//				hash.Add ("easeType", iTween.EaseType.linear);
//				hash.Add ("onupdate", "scroll");
//				hash.Add ("oncompletetarget", gameObject);
//				hash.Add ("oncomplete", "fitComplete");
//
//				iTween.ValueTo (gameObject, hash);
//			} else if (rect.localPosition.y > getContentsSizeDelta ()) {
//				move = true;
//				Hashtable hash = new Hashtable ();
//				hash.Add ("from", rect.localPosition.y);
//				hash.Add ("to", getContentsSizeDelta ());
//				hash.Add ("time", 0.2f);
//				hash.Add ("easeType", iTween.EaseType.linear);
//				hash.Add ("onupdate", "scroll");
//				hash.Add ("oncompletetarget", gameObject);
//				hash.Add ("oncomplete", "fitComplete");
//
//				iTween.ValueTo (gameObject, hash);
//			} else {
//				move = false;
//			}
//		}
//	}


	//AnimationScrollに使う予定だった残骸
//	private void fitComplete(){
//		move = false;
//	}
}
