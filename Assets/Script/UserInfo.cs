using UnityEngine;
using System.Collections;

public class UserInfo{
	public string latitude;
	public string longitude;

	private static UserInfo _instance;

	public static UserInfo instance {
		get{
			if (_instance == null) {
				_instance = new UserInfo();
			}
			return _instance;
		}
	}

	public void SetLocation(string lat, string lon){
		this.latitude = lat;
		this.longitude = lon;
	}
}
