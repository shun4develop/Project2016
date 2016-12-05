using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyCommon;
public class SocialLoginButton: MonoBehaviour {
	public void login(string type){
		WebManager.instance.socialLogin (type);	
	}
}
