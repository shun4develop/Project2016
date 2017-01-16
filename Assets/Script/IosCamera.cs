using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IosCamera{
	[DllImport("__Internal")]
	private static extern int sampleMethod1();

	public static string test(){
		int s = sampleMethod1();
		return s.ToString();
	}
}
