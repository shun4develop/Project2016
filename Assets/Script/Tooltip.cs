using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyClass;
public class Tooltip : MonoBehaviour {
	public ContentsOfThumbnail thumbnail;
	public Text statusLog;

	public void init(Item item){
		thumbnail.init (item);
		thumbnail.show ();

		if (!string.IsNullOrEmpty (item.getFilepath ())) {
			statusLog.text = "AR画面に出現中!!";
		} else {
			statusLog.text = "少し距離があるようです";
		}
	}
}
