using UnityEngine;
using System.Collections;
using ValueObjects;

namespace SessionManager{
	public class Session  {
		public UserVO currentUser {get; set;}
		public string token {get; set;}
		public string uid {get; set;}

	}
}