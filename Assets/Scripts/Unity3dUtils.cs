// dnSpy decompiler from Assembly-CSharp.dll class: Unity3dUtils
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Unity3dUtils
{
	public static T FindFirstComponentOfBaseTypeInObject<T>(GameObject gameObject) where T : class
	{
		Component[] components = gameObject.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (component is T)
			{
				return component as T;
			}
		}
		return (T)((object)null);
	}

	public static List<T> FindComponentsOfBaseTypeInObject<T>(GameObject gameObject) where T : class
	{
		List<T> list = new List<T>();
		Component[] components = gameObject.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (component is T)
			{
				list.Add(component as T);
			}
		}
		return list;
	}

	public static I GetInterfaceComponent<I>(GameObject gameObject) where I : class
	{
		return gameObject.GetComponent(typeof(I)) as I;
	}

	public static I GetInterfaceComponent<I>(MonoBehaviour monoBehavior) where I : class
	{
		return monoBehavior.GetComponent(typeof(I)) as I;
	}

	public static List<I> FindObjectsOfInterface<I>() where I : class
	{
		MonoBehaviour[] array = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		List<I> list = new List<I>();
		foreach (MonoBehaviour monoBehaviour in array)
		{
			I i2 = monoBehaviour.GetComponent(typeof(I)) as I;
			if (i2 != null)
			{
				list.Add(i2);
			}
		}
		return list;
	}

	public static Bounds GetBoundsOfObjectWithItsChildren(GameObject obj)
	{
		Bounds result = (!(null == obj.GetComponent<Renderer>())) ? obj.GetComponent<Renderer>().bounds : new Bounds(obj.transform.position, Vector3.zero);
		Unity3dUtils.AddChildrenToBounds(ref result, obj);
		return result;
	}

	private static void AddChildrenToBounds(ref Bounds bounds, GameObject obj)
	{
		IEnumerator enumerator = obj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				Transform transform = (Transform)obj2;
				if (null != transform.GetComponent<Renderer>())
				{
					bounds.Encapsulate(transform.GetComponent<Renderer>().bounds);
				}
				Unity3dUtils.AddChildrenToBounds(ref bounds, transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
