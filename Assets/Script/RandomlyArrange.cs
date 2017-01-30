using UnityEngine;
using System.Collections;

/// <summary>
///  オブジェクトを適当な位置に配置する
/// 
///  target の方向を向く
/// </summary>

public class RandomlyArrange : MonoBehaviour {


	GameObject target;

	float x;
	float y = 0;
	float z;

	// Use this for initialization
	void Start () {

		target = GameObject.Find("Main Camera");

		x = Random.Range(-5.0f, 5.0f);
		y = Random.Range(-5.0f, 5.0f);
		z = Random.Range(-5.0f, 5.0f);

		if (x > 0) {
			x += 5f;
		} else {
			x -= 5f;
		}
		if (y > 0) {
			y += 5;
		} else {
			y -= 5;
		}
		if (z > 0) {
			z += 5f;
		} else {
			z -= 5f;
		}
		gameObject.transform.position = new Vector3 (x, y, z);

		gameObject.transform.LookAt (target.transform);
		//this.gameObject.transform.LookAt (Vector3);
		transform.Rotate(new Vector3(0, 1, 0), 180);

	}
}
