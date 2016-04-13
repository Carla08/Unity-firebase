using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FirebaseQueries;
using CoroutinesManager;
using SessionManager;

public class UserSession : MonoBehaviour {
	GameObject input_username;
	GameObject input_password;
	DBQueries query;
	Session session;

	// Use this for initialization
	void Start () {
		input_username = GameObject.Find("userName");
		input_password = GameObject.Find ("userPassword");
		query = this.GetComponent<DBQueries>();
		session = new Session();
	}

	public IEnumerator LogIn_enum (){
		string user = input_username.GetComponent<InputField>().text;
		string password = input_password.GetComponent<InputField>().text;
		CoroutineWithData cd = new CoroutineWithData(this,query.LogIn(user,password));
		yield return cd.coroutine;
		//Debug.Log (cd.result.ToString());
		session = (Session)cd.result;
		Debug.Log("Current user: " + session.currentUser.username);
		//Application.LoadLevel();
	}
	public IEnumerator unit_testing () {
		CoroutineWithData cd = new CoroutineWithData(this, query.test());
		yield return cd.coroutine;
	}

	public void LogInButton () {
		//StartCoroutine(LogIn_enum());
		StartCoroutine(unit_testing());
	}
}
