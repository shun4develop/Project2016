using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyClass;
public class ContentsAboutInfomationUI : MonoBehaviour {

	public ContentsOfThumbnail thumbnail;
	public Text owner;
	public Text title;
	public Text date;
	public Text statusLog;

	public void init(Item item){
		thumbnail.init (item);
		thumbnail.show ();
		owner.text = item.getOwner();
		title.text = item.getTitle ();
		date.text = item.getDate ().Substring(0,10);

		if (!string.IsNullOrEmpty (item.getFilepath ())) {
			statusLog.text = "<color=red>AR画面に出現中!!</color>";
		} else {
			statusLog.text = "少し距離があるようです";
		}
	}
}
