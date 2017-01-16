using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IosCamera{
	[DllImport("__Internal")]
	private static extern int sampleMethod1();
	[DllImport("__Internal")]
	private static extern void showCamera();

	public static string test(){
		int s = sampleMethod1();
		return s.ToString();
	}

	public static void cameraStart(){
		showCamera ();
	}
}
