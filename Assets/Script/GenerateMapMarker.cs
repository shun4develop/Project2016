using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MyClass;

public class GenerateMapMarker{

	private static List<OnlineMapsMarker> markerlist = new List<OnlineMapsMarker>();
	private static List<string> titlelist = new List<string>();

//	public GenerateMapMarker(){

//	}

	public void createMarker(List<Item> items){
		for(int i = 0; i < items.Count; i++){
			Item item = items [i];
			markerlist.Add(OnlineMaps.instance.AddMarker(item.getLongitudeParseDouble() ,item.getLatitudeParseDouble(), null, ""));
			markerlist [i].customData = item;
			markerlist [i].label = item.getTitle() + "\n" + item.getDesc();
			titlelist.Add(item.getTitle ());
		}
	}

	public void allMarkerDestroy(Text t){
		int a=0;
		if (markerlist.Count != 0) {
			while (markerlist.Count != 0) {
				OnlineMaps.instance.RemoveMarker (markerlist [0]);
				markerlist.Remove (markerlist [0]);
				titlelist.Remove (titlelist [0]);
				//t.text += "markerDestoroy" + "\n";
				a++;
			}
		}
		//t.text += "destroy呼ばれた回数" + a.ToString();
	}

	public List<OnlineMapsMarker> getMarkerList(){
		return markerlist;
	}

	public List<string> getTitleList(){
		return titlelist;
	}
}