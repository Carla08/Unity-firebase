using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseUnity {
	public class FirebaseAPI{
		IFirebase firebase;
		public void initialize(string url) {
			firebase = Firebase.CreateNew(base_url);
			this.base_url = url;
		}
		public string base_url {get; set;}

		public IEnumerator GET(string collection) {
			Debug.Log("Line 16 of FirebaseAPI: GET called");
			string url = base_url+collection;
			Debug.Log ("Line 16 Firebase API: URL:" + url);
			WWW www = new WWW(url);
			float elapsedTime = 0.0f;
			while (!www.isDone) {
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= 10.0f) {break;}
				yield return null;  
			}
			if (!www.isDone || !string.IsNullOrEmpty(www.error)) {
				Debug.LogError(string.Format("Fail Whale!\n{0}", www.error));
			}
			yield return www.text;
			Debug.Log ("JSON:" + www.text );
		}

		public IEnumerator LogIn(string user, string pw){
			string uid = "";
			firebase.AuthWithPassword(user,pw, (AuthData auth) => {
				Debug.Log("Auth ID: " + auth.Uid);
					uid = auth.Uid + "," + auth.Token + "," +auth.Expiration;
				}, (FirebaseError e) =>{
					uid = "";
				}
			);
			yield return uid;
		}
	}//Class end
}//end of namespace