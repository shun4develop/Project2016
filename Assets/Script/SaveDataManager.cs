using UnityEngine;
using System.Collections;
using MyLibrary;
namespace MyManagers{
	public class SaveDataManager{
		static string userNameDataKey = "Ptnyrw_PLKpzDJgGYtsLjyZkEDzJfhwe";
		static string tokenDataKey = "dRbVArxhQNnGBdrajMKVMpPLYLnusPtW";
		static string userInfoDataKey = "fkm3De26GDccdqMBvdltoyEfF45";
		static string userIconDataKey = "LKpzDrajMKV6GDccdltoyZkEDzJfhQNn";
		public static string loadUserName(){
			try{
				return Crypt.Decrypt(PlayerPrefs.GetString (userNameDataKey)).Replace("\0","");
			}catch{
				return "";
			}
		}
		public static string loadToken(){
			try{
				return Crypt.Decrypt (PlayerPrefs.GetString(tokenDataKey)).Replace("\0","");
			}catch{
				return "";
			}
		}
		public static Profile loadUserInfo(){
			try{
				string json = Crypt.Decrypt (PlayerPrefs.GetString(userInfoDataKey));
				return JsonUtility.FromJson<Profile>(json);
			}catch{
				return null;
			}
		}
		public static Sprite loadUserIcon(){
			try{
				string base64str = Crypt.Decrypt (PlayerPrefs.GetString(userIconDataKey));
				Debug.Log(base64str);
				byte[] bytes = System.Convert.FromBase64String(base64str);
				// bytes -> texture
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImage(bytes);
				Debug.Log("test");
				// texture -> sprite
				return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
			}catch{
				return null;
			}
		}
		public static void saveUserIcon(Sprite icon){
			string base64str = System.Convert.ToBase64String(icon.texture.EncodeToPNG());
			PlayerPrefs.SetString (userIconDataKey,Crypt.Encrypt(base64str));
			PlayerPrefs.Save ();
		}
		public static void saveUserInfo(Profile info){
			PlayerPrefs.SetString (userInfoDataKey,Crypt.Encrypt(JsonUtility.ToJson(info)));
			PlayerPrefs.Save ();
		}
		public static void saveUserName(string name){
			PlayerPrefs.SetString (userNameDataKey,Crypt.Encrypt(name.Replace("\n","")));
			PlayerPrefs.Save ();
		}
		public static void saveToken(string token){
			PlayerPrefs.SetString (tokenDataKey,Crypt.Encrypt(token.Replace("\n","")));
			PlayerPrefs.Save ();
		}
		public static void deleteData(){
			PlayerPrefs.DeleteAll ();
		}
	}
}
