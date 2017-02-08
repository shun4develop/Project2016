using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinchInOut : MonoBehaviour {

	public bool touchFlag = false;

	NavMeshAgent agent;

	float vMin = -10.0f;
	float vMax = 10.0f;

	//直前の2点間の距離.
	private float backDist = 0.0f;
	//初期値
	//float view = 60.0f;
	float v = 1.0f;

	Vector2 startPos;
	RectTransform rectTrans;

	Vector2 max;
	Vector2 min;


	void Start(){
		startPos = transform.position;
		rectTrans = GetComponent <RectTransform> ();
	}

	// Update is called once per frame
	void Update () {

		if(rectTrans.sizeDelta.x == 0)
			startPos = transform.position;

		if (touchFlag) {
			if (rectTrans.offsetMax.y <= 0) {
				rectTrans.anchoredPosition = new Vector2 (rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y + 5f);
			}
			if (rectTrans.offsetMax.x <= 0) {
				rectTrans.anchoredPosition = new Vector2 (rectTrans.anchoredPosition.x + 5f, rectTrans.anchoredPosition.y);
			}
			if (rectTrans.offsetMin.x >= 0) {
				rectTrans.anchoredPosition = new Vector2 (rectTrans.anchoredPosition.x - 5f, rectTrans.anchoredPosition.y);
			}
			if (rectTrans.offsetMin.y >= 0) {
				rectTrans.anchoredPosition = new Vector2 (rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y - 5f);
			}
			if (Input.touchCount == 1 && rectTrans.sizeDelta.x > 0) {
				Touch touch = Input.GetTouch (0);

				Vector2 tmpPos = rectTrans.anchoredPosition;

				if (rectTrans.offsetMax.y > 0 && rectTrans.offsetMin.y < 0&& rectTrans.offsetMax.x > 0 && rectTrans.offsetMin.x < 0) {
					rectTrans.anchoredPosition = new Vector2 (rectTrans.anchoredPosition.x + touch.deltaPosition.x, rectTrans.anchoredPosition.y + touch.deltaPosition.y);
				}
			}
			// マルチタッチかどうか確認
			if (Input.touchCount >= 2) {
				// タッチしている２点を取得
				Touch t1 = Input.GetTouch (0); 
				Touch t2 = Input.GetTouch (1);

				//2点タッチ開始時の距離を記憶
				if (t2.phase == TouchPhase.Began) {
					backDist = Vector2.Distance (t1.position, t2.position);
				} else if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved) {
					// タッチ位置の移動後、長さを再測し、前回の距離からの相対値を取る。
					float newDist = Vector2.Distance (t1.position, t2.position);
					//view = view + (backDist - newDist) / 100.0f;
					v = v + (newDist - backDist) / 0.1f;

					// 限界値をオーバーした際の処理
					if (v > vMax) {
						v = vMax;
					} else if (v < vMin) {
						v = vMin;
					}

					// 相対値が変更した場合、カメラに相対値を反映させる
					if (v != 0) {
						//transform.localScale = new Vector3 (v, v, 1.0f);
						if(backDist<newDist && rectTrans.sizeDelta.x <= 400){
							rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x + v*2f, rectTrans.sizeDelta.y + v*2f);
						}
						if(backDist>newDist && rectTrans.sizeDelta.x > 0){
							rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x + v*2f, rectTrans.sizeDelta.y + v*2f);
						}
						backDist = newDist;
					}
				}
			}
		}
	}

	public void trueTouchFlag(){
		touchFlag = true;
	}

	public void falseTouchFlag(){
		touchFlag = false;
		rectTrans.sizeDelta = new Vector2(0,0);
		transform.position = startPos;
	}
}
