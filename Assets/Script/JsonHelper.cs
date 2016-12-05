using UnityEngine;
using System.Collections.Generic;
using System;
using MyClass;
namespace MyLibrary{
	public class JsonHelper{
		public static List<T> ListFromJson<T>(string json)
		{
			//配列のリストであることを表すために下記の文字列を付け加える
			var newJson = "{ \"list\": " + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
			return wrapper.list;
		}
		public static Item ItemFromJson(string json){
			Item item = JsonUtility.FromJson<Item> (json);
			return item;
		}
		public static UserOfTwitter TwitterUserFromJson(string json){
			UserOfTwitter user = JsonUtility.FromJson<UserOfTwitter> (json);
			return user;
		}
		public static UserOfFacebook FacebookUserFromJson(string json){
			UserOfFacebook user = JsonUtility.FromJson<UserOfFacebook> (json);
			return user;
		}
		public static string DictionaryToJson(Dictionary<string,string> data){
			string json = JsonUtility.ToJson(new Serialization<string,string>(data));
			return json;
		}
		public static Dictionary<string,string> JsonToDictionary(string json){
			Dictionary<string,string> data = JsonUtility.FromJson<Serialization<string,string>>(json).ToDictionary();
			return data;
		}
		[Serializable]
		private class Wrapper<T>
		{
			public List<T> list;
		}
		[Serializable]
		public class Serialization<T>
		{
			[SerializeField]
			List<T> target;
			public List<T> ToList() { return target; }

			public Serialization(List<T> target)
			{
				this.target = target;
			}
		}

		// Dictionary<TKey, TValue>
		[Serializable]
		public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
		{
			[SerializeField]
			List<TKey> keys;
			[SerializeField]
			List<TValue> values;

			Dictionary<TKey, TValue> target;
			public Dictionary<TKey, TValue> ToDictionary() { return target; }

			public Serialization(Dictionary<TKey, TValue> target)
			{
				this.target = target;
			}

			public void OnBeforeSerialize()
			{
				keys = new List<TKey>(target.Keys);
				values = new List<TValue>(target.Values);
			}

			public void OnAfterDeserialize()
			{
				var count = Math.Min(keys.Count, values.Count);
				target = new Dictionary<TKey, TValue>(count);
				for (var i = 0; i < count; ++i)
				{
					target.Add(keys[i], values[i]);
				}
			}
		}
	}
}