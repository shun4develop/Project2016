using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace MyLibrary
{
	public static class UriStringDecoder
	{
		static Dictionary<string,string> code = new Dictionary<string, string>()
		{
			{"%20"," "},
			{"%21", "!"},
			{"%22", "\""},
			{"%23", "#"},
			{"%24", "$"},
			{"%25", "%"},
			{"%26", "&"},
			{"%27", "'"},
			{"%28", "("},
			{"%29", ")"},
			{"%3D", "="},
			{"%5E", "^"},
			{"%7E", "~"},
			{"%5C", "\\"},
			{"%7C", "|"},
			{"%60", "`"},
			{"%40", "@"},
			{"%5B", "["},
			{"%5D", "]"},
			{"%7B", "{"},
			{"%7D", "}"},
			{"%3B", ";"},
			{"%2B", "+"},
			{"%3A", ":"},
			{"%2A", "*"},
			{"%2C", ","},
			{"%3C", "<"},
			{"%3E", ">"},
			{"%2F", "/"},
			{"%3F", "?"},
		};
	

		public static string decode(string str){

			foreach (string key in code.Keys) {
				str = str.Replace (key, code [key]);
			}

			return str;
		}
	}
}