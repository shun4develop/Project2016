using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class FlexibleScrollVewController : ScrollRect{
	public override void OnInitializePotentialDrag (PointerEventData eventData)
	{
		base.OnInitializePotentialDrag (eventData);
		Debug.Log ("test");
	}

	public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnDrag (eventData);
		Debug.Log ("test");
	}

	public override void OnBeginDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnBeginDrag (eventData);
		Debug.Log ("test");
	}

	public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnEndDrag (eventData);
		Debug.Log ("test");
	}
}
