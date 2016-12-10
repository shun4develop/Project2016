using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyManagers;
using System.Collections.Generic;
public class SocialLoginController : MonoBehaviour {
	
	public SocialLoginPage loginPage;
	public SocialRegisterPage registerPage;

	public void socialLogin(Dictionary<string,object> dic){
		if(dic ["user_find"] == null){
			return;
		}else if ((bool)dic ["user_find"]) {
			loginPage.SendMessage ("slideIn","RIGHT");
			loginPage.SendMessage ("showLoginInfo",dic);
		} else {
			registerPage.SendMessage ("slideIn","RIGHT");
			registerPage.SendMessage ("showRegisterInfo",dic);
		}
	}
}
