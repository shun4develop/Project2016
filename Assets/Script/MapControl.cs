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

	private OnlineMapsMarkerBase selectedMarker;
	private bool canCreateTooltipFlag;

	private List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();

	//custom tooltip用変数
	public GameObject tooltipPrefab;
	public Canvas container;

	private OnlineMapsMarker marker;
	private GameObject tooltip;
	private int markerHeight;
	private bool markertouchflag;

	private Vector2 pos;

	private GenerateMapMarker ma;

	private void Start(){
		map = GetComponent<OnlineMaps>();
		control = GetComponent<OnlineMapsTileSetControl>();

		// Get LocationService
		locationService = GetComponent<OnlineMapsLocationService>();

		locationService.OnLocationChanged += OnLocationChanged;
		//control.OnMapClick;
		OnlineMaps.instance.control.OnMapClick += OnMapClick;

		OnlineMapsBuildings.instance.OnBuildingCreated += OnBuildingCreated;
		//locationServiceがなかった場合
		if (locationService == null)
		{
			Debug.LogError(
				"Location Service not found.\nAdd Location Service Component (Component / Infinity Code / Online Maps / Plugins / Location Service).");
			return;
		}

		ma = new GenerateMapMarker ();
		//test用のマーカー
//		marker = map.AddMarker(139.7942,35.74987, "marker2");
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
		t.text += position.ToString("F6") + "\n";
		a++;
		t.text += a.ToString();
		UserInfo.instance.SetLocation (position.y.ToString ("F6"), position.x.ToString ("F6"));

			Debug.Log ("createmarker");
			//成功
			Action<string> positive_func = (string text) => {
				ItemData.instance.SetLocationItems (JsonHelper.ListFromJson<Item> (text));
				ma.allMarkerDestroy (t);
				ma.createMarker (ItemData.instance.locationItems);
				markerlist = ma.getMarkerList ();
				foreach (OnlineMapsMarker m in markerlist) {
					m.OnClick += OnMarkerPress;
					m.OnDrawTooltip = delegate {
					};
				markerHeight = markerlist [0].height;
				}
			};
			//失敗
			Action negative_func = () => {
				Debug.Log ("miss");
				t.text = "miss";
			};

		WebManager.instance.downloadContents (positive_func, negative_func, UserInfo.instance.latitude, UserInfo.instance.longitude);
			pos = position;
		Debug.Log (UserInfo.instance.latitude + " + " + UserInfo.instance.longitude);
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
	private void OnMarkerPress(OnlineMapsMarkerBase pressedMarker)
	{
		if (selectedMarker != pressedMarker) {
			// Change active marker
			selectedMarker = pressedMarker;
			createTooltip ();
		} else {
			selectedMarker = null;
			Destroy (tooltip);
		}
	}

	private void OnMapClick(){
		selectedMarker = null;
		Destroy (tooltip);
	}
	private void createTooltip(){
		if (tooltip == null) {
			tooltip = Instantiate (tooltipPrefab) as GameObject;
			(tooltip.transform as RectTransform).SetParent (container.transform);
		}
		Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition (selectedMarker.position);
		screenPosition.y += markerHeight+150;
		Vector2 point;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (container.transform as RectTransform, screenPosition, Camera.main , out point);

		RectTransform tooltipRectTransform = tooltip.GetComponent<RectTransform> ();
		tooltipRectTransform.localPosition = point;
		tooltipRectTransform.localScale = new Vector3 (1,1,1);
		tooltipRectTransform.rotation = new Quaternion (0,0,0,0);

		tooltip.transform.SetAsFirstSibling ();

		Item item = (Item)selectedMarker.customData;
		tooltip.GetComponentInChildren<Text> ().text = item.getTitle();
		tooltip.GetComponentInChildren<ContentsViewerBase> ().init (item);
		tooltip.GetComponentInChildren<ContentsViewerBase> ().show ();
	}

	//ビルを作る際にMeshColliderを無効化する
	private void OnBuildingCreated(OnlineMapsBuildingBase building)
	{
		building.GetComponent<MeshCollider>().enabled = false;
	}
}