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
	private bool tooltipflag;

	private List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();

	//custom tooltip用変数
	public GameObject tooltipPrefab;
	public Canvas container;

	private OnlineMapsMarker marker;
	private GameObject tooltip;
	private int markerHeight;

	private Vector2 pos;

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

		//test用のマーカー
//		marker = map.AddMarker(138.5097,35.675194, "marker2");
//		marker.OnPress += OnMarkerPress;
//		marker.customData = "custamdata";
//
//		marker.OnDrawTooltip = delegate {  };

		//map.OnUpdateLate += OnUpdateLate;
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

		if (pos.x != position.x && pos.y != position.y) {
			Debug.Log ("createmarker");
			//成功
			Action<string> positive_func = (string text) => {
				ItemData.instance.SetLocationItems (JsonHelper.ListFromJson<Item> (text));
				GenerateMapMarker marker = new GenerateMapMarker ();
				marker.allMarkerDestroy (t);
				marker.createMarker (ItemData.instance.locationItems);
				markerlist = marker.getMarkerList ();
				foreach (OnlineMapsMarker m in markerlist) {
					m.OnPress += OnMarkerPress;
					m.OnDrawTooltip = delegate {
					};
				}
				markerHeight = markerlist [0].height;
			};
			//失敗
			Action negative_func = () => {
				Debug.Log ("miss");
				t.text = "miss";
			};

			WebManager.instance.downloadContents (positive_func, negative_func, UserInfo.instance.latitude, UserInfo.instance.longitude);
			pos = position;
		}
	}

	//GPSの緯度情報を返す
	public double getPositonX(){
		return locationService.position.x;
	}
	public string getPositonXParseString(){
		return locationService.position.x.ToString("F6");
	}
	//GPSの経度情報を返す
	public double getPositonY(){
		return locationService.position.y;
	}
	public string getPositonYParseString(){
		return locationService.position.y.ToString("F6");
	}

	//マーカーを押した時の処理
	private void OnMarkerPress(OnlineMapsMarkerBase marker)
	{
		
		//this.marker = marker;
		if (tooltip != null) {
			OnlineMapsUtils.DestroyImmediate (tooltip);
		}
		// Change active marker
		if (activeMarker == marker) {
			tooltipflag = !tooltipflag;
		} else {
			tooltipflag = true;
		}
		activeMarker = marker;

		Debug.Log ("ompress");
		//custamtooltip作成
		OnlineMapsMarkerBase tooltipMarker = OnlineMaps.instance.tooltipMarker;
		if (tooltipflag) {
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
			tooltip.GetComponentInChildren<ContentsViewerBase> ().init (item);
			tooltip.GetComponentInChildren<ContentsViewerBase> ().show ();
		}
	}
	//custom tooltip
	private void OnUpdateLate()
	{
//		Debug.Log ("OnupdateLate");
//		OnlineMapsMarkerBase tooltipMarker = OnlineMaps.instance.tooltipMarker;
//		if (tooltipMarker == activeMarker && tooltipflag) {
//			if (tooltip == null) {
//				tooltip = Instantiate (tooltipPrefab) as GameObject;
//				(tooltip.transform as RectTransform).SetParent (container.transform);
//			}
//			Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition (activeMarker.position);
//			screenPosition.y += markerHeight + 100;
//			Vector2 point;
//			RectTransformUtility.ScreenPointToLocalPointInRectangle (container.transform as RectTransform, screenPosition, null, out point);
//			(tooltip.transform as RectTransform).localPosition = point;
//			Item item = (Item)activeMarker.customData;
//			tooltip.GetComponentInChildren<Text> ().text = item.getTitle();
//			tooltip.GetComponentInChildren<ContentsViewerBase> ().init (item);
//			tooltip.GetComponentInChildren<ContentsViewerBase> ().show ();
//		}
	}
}