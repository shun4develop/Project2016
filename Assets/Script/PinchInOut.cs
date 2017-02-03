﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PinchInOut : MonoBehaviour {

	[SerializeField]
	private GridLayoutGroup content;        //拡大縮小するコンテンツ
	[SerializeField]
	private Transform wrapper;                      //コンテンツのラッパー

	//コンテンツのRectTransformの参照
	private RectTransform contentRect;

	[SerializeField]
	private float scale;    //現在の拡大率

	[System.Serializable]
	struct RangeClass
	{
		public float min, max;
	}

	[SerializeField]
	private RangeClass RangeScale;                  //拡大縮小の範囲
	[SerializeField]
	private RangeClass RangeLimitedScale;   //収束する範囲

	[SerializeField]
	private float TweenSecond;              //収束するまでにかかる時間

	private bool isPinch = false;           //ピンチ中であればtrue
	private Vector3 center;                         //現在の中心座標
	private Vector2 defauldCellSize;        //拡大率が1の時のコンテンツの大きさ

	#if UNITY_IOS || UNITY_ANDROID
	private float max_distance = 0;         //ピンチ開始時の指間の距離
	#endif


	void Start () {

		contentRect = content.GetComponent<RectTransform> ();   //参照を設定

		defauldCellSize = content.cellSize;
		center = contentRect.localPosition / scale;

		//状態の初期化
		UpdateScaling ();

		//表示されている画面の中心を拡大率に合わせて調整する
		contentRect.anchoredPosition *= scale;
	}


	void Update () {

		#if UNITY_EDITOR
		EditorControl ();
		#endif

		#if !UNITY_EDITOR
		MobileControl();
		#endif

	}


	#if UNITY_EDITOR
	private void EditorControl(){
		//タッチ中の処理
		if (isPinch) {
			//タッチ終了を感知し、終了処理をする
			if (Input.GetAxisRaw ("Vertical") == 0) {
				isPinch = false;
				StartTweenCoroutine ();
				return;
			}
			scale += Input.GetAxisRaw ("Vertical") * 1f * Time.deltaTime;
			SetNewScale (scale);
			UpdateScaling ();
			return;
		}
		//タッチ開始時を感知し、初期化処理をする
		if (Input.GetAxisRaw ("Vertical") != 0) {
			center = contentRect.localPosition / scale;
			isPinch = true;
		}
	}
	#endif


	#if UNITY_IOS || UNITY_ANDROID
	private void MobileControl(){
		//タッチ中の処理
		if (isPinch) {
			//タッチ終了を感知し、終了処理をする
			if (Input.touchCount < 2) {
				isPinch = false;
				StartTweenCoroutine ();
				return;
			}
			float distance = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
			SetNewScale (distance / max_distance);
			UpdateScaling ();
			return;
		}
		//タッチ開始時を感知し、初期化処理をする
		if (Input.touchCount == 2) {
			center = contentRect.localPosition / scale;
			isPinch = true;
			float distance = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
			max_distance = distance / scale;
		}
	}
	#endif


	/// <summary>
	/// 新しい拡大率のバリデートと更新をする
	/// </summary>
	private void SetNewScale(float new_scale){

		// min < 新しい拡大率 < max に設定する
		new_scale = Mathf.Min (new_scale, RangeScale.max);
		new_scale = Mathf.Max (new_scale, RangeScale.min);

		scale = new_scale;

	}


	/// <summary>
	/// 収束させる拡大率を求め、コルーチンを開始する
	/// </summary>
	private void StartTweenCoroutine(){

		// min < 収束させる拡大率 < max に設定する
		float limited_scale = scale;
		limited_scale = Mathf.Min (limited_scale, RangeLimitedScale.max);
		limited_scale = Mathf.Max (limited_scale, RangeLimitedScale.min);

		StartCoroutine (TweenLimitedScale (limited_scale));

	}


	/// <summary>
	/// 拡大率を設定された値に収束させる
	/// </summary>
	IEnumerator TweenLimitedScale(float limited_scale){

		if (scale == limited_scale)
			yield break;

		float timer = 0;
		float def_scale = scale - limited_scale;

		//scaleをTweenSecond秒以内にlimited_rateにする
		while(timer < TweenSecond){
			timer += Time.deltaTime;
			scale -= def_scale * Time.deltaTime * (1f / TweenSecond);
			UpdateScaling ();
			yield return 0;
		}

	}


	/// <summary>
	/// 設定された拡大率に基づいてオブジェクトの大きさを更新する
	/// </summary>
	private void UpdateScaling(){
		content.cellSize = defauldCellSize * scale;                     //想定するコンテンツの大きさを更新する
		contentRect.localPosition = center * scale;                     //拡大率が変わった時に中心座標がずれないように再設定する
		wrapper.localScale = new Vector3(scale,scale,1);        //全体を拡大縮小する
	}

}

//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//
//public class PinchInOut : MonoBehaviour {
//
//	public bool touchFlag = false;
//
//	float vMin = 1.0f;
//	float vMax = 5.0f;
//
//	//直前の2点間の距離.
//	private float backDist = 0.0f;
//	//初期値
//	float view = 60.0f;
//	float v = 1.0f;
//
//	public GameObject content;
//
//	// Update is called once per frame
//	void Update () {
//		
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			content.transform.localScale= new Vector3 (transform.localScale.x + 1, transform.localScale.y+1);
//		}
//		if (touchFlag) {
//			// マルチタッチかどうか確認
//			if (Input.touchCount == 2) {
//				// タッチしている２点を取得
//				Touch t1 = Input.GetTouch (0);
//				Touch t2 = Input.GetTouch (1);
//
//				//2点タッチ開始時の距離を記憶
//				if (t2.phase == TouchPhase.Began) {
//					backDist = Vector2.Distance (t1.position, t2.position);
//				} else if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved) {
//					// タッチ位置の移動後、長さを再測し、前回の距離からの相対値を取る。
//					float newDist = Vector2.Distance (t1.position, t2.position);
//					view = view + (backDist - newDist) / 100.0f;
//					v = v + (newDist - backDist) / 1000.0f;
//
//					// 限界値をオーバーした際の処理
//					if (v > vMax) {
//						v = vMax;
//					} else if (v < vMin) {
//						v = vMin;
//					}
//
//					// 相対値が変更した場合、カメラに相対値を反映させる
//					if (v != 0) {
////					map.transform.localScale = new Vector3(v, v, 1.0f);
//						content.transform.localScale = new Vector3 (v, v, 1.0f);
//						backDist = newDist;
//					}
//				}
//			}
//		}
//	}
//
//	public void trueTouchFlag(){
//		touchFlag = true;
//	}
//
//	public void falseTouchFlag(){
//		touchFlag = false;
//		content.transform.localScale = new Vector3 (1, 1, 1);
//	}
//}
