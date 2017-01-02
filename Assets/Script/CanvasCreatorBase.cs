using UnityEngine;
using System.Collections;
using MyClass;

/// <summary>
///  ベースクラス
/// 
/// </summary>

public abstract class CanvasCreatorBase : MonoBehaviour {
	protected GameObject canvas;

	public abstract void create (Item item);
}
