using UnityEngine;
using System.Collections;
using System;
[Serializable]
public class UserOfTwitter : UserOfSNS{
	public string id_str;
	public string screen_name;
	public string description;
	public string profile_background_image_url;
	public string profile_background_image_url_https;
	public string profile_image_url;
	public string profile_image_url_https;
	public string email;

	public override string ToString ()
	{	
		return base.ToString ()
			+ "id_str->" + id_str + "\n"
			+ "screen_name->" + screen_name + "\n"
			+ "desc->" + description + "\n"
			+ "profile_background_image_url->" + profile_background_image_url + "\n"
			+ "profile_background_image_url_https->" + profile_background_image_url_https + "\n"
			+ "profile_image_url->" + profile_image_url + "\n"
			+ "profile_image_url_https->" + profile_image_url_https + "\n"
			+ "email -> " + email +"\n";
	}
}
