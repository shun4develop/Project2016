using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]

public class DynamicGridLayout : MonoBehaviour {
	public int col;
	private int childCount;
	// Use this for initialization
	void Start () {
		RectTransform rt = GetComponent<RectTransform> ();
		GridLayoutGroup gl = GetComponent<GridLayoutGroup> ();
		float size = rt.rect.width / col;
		gl.cellSize = new Vector2 (size,size);
	}
	
	// Update is called once per frame
	void Update () {
		if(childCount != transform.childCount){
			foreach (RectTransform rt in transform) {
				rt.localScale = new Vector2 (0.95f,0.95f);
			}
			childCount = transform.childCount;
		}
	}
}
