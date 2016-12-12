using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyClass;

//クリックしたら詳細画面に飛ぶ画像オブジェクトのベースクラス
//画像オブジェクトの型に応じて派生させる

public abstract class ContentsViewerBase : MonoBehaviour {

	protected const string getImageURL = "http://160.16.216.204/~hosoya/puts/get_resources.php";
	public Item Item{ set; get;}
	public bool showCompleted{ set; get;}
	
	public void init(Item item){
		this.Item = item;
	}

	public abstract void show ();
	public abstract void show(string filepath);
	public abstract void show (int id);

	public abstract void setTexture (Texture2D tex);

	public IEnumerator getImage(string filepath){
		WWWForm data = new WWWForm ();
		data.AddField ("filepath", filepath);
		WWW www = new WWW (getImageURL, data);

		yield return www;

		if (www.texture != null) {
			showCompleted = true;
		}

		Texture2D tex = www.texture;

		setTexture (tex);
	}
	public void getImage(){
		Texture2D tex = ItemData.instance.getContentsTexture2DById(Item.getId ());
		if (tex != null) {
			setTexture (tex);
		}
	}

	public void flagChange(){
		this.gameObject.GetComponent<Image> ().sprite  = null;
		showCompleted = false;
	}
}
