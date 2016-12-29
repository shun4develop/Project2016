using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Scrollbar))]
public class ScrollBarController : MonoBehaviour {
	private Scrollbar bar;
	public float time = 1f;
	public float moveAlpha = 0.5f;
	public float originalAlpha = 0.2f;

	void Awake(){
		bar = GetComponent<Scrollbar> ();
	}
	public void setAlpha(){
		if (bar.IsActive()) {
			StartCoroutine (setColor ());
		}
	}
	private IEnumerator setColor(){
		setAlphaToNormalColor (moveAlpha);
		yield return new WaitForSeconds (time);
		setAlphaToNormalColor (originalAlpha);
	}

	private void setAlphaToNormalColor(float a){
		ColorBlock colors = bar.colors;
		Color c = bar.colors.normalColor;
		colors.normalColor = new Color (c.r, c.g, c.b, a);

		bar.colors = colors;
	}
}
