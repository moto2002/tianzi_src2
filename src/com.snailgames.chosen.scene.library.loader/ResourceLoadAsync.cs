using System;
using UnityEngine;

namespace com.snailgames.chosen.scene.library.loader
{
	public class ResourceLoadAsync : MonoBehaviour
	{
		public delegate void OnResourceLoadCallBack(string filename, UnityEngine.Object o);

		private static ResourceLoadAsync instance;

		public static ResourceLoadAsync Instance
		{
			get
			{
				if (ResourceLoadAsync.instance == null)
				{
					GameObject gameObject = new GameObject(Config.MessageName);
					ResourceLoadAsync.instance = gameObject.AddComponent<ResourceLoadAsync>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return ResourceLoadAsync.instance;
			}
		}

		public void Load(string filename, ResourceLoadAsync.OnResourceLoadCallBack callback)
		{
			UnityEngine.Object @object = Resources.Load(filename);
			if (@object != null)
			{
				callback(filename, @object);
			}
			else
			{
				callback(filename, null);
			}
		}
	}
}
