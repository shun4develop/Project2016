using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyClass;

//クリックしたら詳細画面に飛ぶ画像オブジェクトのベースクラス
//画像オブジェクトの型に応じて派生させる

public abstract class ContentsViewerBase : MonoBehaviour {
	
	public Item Item{ set; get;}
	public bool showCompleted{ set; get;}

	public void init(Item item){
		this.Item = item;
	}

	public abstract void show ();

	public abstract void setTexture (Texture2D tex);


//	public void getImage(){
//		Texture2D tex = ItemData.instance.getContentsTexture2DById(Item.getId ());
//		if (tex != null) {
//			setTexture (tex);
//		}
//	}


}
