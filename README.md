# Unity-firebase
##How to use:
1-. Create an empty Game Object, name it Firebase
2-. Add the DBQueries, KeepAlive and Session scripts to it.
3-. Open de the DBQueries file, on the function Start set it to your firebase url.
4-. You can use the already made queries or make your own.

##Make your own queries:
1-. Create it in the shape of an IEnumerator, create it preferrably in the DBQueries file.
2-. Create a string containing the collection you want to query, make sure you have your data indexed from Firebase. Do not include the base url.
3-. Instatiate the class CoroutineWithData so you can access the result from the coroutine GET. Keep reading for more info. Make sure you use yield return.
4-. You can use the customDeserializer, or you can create your own, or just parse the JSON into an IDictionary to get what you want.
See example:

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

