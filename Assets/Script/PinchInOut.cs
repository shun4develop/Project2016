using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinchInOut : MonoBehaviour {

//	public Image image;

	public bool touchFlag = false;

	//カメラ視覚の範囲
//	float viewMin = 20.0f;
//	float viewMax = 60.0f;

	float vMin = 1.0f;
	float vMax = 5.0f;

	//直前の2点間の距離.
	private float backDist = 0.0f;
	//初期値
	float view = 60.0f;
	float v = 1.0f;

	Vector2 oldPos;
	Vector2 startPos;
	Vector2 min;
	Vector2 max;

	Rect rect;

	void Start(){
		startPos = new Vector2 (transform.position.x, transform.position.y);

		rect = GetComponent<RectTransform> ().rect;

	}

	// Update is called once per frame
	void Update () {
		
		min = new Vector2(rect.xMin, rect.yMin);
		max = new Vector2(rect.xMax, rect.yMax);

		Vector2 pos = transform.position;

		//デバッグ用
		if (Input.GetKey (KeyCode.UpArrow))
			transform.position = new Vector2 (transform.position.x, transform.position.y -10);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.position = new Vector2 (transform.position.x, transform.position.y +10);
		if (Input.GetKey (KeyCode.LeftArrow) && pos.x > min.x)
			transform.position = new Vector2 (transform.position.x + 10, transform.position.y);
		if (Input.GetKey (KeyCode.RightArrow))
			transform.position = new Vector2 (transform.position.x-10, transform.position.y);

		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.localScale= new Vector3 (transform.localScale.x + 1, transform.localScale.y+1);
			Debug.Log (min + " / " + max);
		}
		//Vector2 pos = transform.position;

		//pos.x = Mathf.Clamp (pos.x, min.x, max.x);
		//pos.y = Mathf.Clamp (pos.y, min.y, max.y);

		//transform.position = pos;


		//Debug.Log ("x : " + transform.position.x + "y : "+ transform.position.y);
		//Debug.Log(transform.localScale);

		if (transform.localScale.x == 1 || transform.localScale.y == 1)
			transform.position = startPos;

		if (touchFlag) {
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
//					map.transform.localScale = new Vector3(v, v, 1.0f);
						transform.localScale = new Vector3 (v, v, 1.0f);
						backDist = newDist;
					}
				}
			} else if (Input.touchCount == 1 && transform.localScale.x != 1 && transform.localScale.y != 1) {
				Touch t = Input.GetTouch (0);

				if (t.phase == TouchPhase.Began) {
//					oldPos = t.position;
				} else if (t.phase == TouchPhase.Moved) {
					Vector2 deltaPos = new Vector2 (transform.position.x + t.deltaPosition.x, transform.position.y + t.deltaPosition.y);
					transform.position = new Vector2 (deltaPos.x,deltaPos.y);
				} else if (t.phase == TouchPhase.Ended) {
//					oldPos = new Vector2 (0, 0);
				}

			}
		}
	}

	public void trueTouchFlag(){
		touchFlag = true;
	}

	public void falseTouchFlag(){
		touchFlag = false;
		transform.position = startPos;
		transform.localScale = new Vector3 (1, 1, 1);
	}
}
