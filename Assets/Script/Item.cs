using UnityEngine;
using System.Collections;
using System;
namespace MyClass{
	[Serializable]
	public class Item {
		[SerializeField]
		private int id;
		[SerializeField]
		private string thumbnail; //サムネイル
		[SerializeField]
		private string filepath;  //コンテンツのURL
		[SerializeField]
		private string owner;
		[SerializeField]
		private string desc;
		[SerializeField]
		private string date;
		[SerializeField]
		private string title;
		[SerializeField]
		private bool permitSave;
		[SerializeField]
		private string latitude;
		[SerializeField]
		private string longitude;
		[SerializeField]
		private string type;
		public override string ToString ()
		{
			return "id -> " + this.id + "\n"
				+ "owner -> " + this.owner + "\n"
				+ "filepath -> " + this.filepath + "\n"
				+ "date -> " + this.date + "\n"
				+ "title -> " + this.title + "\n"
				+ "desc -> " + this.desc + "\n"
				+ "permitSave -> " + this.permitSave + "\n"
				+ "latitude -> " + this.latitude + "\n"
				+ "longitude -> " + this.longitude + "\n"
				+ "type -> " + this.type + "\n";
		}
		public int getId(){
			return this.id;
		}
		public string getTitle(){
			return this.title;
		}
		public string getThumbnail(){
			return this.thumbnail;
		}
		public string getFilepath(){
			return filepath;
		}
		public string getOwner(){
			return owner;
		}
		public string getDesc(){
			return desc;
		}
		public string getDate(){
			return date;
		}
		public bool getPermitSave(){
			return permitSave;
		}
		public string getLatitude(){
			return latitude;
		}
		public double getLatitudeParseDouble(){
			return Double.Parse(latitude);
		}
		public string getLongitude(){
			return longitude;
		}
		public double getLongitudeParseDouble(){
			return Double.Parse (longitude);
		}
		public void setPermitSave(bool permit){
			this.permitSave = permit;
		}
	}
}