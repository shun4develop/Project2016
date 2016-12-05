using UnityEngine;
using System.Collections;
using MyClass;

/// <summary>
///  ベースクラス
/// </summary>

public abstract class CanvasCreatorBase : MonoBehaviour {
	public GameObject canvas;

	public abstract void create (Item item);
	public abstract void create (int id);
}
