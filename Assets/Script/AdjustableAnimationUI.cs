using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]

public class AdjustableAnimationUI : MonoBehaviour {
	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	public float time;
	public float delay;

	public bool VirticalAnchorMinAnimation = false;
	public float toValueVirticalAnchorMin;

	public bool VirticalAnchorMaxAnimation = false;
	public float toValueVirticalAnchorMax;

	public iTween.EaseType MOVE_TYPE = iTween.EaseType.easeInOutBounce;

	private bool move;
	void Awake(){
		rectTransform = GetComponent<RectTransform> ();
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	public void animation () {
		if (VirticalAnchorMaxAnimation)
			moveVirticalAnchorMax (toValueVirticalAnchorMax);

		if (VirticalAnchorMinAnimation)
			moveVirticalAnchorMin (toValueVirticalAnchorMin);
	}
	void Update(){
		if (move) {
			canvasGroup.interactable = false;
		} else {
			canvasGroup.interactable = true;
		}
	}
	private void complete(){
		move = false;
	}
	public bool isMove(){
		return move;
	}

	public void moveVirticalAnchorMin(float toValue){
		move = true;
		Hashtable hash = new Hashtable ();
		hash.Add ("delay",delay);
		hash.Add ("from",rectTransform.anchorMin.y);
		hash.Add ("to",toValue);
		hash.Add ("time",time);
		hash.Add ("easeType",MOVE_TYPE);
		hash.Add ("oncompletetarget",gameObject);
		hash.Add ("oncomplete","complete");

		hash.Add ("onupdate","setVirticalAnchorMin");

		iTween.ValueTo (gameObject,hash);
	}
	private void setVirticalAnchorMin(float value){
		rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x,value);
	}
	private void setVirticalAnchorMax(float value){
		rectTransform.anchorMax = new Vector2 (rectTransform.anchorMax.x, value);
	}
	public void moveVirticalAnchorMax(float toValue){
		move = true;
		Hashtable hash = new Hashtable ();
		hash.Add ("delay",delay);
		hash.Add ("from",rectTransform.anchorMax.y);
		hash.Add ("to",toValue);
		hash.Add ("time",time);
		hash.Add ("easeType",MOVE_TYPE);
		hash.Add ("oncompletetarget",gameObject);
		hash.Add ("oncomplete","complete");

		hash.Add ("onupdate","setVirticalAnchorMax");

		iTween.ValueTo (gameObject,hash);
	}
	private void setHorizonAnchorMax(float value){
		rectTransform.anchorMax = new Vector2 (value,rectTransform.anchorMax.y);
	}
	private void setHorizonAnchorMin(float value){
		rectTransform.anchorMax = new Vector2 (value,rectTransform.anchorMin.y);
	}
}
