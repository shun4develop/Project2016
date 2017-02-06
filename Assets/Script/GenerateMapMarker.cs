using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MyClass;

public class GenerateMapMarker{

	private static List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();
	private static List<string> titlelist = new List<string>();

	//マーカー作成
	public void createMarker(List<Item> items){
		for(int i = 0; i < items.Count; i++){
			Item item = items [i];
			markerlist.Add(OnlineMaps.instance.AddMarker(item.getLongitudeParseDouble() ,item.getLatitudeParseDouble(), null, ""));
			markerlist [i].customData = item;
			markerlist [i].label = item.getTitle() + "\n" + item.getDesc();
			titlelist.Add(item.getTitle ());
		}
		Debug.Log ("createmarker" + markerlist.Count);
	}
	//マーカーを全て削除
	public void destroyAllMarker(){
		if (markerlist.Count > 0) {
			foreach (OnlineMapsMarker m in markerlist) {
				OnlineMaps.instance.RemoveMarker (m);
			}
			//OnlineMaps.instance.RemoveAllMarkers ();
			markerlist.Clear ();
			Debug.Log ("demarkerlist" + markerlist.Count);
		}
	}

	public List<OnlineMapsMarker> getMarkerList(){
		return markerlist;
	}

	public List<string> getTitleList(){
		return titlelist;
	}
}