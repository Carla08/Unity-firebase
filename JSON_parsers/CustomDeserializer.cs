using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ValueObjects;
using MiniJSON;

namespace CustomDeserializers {
	public class CustomDeserializer : MonoBehaviour {

		public List<UserVO> deserializeToListUsers (string response){
			IDictionary search = Json.Deserialize(response) as IDictionary;
			List<UserVO> users = new List<UserVO>();
			foreach (var key in search.Keys){
				IDictionary temp =(IDictionary) search[key];
				UserVO user = new UserVO();
				user.key = key.ToString();
				user.username = temp["username"].ToString(); 
				user.email = temp["email"].ToString();
				user.password = temp["password"].ToString();
				user.isTeacher = (bool)temp["isTeacher"];
				List<object> sbj = temp["subjects"] as List<object>;
				List<string> subjects = new List<string>();
				foreach(var item in sbj) {
					subjects.Add(item.ToString());
				}
				user.subjects = subjects;
				user.status = false;
				users.Add(user);
			}
			return users;
		}
		public UserVO deserializeToUser (string response){
			IDictionary search = Json.Deserialize(response) as IDictionary;
			UserVO user = new UserVO();
			foreach (var key in search.Keys){
				IDictionary temp =(IDictionary) search[key];
				user.key = key.ToString();
				user.username = temp["username"].ToString(); 
				user.email = temp["email"].ToString();
				user.password = temp["password"].ToString();
				user.isTeacher = (bool)temp["isTeacher"];
				List<object> sbj = temp["subjects"] as List<object>;
				List<string> subjects = new List<string>();
				foreach(var item in sbj) {
					subjects.Add(item.ToString());
				}
				user.subjects = subjects;
				user.status = false;
		}
			return user;
		}
		public SubjectVO deserializeToSubject (string response){
			IDictionary search = Json.Deserialize(response) as IDictionary;
			SubjectVO subject = new SubjectVO();
			subject.name = search["name"].ToString();
			subject.group_id = search["group"].ToString();
			return subject;
		}
		public List<ContentVO> deserializeToListContent (string response) {
			IDictionary search = Json.Deserialize(response) as IDictionary;
			List<ContentVO> contents = new List<ContentVO>();
			foreach (var key in search.Keys){
				IDictionary temp =(IDictionary) search[key];
				ContentVO content = new ContentVO();
				content.key = key.ToString();
				content.name = temp["name"].ToString(); 
				content.description = temp["description"].ToString();
				content.content = temp["content"].ToString();
				content.subject_id = temp["subject"].ToString();
				contents.Add(content);
				Debug.Log("Content: " + content.name);
			}
			return contents;
		}
		public PointerVO deserializeToPointer (string response) {
			IDictionary search = Json.Deserialize(response) as IDictionary;
			PointerVO pointer = new PointerVO();
			foreach (var key in search.Keys){
				IDictionary temp =(IDictionary) search[key];
				pointer.key = key.ToString();
				pointer.content_id = temp["content"].ToString(); 
				pointer.subject_id = temp["subject"].ToString();
			}
			return pointer;
		}

		public GroupVO deserializeToGroup (string response) {
			IDictionary search = Json.Deserialize(response) as IDictionary;
			GroupVO group = new GroupVO();
			group.name = search["group"].ToString();
			group.grade = search["grade"].ToString();
			return group;
		}
	}//Class end
}//Namespace end
