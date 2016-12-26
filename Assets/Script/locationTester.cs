using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class locationTester : MonoBehaviour {

	private Text text;
	private LocationInfo tmpLocation;


	double lat;
	double lon;

	int distance;

	// Use this for initialization
	void Start () {
		if (Input.location.isEnabledByUser) {
			Input.location.Start ();
		}
		text = GetComponent<Text> ();
		tmpLocation = Input.location.lastData;

		StartCoroutine (locationUpdate());
	}
	
	// Update is called once per frame
	void Update () {

//		double lat = 35.67508;
//		double lon = 138.5096;
//
//		double lat2 = 35.6755;
//		double lon2 = 138.509;

//		text.text = Input.location.status + "\n" + Input.location.lastData.timestamp + "\n" +Input.location.lastData.latitude.ToString() + "\n" + Input.location.lastData.longitude.ToString();

	}

	private IEnumerator locationUpdate(){
		while(true){
			distance = CalculateDistance (tmpLocation, Input.location.lastData);

			text.text = Input.location.status + "\n" + distance.ToString() + "\n" + Input.location.lastData.timestamp + "\n" + tmpLocation.latitude.ToString() + "\n" + tmpLocation.longitude.ToString()
				+ "\n\n" + Input.location.lastData.latitude.ToString() + "\n" + Input.location.lastData.longitude.ToString()+"\n";
			
			if (Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running) {
				if (distance >= 5) {
					tmpLocation = Input.location.lastData;
				}
			}
			yield return new WaitForSeconds (3);
		}
	}


	/// <summary>
	/// 度単位から等価なラジアン単位に変換します。
	/// </summary>
	/// <param name="deg">度単位</param>
	/// <returns></returns>
	static double deg2rad(double deg)
	{
		return (deg / 180) * Math.PI;
	}

	/// <summary>
	/// 2点間の位置情報から距離を求める
	/// </summary>
	/// <param name="posA"></param>
	/// <param name="posB"></param>
	/// <returns></returns>
	public static int CalculateDistance(LocationInfo posA, LocationInfo posB)
	{
		return CalculateDistance (posA.latitude,posA.longitude,posB.latitude,posB.longitude);
	}
	public static int CalculateDistance(double a1,double a2,double b1,double b2)
	{
		// 2点の緯度の平均
		double latAvg = deg2rad(a1 + ((b1 - a1) / 2));
		// 2点の緯度差
		double latDifference = deg2rad(a1 - b1);
		// 2点の経度差
		double lonDifference = deg2rad(a2 - b2);

		double curRadiusTemp = 1 - 0.00669438 * Math.Pow(Math.Sin(latAvg), 2);
		// 子午線曲率半径
		double meridianCurvatureRadius = 6335439.327 / Math.Sqrt(Math.Pow(curRadiusTemp, 3));
		// 卯酉線曲率半径
		double primeVerticalCircleCurvatureRadius = 6378137 / Math.Sqrt(curRadiusTemp);

		// 2点間の距離
		double distance = Math.Pow(meridianCurvatureRadius * latDifference, 2) 
			+ Math.Pow(primeVerticalCircleCurvatureRadius
				* Math.Cos(latAvg) * lonDifference, 2);
		distance = Math.Sqrt(distance);

		return (int)Math.Round(distance);
	}
}
