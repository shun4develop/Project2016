using UnityEngine;
using System.Collections;

public class UserOfSNS {
	public string id;
	public string name;
	public string socialType;

	public override string ToString ()
	{
		return "id -> " + id + "\n"
		+ "name -> " + name + "\n";
	}
}
