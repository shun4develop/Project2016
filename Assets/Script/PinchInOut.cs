using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinchInOut : MonoBehaviour {

	public bool touchFlag = false;

	NavMeshAgent agent;

	float vMin = 1.0f;
	float vMax = 5.0f;

	//直前の2点間の距離.
	private float backDist = 0.0f;
	//初期値
	float view = 60.0f;
	float v = 1.0f;

	Vector2 startPos;
	RectTransform rectTrans;

	Vector2 max;
	Vector2 min;

	public GameObject text;
	Text tex;

	void Start(){
		startPos = transform.position;
		rectTrans = GetComponent <RectTransform> ();
		tex = text.GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {

		tex.text = rectTrans.offsetMax.ToString () +"\n"+ rectTrans.offsetMin.ToString ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x+1, rectTrans.sizeDelta.y+1);
			Debug.Log( rectTrans.offsetMax );
			Debug.Log( rectTrans.offsetMin );
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x-1, rectTrans.sizeDelta.y-1);
			Debug.Log( rectTrans.offsetMax );
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (rectTrans.offsetMax.y > 0 && rectTrans.offsetMin.y < 0 && rectTrans.offsetMax.x > 0 && rectTrans.offsetMin.x < 0) {
				rectTrans.position = new Vector2 (transform.position.x - 1, transform.position.y );
				Debug.Log ("max " + rectTrans.offsetMax);
				Debug.Log ("min " +rectTrans.offsetMin);
			}
				
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (rectTrans.offsetMax.y >= 0 && rectTrans.offsetMin.y <= 0 && rectTrans.offsetMax.x >= 0 && rectTrans.offsetMin.x <= 0) {
				rectTrans.position = new Vector2 (transform.position.x + 1, transform.position.y );
				Debug.Log ("max " + rectTrans.offsetMax);
				Debug.Log ("min " +rectTrans.offsetMin);
			}
		}

//		rectTrans.position = new Vector2 (
//			Mathf.Clamp(rectTrans.position.x, min, max),
//			Mathf.Clamp(rectTrans.position.y, min, max)
//		);

		if (rectTrans.offsetMax.x <= 0) {

			rectTrans.offsetMax = new Vector2 (0, rectTrans.offsetMax.y);
			//rectTrans.offsetMin = new Vector2 (rectTrans.offsetMin.x - rectTrans.offsetMax.x, rectTrans.offsetMin.y);
		}

		if (rectTrans.offsetMin.x > 0) {

			rectTrans.offsetMin = new Vector2 (0, rectTrans.offsetMax.y);
			//rectTrans.offsetMax = new Vector2 (rectTrans.offsetMax.x - rectTrans.offsetMin.x, rectTrans.offsetMin.y);
		}

		if (touchFlag) {
			if (Input.touchCount == 1 && rectTrans.sizeDelta.x > 0) {
				Touch touch = Input.GetTouch (0);

				if (rectTrans.offsetMax.y >= 0 && rectTrans.offsetMin.y <= 0&& rectTrans.offsetMax.x >= 0 && rectTrans.offsetMin.x <= 0) {
					rectTrans.position = new Vector2 (transform.position.x + touch.deltaPosition.x, transform.position.y + touch.deltaPosition.y);
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
					view = view + (backDist - newDist) / 100.0f;
					v = v + (newDist - backDist) / 1000.0f;

					// 限界値をオーバーした際の処理
					if (v > vMax) {
						v = vMax;
					} else if (v < vMin) {
						v = vMin;
					}

					// 相対値が変更した場合、カメラに相対値を反映させる
					if (v != 0) {
						//transform.localScale = new Vector3 (v, v, 1.0f);
						if(backDist<newDist && rectTrans.sizeDelta.x <= 200){
							rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x + v, rectTrans.sizeDelta.y + v);
						}
						if(backDist>newDist && rectTrans.sizeDelta.x >= 0){
							rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x - v, rectTrans.sizeDelta.y - v);
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
