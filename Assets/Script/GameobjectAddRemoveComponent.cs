using UnityEngine;

/// <summary>
/// Component 型の拡張メソッドを管理するクラス
/// </summary>
public static class ComponentExtensions
{
	/// <summary>
	/// コンポーネントを削除します
	/// </summary>
	public static void RemoveComponent<T>(this Component self) where T : Component
	{
		GameObject.Destroy(self.GetComponent<T>());
	}
}

/// <summary>
/// GameObject 型の拡張メソッドを管理するクラス
/// </summary>
public static class GameObjectExtensions
{   
	/// <summary>
	/// コンポーネントを削除します
	/// </summary>
	public static void RemoveComponent<T>(this GameObject self) where T : Component
	{
		GameObject.Destroy(self.GetComponent<T>());
	}
}