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
	public GameObject webManager;
	private int a;

	private List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();

	private void Start(){
		map = GetComponent<OnlineMaps>();
		control = GetComponent<OnlineMapsTileSetControl>();

		// Get instance of LocationService.
		locationService = GetComponent<OnlineMapsLocationService>();//OnlineMapsLocationService.instance;

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

	}

	void Update(){
//		if (map.zoom >= 16) {
//			control.allowZoom = true;
//		}
	}

	//locationが変化した時行う処理
	private void OnLocationChanged(Vector2 position)
	{
		Debug.Log("location change");
		t.text = position.ToString("F6") + "\n";
		a++;
		t.text += a.ToString();

		//成功
		Action<string> positive_func = (string text) => {
			List<Item> items = JsonHelper.ListFromJson<Item> (text);
			GenerateMapMarker marker = new GenerateMapMarker();
			marker.allMarkerDestroy(t);
			marker.createMarker(items);

		};
		//失敗
		Action negative_func = () => {
			Debug.Log("miss");
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
}
