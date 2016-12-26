using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Profile{
	[SerializeField]
	private string login_type;
	[SerializeField]
	private string name;
	[SerializeField]
	private string description;
	[SerializeField]
	private string icon;

	public Profile(string login_type,string name,string description,string icon){
		this.name = name;
		this.description = description;
		this.icon = icon;
		this.login_type = login_type;
	}
	public void setDesc(string desc){
		this.description = desc;
	}
	public void setIcon(string icon){
		this.icon = icon;
	}
	public string getUserName(){
		return name;
	}
	public string getDesc(){
		return description;
	}
	public string getUserIconDataPath(){
		return icon;
	}
	public string getLoginType(){
		return login_type;
	}
	public override string ToString ()
	{
		
		return "login_type -> " + login_type + "\n"
			+ "user_name -> " + name + "\n"
			+ "desc -> " +description+"\n" 
			+ "userIconDataPath -> "+ icon + "\n";
	}
}
