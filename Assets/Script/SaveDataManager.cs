using UnityEngine;
using System.Collections;
using MyLibrary;
namespace MyManagers{
	public class SaveDataManager{
		static string userNameDataKey = "Ptnyrw_PLKpzDJgGYtsLjyZkEDzJfhwe";
		static string tokenDataKey = "dRbVArxhQNnGBdrajMKVMpPLYLnusPtW";
		static string userInfoDataKey = "fkm3De26GDccdqMBvdltoyEfF45";
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
