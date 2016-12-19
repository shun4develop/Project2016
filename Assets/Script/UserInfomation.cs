using UnityEngine;
using System.Collections;


public class UserInfomation{
	[SerializeField]
	private string name;
	[SerializeField]
	private string description;
	[SerializeField]
	private string icon_filepath;
	[SerializeField]
	private string email;

	public string getUserName(){
		return name;
	}
	public string getDesc(){
		return description;
	}
	public string getUserIconDataPath(){
		return icon_filepath;
	}
	public string getEmail(){
		return email;
	}
	public override string ToString ()
	{
		if (string.IsNullOrEmpty (email)) {
			email = "情報なし";
		}
		return "user_name -> " + name + "\n"
			+ "desc -> " +description+"\n" 
			+ "userIconDataPath -> "+ icon_filepath + "\n"
			+ "email -> " + email;
	}
}
