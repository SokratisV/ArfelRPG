using System.Collections;
using UnityEngine;

namespace RPG.Core
{
	public static class CoroutineExtension
	{
		public static void StopCoroutine( this Coroutine coroutine, MonoBehaviour mono )
		{
			if( coroutine != null ) mono.StopCoroutine( coroutine );
		}

		public static Coroutine StartCoroutine( this Coroutine coroutine, MonoBehaviour mono, IEnumerator method )
		{
			coroutine.StopCoroutine( mono );
			return mono.StartCoroutine( method );
		}
	}
}