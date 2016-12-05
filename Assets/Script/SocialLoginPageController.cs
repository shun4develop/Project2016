using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SocialLoginPageController : MonoBehaviour {
	public Image img;
	public Text user_name;
	public Text log;
	private string token;
	private SNSIconImageViewer iv;
	private UserOfSNS user;
	void Start(){
		iv = img.GetComponent<SNSIconImageViewer> ();
	}

	public void setUser(UserOfSNS user){
		this.user = user;
		if (user.GetType () == typeof(UserOfTwitter)) {
			UserOfTwitter t_user = (UserOfTwitter)user;
			iv.show (t_user.profile_image_url);
		} else if (user.GetType () == typeof(UserOfFacebook)) {
			UserOfFacebook f_user = (UserOfFacebook)user;
			iv.show ("http://graph.facebook.com/" + f_user.id + "/picture");
		}
		user_name.text = user.name;
	}
	public void setToken(string token){
		this.token = token;
	}
	public UserOfSNS getSNSUser(){
		return this.user;
	}
	public string getToken(){
		return token;
	}
}
