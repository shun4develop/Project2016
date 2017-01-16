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

	private GameObject tooltip;

	private void Start(){
		map = GetComponent<OnlineMaps>();
		control = GetComponent<OnlineMapsTileSetControl>();

		// Get LocationService
		locationService = GetComponent<OnlineMapsLocationService>();

		//説明文のスタイル変更
		OnlineMaps.instance.OnPrepareTooltipStyle += OnPrepareTooltipStyle;
		locationService.OnLocationChanged += OnLocationChanged;

		//locationServiceがなかった場合
		if (locationService == null)
		{
			Debug.LogError(
				"Location Service not found.\nAdd Location Service Component (Component / Infinity Code / Online Maps / Plugins / Location Service).");
			return;
		}

		map.showMarkerTooltip = OnlineMapsShowMarkerTooltip.onPress;
		map.AddMarker(138.509600,35.675190, "Dynamic marker").OnPress += OnMarkerPress;
		map.AddMarker(138.5097,35.675194, "marker2").OnPress += OnMarkerPress;

		OnlineMapsControlBase.instance.OnUpdateAfter += OnUpdateAfter;

		// Intercepts tooltip style.
		map.OnPrepareTooltipStyle += OnPrepareTooltipStyle;

		// Intercepts drawing tooltips.
		OnlineMapsMarkerBase.OnMarkerDrawTooltip += OnMarkerDrawTooltip;
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
			}
//			Debug.Log(markerlist[0].label);
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

	public void positionMoveMap(){
		Vector3 pos = map.transform.position;
		pos.x += 10000;
		map.transform.position = pos;
	}

	public void positionreturnMap(){
		Vector3 pos = map.transform.position;
		pos.x -= 10000;
		map.transform.position = pos;
		
	}

	private void OnMarkerPress(OnlineMapsMarkerBase marker)
	{
		// Change active marker
		if (activeMarker == marker) {
			tooltipflag = !tooltipflag;
		} else {
			tooltipflag = true;
		}
		activeMarker = marker;
	}

	private void OnMarkerDrawTooltip(OnlineMapsMarkerBase marker)
	{
		if (tooltipflag) {
			// Get screen position of marker
			Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition (marker.position);

			// Calculate the size
			GUIContent tip = new GUIContent (marker.label);
			Debug.Log (tip);
			Vector2 size = tooltipStyle.CalcSize (tip);

			// Draw the tooltip
			GUI.Label (new Rect (screenPosition.x - size.x / 2 - 5, Screen.height - screenPosition.y - size.y - 20, size.x + 10, size.y + 5), marker.label, tooltipStyle);
		}
	}

	private void OnUpdateAfter()
	{
		// If activeMarker exists, restore tootip
		if (activeMarker != null)
		{
			map.tooltipMarker = activeMarker;
			map.tooltip = activeMarker.label;
		}
	}

}
