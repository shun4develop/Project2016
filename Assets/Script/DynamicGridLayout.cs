using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]

public class DynamicGridLayout : MonoBehaviour {
	public int col;
	// Use this for initialization
	void Start () {
		RectTransform rt = GetComponent<RectTransform> ();
		GridLayoutGroup gl = GetComponent<GridLayoutGroup> ();
		float size = rt.rect.width / col;;
		gl.cellSize = new Vector2 (size,size);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
