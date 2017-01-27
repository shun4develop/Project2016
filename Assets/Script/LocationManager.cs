using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LocationManager : MonoBehaviour
{
	public static float IntervalSeconds = 1.0f;
	public static LocationServiceStatus Status;
	public static LocationInfo location;

	IEnumerator Start()
	{
		while (true)
		{
			Status = Input.location.status;
			if (Input.location.isEnabledByUser)
			{
				switch(Status)
				{
				case LocationServiceStatus.Stopped:
					Input.location.Start(1,1);
					break;
				case LocationServiceStatus.Running:
					location = Input.location.lastData;
					//Debug.Log ("緯度 ->" + location.latitude);
					//Debug.Log ("経度 -> "+location.longitude);
					break;
				default:
					break;
				}
			}
			else
			{
				// FIXME 位置情報を有効にして!! 的なダイアログの表示処理を入れると良さそう
				Debug.Log("位置情報へのアクセスを許可してください");
			}

			// 指定した秒数後に再度判定を走らせる
			yield return new WaitForSeconds(IntervalSeconds);
		}
	}

}