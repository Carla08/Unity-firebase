﻿using UnityEngine;
using System.Collections;
//http://answers.unity3d.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html

namespace CoroutinesManager {
	public class CoroutineWithData {
		public Coroutine coroutine { get; private set; }
		public object result;
		private IEnumerator target;
		public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
			this.target = target;
			this.coroutine = owner.StartCoroutine(Run());
		}
		
		private IEnumerator Run() {
			while(target.MoveNext()) {
				result = target.Current;
				yield return result;
			}
		}
	}
}
