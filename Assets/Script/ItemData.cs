using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using MyClass;

public class ItemData : MonoBehaviour {

	//シングルトン

	private static ItemData _instance;
	public List<Item> items;
	private Dictionary<int,Texture2D> contents;
	private Dictionary<int,Sprite> contentsSprite;
	private Dictionary<int,Texture2D> thumbnail;

	void Awake(){
		thumbnail = new Dictionary<int, Texture2D> ();
		contents = new Dictionary<int,Texture2D> ();
		contentsSprite = new Dictionary<int, Sprite> ();

	}

	public static ItemData instance {
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<ItemData> ();
			}
			return _instance;
		}
	}

	public void SortById(){
		items.Sort (delegate(Item a, Item b){return a.getId() - b.getId();});
	}
	public void addThumbnail(int id,Texture2D tex){
		thumbnail.Add (id,tex);
	}
	public void addContents(int id,Texture2D tex){
		contents.Add (id,tex);
	}
	public void addSprite(int id,Sprite s){
		contentsSprite.Add (id,s);
	}
	public void SetItems(List<Item> items){
		this.items = items;
		SortById ();
	}
	public Item getItemById(int id){
		for (int i = 0; i < items.Count; i++) {
			if (items [i].getId() == id) {
				return items [i];
			}
		}
		return null;
	}
	public Texture2D getContentsTexture2DById(int id){
		Texture2D tex;
		if (contents.TryGetValue (id, out tex)) {
			return tex;
		} else {
			return null;
		}
	}

	public Sprite getContentsSpriteById(int id){
		Sprite s;
		if (contentsSprite.TryGetValue (id, out s)) {
			return s;
		} else {
			return null;
		}
	}

	public Texture2D getThumbnailTexture2DById(int id){
		Texture2D tex;
		if (thumbnail.TryGetValue (id, out tex)) {
			return tex;
		} else {
			return null;
		}
	
		
	}
}
