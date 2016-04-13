using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

namespace ValueObjects {
	public class UserVO : MonoBehaviour {
		public string key {get; set;}
		public string username {get; set;}
		public string email {get; set;}
		public bool  isTeacher {get; set;}
		public string password {get; set;}
		public List<string> subjects {get; set;}
		public bool status {get; set;}

		public void Create (string key, string username, string email, string password, bool isTeacher, bool status, List<string> subjects){
			this.key = key;
			this.username = username;
			this.email = email;
			this.password = password;
			this.isTeacher = isTeacher;
			this.status = status;
			this.subjects = subjects;

		}

	}
}
