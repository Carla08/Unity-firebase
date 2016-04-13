using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using FirebaseUnity;
using CoroutinesManager;
using ValueObjects;
using CustomDeserializers;
using SessionManager;

namespace FirebaseQueries {
	public class DBQueries: MonoBehaviour  {
		//Carla's first queries :')
		FirebaseUnity.FirebaseAPI myFirebase;
		CustomDeserializers.CustomDeserializer customDeserializer;
		IFirebase firebase;

		//Init Firebase API
		public void Start () {
			Debug.Log("DBQueries-Line 16: Start called");
			myFirebase = new FirebaseUnity.FirebaseAPI();
			firebase= Firebase.CreateNew("https://popping-fire-7321.firebaseio.com/");
			myFirebase.initialize("https://popping-fire-7321.firebaseio.com/");
			customDeserializer = new CustomDeserializer();
		}
		public IEnumerator test () {
			//UNIT TESTING ZONE:
			CoroutineWithData cd = new CoroutineWithData(this, GetUserByName("carla"));
			yield return cd.coroutine;
			UserVO testuser = cd.result as UserVO;
			//UNIT TESTING ZONE:
			CoroutineWithData cd1 = new CoroutineWithData(this, GetUserSubjects(testuser.subjects));
			yield return cd1.coroutine;
			IList<SubjectVO> testsubs = cd1.result as List<SubjectVO>;
			CoroutineWithData cd2 = new CoroutineWithData(this, GetSubjectContentList(testsubs[0]));
			yield return cd2.coroutine;
			IList<ContentVO> contents = cd2.result as List<ContentVO>;
			CoroutineWithData cd3 = new CoroutineWithData(this, GetPointerByContent(contents[0]));
			yield return cd3.coroutine;
			PointerVO pointer = cd3.result as PointerVO;
			CoroutineWithData cd4 = new CoroutineWithData(this, GetPointerBySubject(testsubs[0]));
			yield return cd4.coroutine;
			PointerVO pointer1 = cd4.result as PointerVO;
			CoroutineWithData cd5 = new CoroutineWithData(this, GetUserGroups(testuser));
			yield return cd5.coroutine;
			IList<GroupVO> groups = cd5.result as List<GroupVO>;
		}

		//Get Users
		public IEnumerator GetUsers() {
			Debug.Log ("Firabase baseUrl: " + myFirebase.base_url);
			CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET("user.json"));
			yield return cd.coroutine;
			IList<UserVO> users = customDeserializer.deserializeToListUsers(cd.result.ToString());
			yield return users;

		}
		public IEnumerator GetUserByName(string name) {
			//'https://popping-fire-7321.firebaseio.com/user.json?orderBy="username"&equalTo="carla"&print=pretty'
			string url = "user.json?orderBy=\"username\"&equalTo=\"" + name +"\"";
			Debug.Log("Url for user: " + url);
			CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET(url));
			yield return cd.coroutine;
			UserVO user = customDeserializer.deserializeToUser(cd.result.ToString());
			//Debug.Log ("User mail: " + user.email);
			yield return user;
		}
		public IEnumerator LogIn(string username,string password) {
			CoroutineWithData cd = new CoroutineWithData(this, GetUserByName(username));
			yield return cd.coroutine;
			UserVO user = cd.result as UserVO;
			Session session = new Session();
			bool isDone = false;
			float elapsedTime = 0.0f;
			string uid = "";
			string token = "";
			firebase.AuthWithPassword(user.email, password, (AuthData auth) => {
				Debug.Log("Log In Success");
				uid = auth.Uid;
				token = auth.Token;
				isDone = true;
				}, (FirebaseError e) =>{
					session = null;
					isDone = true;
				}	
			);
			while (!isDone){
				elapsedTime += Time.deltaTime;
				if(elapsedTime >= 10.0f){break;}
				yield return null;
			}
			if(isDone && string.IsNullOrEmpty(uid)){
				Debug.Log("error on login");
			}
			session.uid = uid;
			session.token = token;
			session.currentUser = user;
			Debug.Log ("Session created with UID: " + session.uid);
			yield return session;
		} 
		public IEnumerator GetUserSubjects (List<string> user_subjects){
			IList<SubjectVO> usersubs = new List<SubjectVO>();
			foreach (var subject in user_subjects){
				string url = "subject/" + subject + ".json";
				Debug.Log ("Subject url: " + url);
				CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET(url));
				yield return cd.coroutine;
				SubjectVO _subject = customDeserializer.deserializeToSubject(cd.result.ToString());
				_subject.key = subject;
				Debug.Log("Subject:" + _subject.name);
				usersubs.Add(_subject);
			}
			yield return usersubs;
		} 
		public IEnumerator GetSubjectContentList (SubjectVO subject) {
			//'https://popping-fire-7321.firebaseio.com/content.json?orderBy="subject"&equalTo="-KE7_i9kraAcrPa5XwI3"&print=pretty'
			string url = "content.json?orderBy=\"subject\"&equalTo=\"" + subject.key + "\"";
			CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET(url));
			yield return cd.coroutine;
			IList<ContentVO> contents = customDeserializer.deserializeToListContent(cd.result.ToString());
			yield return contents;
		}
		public IEnumerator GetPointerByContent (ContentVO content){
			//'https://popping-fire-7321.firebaseio.com/pointer.json?orderBy="content"&equalTo="-KAGMywVBTFkPIHQNzL9"&print=pretty'
			string url = "pointer.json?orderBy=\"content\"&equalTo=\"" + content.key + "\"";
			CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET(url));
			yield return cd.coroutine;
			PointerVO pointer = customDeserializer.deserializeToPointer(cd.result.ToString());
			Debug.Log("Pointer from Content: " + pointer.content_id);
			yield return pointer;
		}
		public IEnumerator GetPointerBySubject (SubjectVO subject){
			//https://popping-fire-7321.firebaseio.com/pointer.json?orderBy="subject"&equalTo="-KE7_i9kraAcrPa5XwI3"&print=pretty'
			string url = "pointer.json?orderBy=\"subject\"&equalTo=\"" + subject.key + "\"";
			CoroutineWithData cd = new CoroutineWithData(this, myFirebase.GET(url));
			yield return cd.coroutine;
			PointerVO pointer = customDeserializer.deserializeToPointer(cd.result.ToString());
			Debug.Log("Pointer from Subject: " + pointer.subject_id);
			yield return pointer;
		}
		public IEnumerator GetUserGroups (UserVO user) {
			CoroutineWithData cd = new CoroutineWithData(this, GetUserSubjects(user.subjects));
			yield return cd.coroutine;
			IList<SubjectVO> user_subjects = cd.result as List<SubjectVO>;
			IList<GroupVO> user_groups = new List<GroupVO>();
			foreach (var subject in user_subjects) {
				string url = "group/" + subject.group_id + ".json";
				Debug.Log ("Groups url: " + url);
				CoroutineWithData cd1 = new CoroutineWithData(this, myFirebase.GET(url));
				yield return cd1.coroutine;
				GroupVO group = customDeserializer.deserializeToGroup(cd1.result.ToString());
				group.key = subject.group_id;
				user_groups.Add(group);
			}
			Debug.Log("Carla is in: " + user_groups[0].name);
			yield return user_groups;
		}

		
	} //class end
}//Namespace end
