using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IOSPlugin{
//	[DllImport("__Internal")]
//	private static extern int sampleMethod1();
	[DllImport("__Internal")]
	private static extern void showCamera();
	[DllImport("__Internal")]
	private static extern void showAlbum();
	[DllImport("__Internal")]
	private static extern void savePhoto();
	[DllImport("__Internal")]
	private static extern void alertTest();
	[DllImport("__Internal")]
	private static extern void statusBarHidden();
	[DllImport("__Internal")]
	private static extern void statusBarShow();


	public static void album(){
		showAlbum ();
	}

	public static void cameraStart(){
		showCamera ();
	}
	public static void save(){
		savePhoto ();
	}

	public static void choice(){
		alertTest ();
	}
	public static void hideStatusBar(){
		statusBarHidden ();	
	}
	public static void showStatusBar(){
		statusBarShow ();
	}

}
