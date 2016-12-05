using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PeripheryContent : MonoBehaviour {

	public Text t;

	private List<string> titlelist = new List<string>();

	public void pushButton(){
		GenerateMapMarker marker = new GenerateMapMarker ();

		titlelist = marker.getTitleList();

		for (int i = 0; i < titlelist.Count; i++) {
			t.text += titlelist[i] + "\n";
		}
	}

}
