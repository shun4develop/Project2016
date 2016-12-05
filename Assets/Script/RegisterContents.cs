using UnityEngine;
using System;
[Serializable]
public class RegisterContents{
	[SerializeField]
	private string data; //base64
	[SerializeField]
	private string desc;
	[SerializeField]
	private string title;
	[SerializeField]
	private double lat;
	[SerializeField]
	private double lon;
	[SerializeField]
	private string owner;
	[SerializeField]
	private bool permitSave;
	[SerializeField]
	private string type;

	public RegisterContents(string data,string desc,string title,double lat,double lon,string owner,bool permitSave, string type){
		this.data = data;
		this.desc = desc;
		this.title = title;
		this.lat = lat;
		this.lon = lon;
		this.owner = owner;
		this.permitSave = permitSave;
		this.type = type;
	}

	public void setTitle(string title){
		this.title = title;
	}

	public void setDesc(String desc){
		this.desc = desc;
	}

	public void setPermitSave(bool permitsave){
		this.permitSave = permitsave;
	}

	public string getData(){
		return this.data;
	}

	public string getTitle(){
		return this.title;
	}

	public string getDesc(){
		return desc;
	}

	public bool getPermitSave(){
		return permitSave;
	}

	public string getType(){
		return type;
	}

	public double getLatitude(){
		return lat;
	}

	public double getLongitude(){
		return lon;
	}

	//禁断のメソッド
	public string asfdhjkl()
	{
		return desc + title + lat + lon + owner + permitSave + type;
	}
}
