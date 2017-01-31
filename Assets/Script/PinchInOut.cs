using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinchInOut : MonoBehaviour {

	public bool touchFlag = false;

	float vMin = 1.0f;
	float vMax = 5.0f;

	//直前の2点間の距離.
	private float backDist = 0.0f;
	//初期値
	float view = 60.0f;
	float v = 1.0f;

	public GameObject content;

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log (touchFlag);
			content.transform.localScale= new Vector3 (transform.localScale.x + 1, transform.localScale.y+1);
		}
		if (touchFlag) {
			// マルチタッチかどうか確認
			if (Input.touchCount == 2) {
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
						content.transform.localScale = new Vector3 (v, v, 1.0f);
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
		content.transform.localScale = new Vector3 (1, 1, 1);
	}
}
