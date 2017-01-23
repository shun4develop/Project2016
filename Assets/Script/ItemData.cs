using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using MyClass;

public class ItemData {

	//シングルトン

	private static ItemData _instance;
	public List<Item> locationItems;
	public List<Item> bagItems;
	private Dictionary<int,Texture2D> contents;
	private Dictionary<int,Sprite> contentsSprite;
	private Dictionary<int,Texture2D> thumbnail;

	private ItemData(){
		thumbnail = new Dictionary<int, Texture2D> ();
		contents = new Dictionary<int,Texture2D> ();
		contentsSprite = new Dictionary<int, Sprite> ();

		locationItems = new List<Item> ();
		bagItems = new List<Item> ();

	}
		
	public static ItemData instance {
		get{
			if (_instance == null) {
				_instance = new ItemData();
			}
			return _instance;
		}
	}

	public void sortById(List<Item> items){
		locationItems.Sort (delegate(Item a, Item b){return a.getId() - b.getId();});
	}


	public void addThumbnail(int id,Texture2D tex){
		try{
			thumbnail.Add (id,tex);
		}catch{
			return;
		}
	}
	public void addContents(int id,Texture2D tex){
		try{
			contents.Add (id,tex);
		}catch{
			return;
		}
	}
	public void addSprite(int id,Sprite s){
		try{
			contentsSprite.Add (id,s);
		}catch{
			return;
		}
	}

	public void SetLocationItems(List<Item> items){
		this.locationItems = items;
		sortById (items);
	}
	public void SetBagItems(List<Item> items){
		this.bagItems = items;
		sortById (items);
	}



	public Item getLocationItemById(int id){
		for (int i = 0; i < locationItems.Count; i++) {
			if (locationItems [i].getId() == id) {
				return locationItems [i];
			}
		}
		return null;
	}

	public Item getBagItemById(int id){
		for (int i = 0; i < bagItems.Count; i++) {
			if (bagItems [i].getId() == id) {
				return bagItems [i];
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

	//バッグの中に同じアイテムがないか調べる
	public bool checkOverlapItemById(int id){
		for (int i = 0; i < bagItems.Count; i++) {
			if (bagItems [i].getId() == id) {
				return false;
			}
		}
		return true;
	}

	public void saveContent(Item item){
		bagItems.Add (item);
		sortById (bagItems);
	}

	public void deleteContentById(int id){
		for (int i = 0; i < bagItems.Count; i++) {
			if (bagItems [i].getId() == id) {
				bagItems.RemoveAt(i);
			}
		}
		contents.Remove (id);
		thumbnail.Remove (id);
		contentsSprite.Remove (id);
	}
}
