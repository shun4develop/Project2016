using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IosCamera{
//	[DllImport("__Internal")]
//	private static extern int sampleMethod1();
	[DllImport("__Internal")]
	private static extern void showCamera();
	[DllImport("__Internal")]
	private static extern void showAlbum();
//
//
	public static void album(){
		showAlbum ();
	}

	public static void cameraStart(){
		showCamera ();
	}
}
