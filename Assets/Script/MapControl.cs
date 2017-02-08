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

	//private Vector2 pos;

	private void Start(){
		control = GetComponent<OnlineMapsTileSetControl>();

		// Get LocationService
		locationService = GetComponent<OnlineMapsLocationService>();

		locationService.OnLocationChanged += OnLocationChanged;
		//control.OnMapClick;
		if (Application.platform == RuntimePlatform.Android) {
			OnlineMaps.instance.control.OnMapPress += OnMapClick;
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			OnlineMaps.instance.control.OnMapClick += OnMapClick;
		} else {
			OnlineMaps.instance.control.OnMapPress += OnMapClick;
		}


		OnlineMapsBuildings.instance.OnBuildingCreated += OnBuildingCreated;
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
		UserInfo.instance.SetLocation (position.y.ToString ("F6"), position.x.ToString ("F6"));
		updateMap ();
	}
	public void updateMap(){
		Action<string> positive_func = (string text) => {
			ItemData.instance.SetLocationItems (JsonHelper.ListFromJson<Item> (text));
			GenerateMapMarker marker = new GenerateMapMarker ();
			marker.destroyAllMarker();
			marker.createMarker (ItemData.instance.locationItems);
			markerlist = marker.getMarkerList ();
			foreach (OnlineMapsMarker m in markerlist) {
				if(Application.platform == RuntimePlatform.Android){
					m.OnPress += OnMarkerPress;
				}else if (Application.platform == RuntimePlatform.IPhonePlayer) {
					m.OnClick += OnMarkerPress;
				}else{
					m.OnClick += OnMarkerPress;
				}
				m.OnDrawTooltip = delegate {
				};
				markerHeight = markerlist [0].height;
			}
			if(tooltip != null){
				Item item = ItemData.instance.getLocationItemById(tooltip.GetComponent<Tooltip>().getItemId());
				Destroy(tooltip);
				tooltip = null;
				if(item != null){
					createTooltip((Item)selectedMarker.customData);
				}
			}
		};
		//失敗
		Action negative_func = () => {
			Debug.Log ("miss");
		};

		WebManager.instance.downloadContents (positive_func, negative_func, UserInfo.instance.latitude, UserInfo.instance.longitude);

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
			if (selectedMarker != null) {
				selectedMarker.scale = 1f;
			}
			pressedMarker.scale = 1.8f;
			// Change active marker
			selectedMarker = pressedMarker;
			createTooltip ((Item)selectedMarker.customData);
		} else {
			selectedMarker.scale = 1f;
			selectedMarker = null;
			Destroy (tooltip);
		}
	}

	private void OnMapClick(){
		if (selectedMarker != null) {
			selectedMarker.scale = 1f;
			selectedMarker = null;
			Destroy (tooltip);
		}
	}
	private void createTooltip(Item item){
		tooltip = Instantiate (tooltipPrefab) as GameObject;
		(tooltip.transform as RectTransform).SetParent (container.transform);

		Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition (selectedMarker.position);
		screenPosition.y += markerHeight+150;
		Vector2 point;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (container.transform as RectTransform, screenPosition, Camera.main , out point);

		RectTransform tooltipRectTransform = tooltip.GetComponent<RectTransform> ();
		tooltipRectTransform.localPosition = point;
		tooltipRectTransform.localScale = new Vector3 (1,1,1);
		tooltipRectTransform.rotation = new Quaternion (0,0,0,0);

		tooltip.transform.SetAsFirstSibling ();

		tooltip.GetComponent<Tooltip> ().init (item);
	}
	//ビルを作る際にMeshColliderを無効化する
	private void OnBuildingCreated(OnlineMapsBuildingBase building)
	{
		building.GetComponent<MeshCollider>().enabled = false;
	}
}