using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using MyClass;
using MyLibrary;

public class MapControl : MonoBehaviour {
	
	private OnlineMapsLocationService locationService;
	private OnlineMapsTileSetControl control;
	private OnlineMaps map;
	public Text t;
	private int a;

	private OnlineMapsMarkerBase activeMarker;
	private GUIStyle tooltipStyle;
	private bool tooltipflag;

	private List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();

	//custom tooltip用変数
	public GameObject tooltipPrefab;
	public Canvas container;

	private OnlineMapsMarker marker;
	private GameObject tooltip;
	private int markerHeight;

	private void Start(){
		map = GetComponent<OnlineMaps>();
		control = GetComponent<OnlineMapsTileSetControl>();

		// Get LocationService
		locationService = GetComponent<OnlineMapsLocationService>();

		locationService.OnLocationChanged += OnLocationChanged;

		//locationServiceがなかった場合
		if (locationService == null)
		{
			Debug.LogError(
				"Location Service not found.\nAdd Location Service Component (Component / Infinity Code / Online Maps / Plugins / Location Service).");
			return;
		}
			
		//map.AddMarker(138.509600,35.675190, "Dynamic marker").OnPress += OnMarkerPress;

		//test用のマーカー
		marker = map.AddMarker(138.5097,35.675194, "marker2");
		marker.OnPress += OnMarkerPress;
		marker.customData = "custamdata";

		marker.OnDrawTooltip = delegate {  };

		// Intercepts tooltip style.
		map.OnPrepareTooltipStyle += OnPrepareTooltipStyle;

		map.OnUpdateLate += OnUpdateLate;
	}

	private void Update(){
		if (control.cameraRotation.x > 40) {
			control.cameraRotation.x = 40;
		}
	}

	//locationが変化した時行う処理
	private void OnLocationChanged(Vector2 position)
	{
		Debug.Log("location change");
		t.text = position.ToString("F6") + "\n";
		a++;
		t.text += a.ToString();
		UserInfo.instance.SetLocation (position.y.ToString ("F6"), position.x.ToString ("F6"));

		//成功
		Action<string> positive_func = (string text) => {
			List<Item> items = JsonHelper.ListFromJson<Item> (text);
			GenerateMapMarker marker = new GenerateMapMarker();
			marker.allMarkerDestroy(t);
			marker.createMarker(items);
			markerlist = marker.getMarkerList();
			foreach(OnlineMapsMarker m in markerlist){
				m.OnPress += OnMarkerPress;
				m.OnDrawTooltip = delegate {  };
			}
			markerHeight = markerlist[0].height;

		};
		//失敗
		Action negative_func = () => {
			Debug.Log("miss");
			t.text = "miss";
		};

		WebManager.instance.downloadContents(positive_func, negative_func,position.y.ToString("F6"), position.x.ToString("F6"));
	}

	//GPSの緯度情報を返す
	public double getPositonX(){
		return locationService.position.x;
	}
	//GPSの経度情報を返す
	public double getPositonY(){
		return locationService.position.y;
	}

	//コメントのフォントサイズ変更
	private void OnPrepareTooltipStyle(ref GUIStyle style)
	{
		// Change the style settings.
		style.fontSize = 30;
		tooltipStyle = style;
	}

	//mapオブジェクトを移動
	public void positionMoveMap(){
		Vector3 pos = map.transform.position;
		pos.x += 10000;
		map.transform.position = pos;
	}

	//mapオブジェクトを元の位置に戻す
	public void positionreturnMap(){
		Vector3 pos = map.transform.position;
		pos.x -= 10000;
		map.transform.position = pos;
		
	}

	//マーカーを押した時の処理
	private void OnMarkerPress(OnlineMapsMarkerBase marker)
	{
		//this.marker = marker;
		OnlineMapsUtils.DestroyImmediate (tooltip);
		// Change active marker
		if (activeMarker == marker) {
			tooltipflag = !tooltipflag;
		} else {
			tooltipflag = true;
		}
		activeMarker = marker;
	}

	//custom tooltip
	private void OnUpdateLate()
	{
		//Debug.Log ("OnupdateLate");
		OnlineMapsMarkerBase tooltipMarker = OnlineMaps.instance.tooltipMarker;
		if (tooltipMarker == activeMarker && tooltipflag) {
			if (tooltip == null) {
				tooltip = Instantiate (tooltipPrefab) as GameObject;
				(tooltip.transform as RectTransform).SetParent (container.transform);
			}
			Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition (activeMarker.position);
			screenPosition.y += markerHeight + 100;
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (container.transform as RectTransform, screenPosition, null, out point);
			(tooltip.transform as RectTransform).localPosition = point;
			Item item = (Item)activeMarker.customData;
			tooltip.GetComponentInChildren<Text> ().text = item.getTitle();
			//tooltip.GetComponentInChildren<Image> ().sprite = item.getThumbnail();
			Debug.Log(item.getThumbnail());
			//Debug.Log (marker.customData);
		}
	}
}