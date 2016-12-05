using UnityEngine;
using System.Collections;
using System;
[Serializable]
public class UserOfFacebook : UserOfSNS {
	public string gender;
	public string first_name;
	public string last_name;
	public string email;
	public override string ToString ()
	{
		return base.ToString ()
		+ "gender -> " + gender + "\n"
		+ "first_name -> " + first_name + "\n"
		+ "last_name -> " + last_name + "\n"
		+ "email -> " + email +"\n";
	}
	//https://graph.facebook.com/[id]/picture
}
