// dnSpy decompiler from Assembly-CSharp.dll class: CheckedMonoBehaviour
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class CheckedMonoBehaviour : MonoBehaviour
{
	private void Awake()
	{
		if (!Attribute.IsDefined(base.GetType(), typeof(CanBeAddedInRuntime)))
		{
			this.InitCheckedMonoBehaviour();
		}
		else
		{
			UnityEngine.Debug.Log("Component " + base.GetType().Name + " can be added in runtime, Awake skips InitCheckedMonoBehaviour");
		}
		this.OnAwake();
	}

	public void InitCheckedMonoBehaviour()
	{
		this.LinkRequiredComponents();
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			this.CheckSetup();
			this.InitializeOtherScriptsOnThisObject();
			this.InitializeLinkedObjects();
		}
	}

	protected virtual void OnAwake()
	{
	}

	private void CheckSetup()
	{
		FieldInfo[] fields = base.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (this.ShouldCheckField(fieldInfo))
			{
				TypeCode typeCode = Type.GetTypeCode(fieldInfo.FieldType);
				switch (typeCode)
				{
				case TypeCode.Single:
				case TypeCode.Double:
					break;
				default:
					if (typeCode == TypeCode.Object)
					{
						if (fieldInfo.FieldType.IsArray)
						{
							this.CheckArray(fieldInfo);
						}
						else
						{
							this.CheckReference(fieldInfo);
						}
						goto IL_AF;
					}
					if (typeCode != TypeCode.Int32)
					{
						goto IL_AF;
					}
					break;
				case TypeCode.String:
					this.CheckString(fieldInfo);
					goto IL_AF;
				}
				this.CheckNumericValue(fieldInfo);
			}
			IL_AF:;
		}
	}

	private bool ShouldCheckField(FieldInfo field)
	{
		return field.IsPublic && field.DeclaringType.IsSubclassOf(typeof(CheckedMonoBehaviour)) && !Attribute.IsDefined(field, typeof(HideInInspector)) && !Attribute.IsDefined(field, typeof(NonSerializedAttribute));
	}

	private void CheckNumericValue(FieldInfo field)
	{
		double num = Convert.ToDouble(field.GetValue(this));
		if (num == 0.0)
		{
			if (!Attribute.IsDefined(field, typeof(CanBeZero)))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					field.Name,
					" is ",
					num,
					" in ",
					this.PrintScriptLocation(),
					". It can't be 0. You should set it to something != 0 in the Inspector"
				}));
			}
		}
		else if (num < 0.0 && !Attribute.IsDefined(field, typeof(CanBeNegative)))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				field.Name,
				" is ",
				num,
				" in ",
				this.PrintScriptLocation(),
				". It can't be < 0. You should set it to something >= 0 in the Inspector"
			}));
		}
	}

	private void CheckString(FieldInfo field)
	{
		string text = field.GetValue(this) as string;
		if ((text == null || text.Length == 0) && !Attribute.IsDefined(field, typeof(CanBeEmpty)))
		{
			UnityEngine.Debug.LogError(field.Name + " is empty in " + this.PrintScriptLocation() + ". You should set some text for it in the Inspector");
		}
	}

	private void CheckReference(FieldInfo field)
	{
		if (field.GetValue(this) == null && !Attribute.IsDefined(field, typeof(CanBeNull)))
		{
			UnityEngine.Debug.LogError(field.Name + " is null in " + this.PrintScriptLocation() + ". You should set it in the Inspector");
		}
	}

	private void CheckArray(FieldInfo field)
	{
		Array array = field.GetValue(this) as Array;
		if (array == null)
		{
			return;
		}
		if (array.Length == 0 && !Attribute.IsDefined(field, typeof(CanBeEmpty)))
		{
			UnityEngine.Debug.LogError(field.Name + " array empty in " + this.PrintScriptLocation() + ". Make sure you have filled the array in Inspector");
		}
		if (Array.IndexOf(array, null) != -1 && !Attribute.IsDefined(field, typeof(CanHaveNullElements)))
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"A null element found in array ",
				field.Name,
				" in ",
				this.PrintScriptLocation(),
				". Make sure you set up the links to all array elemens in Inspector"
			}));
		}
	}

	private string PrintScriptLocation()
	{
		return base.GetType().Name + " on object " + base.gameObject.name;
	}

	public T GetRequiredComponent<T>() where T : Component
	{
		T component = base.GetComponent<T>();
		if (null == component)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Component ",
				typeof(T).Name,
				"is required by ",
				this.PrintScriptLocation(),
				" but the component is missing"
			}));
		}
		return component;
	}

	public Component GetRequiredComponent(Type type)
	{
		Component component = base.GetComponent(type);
		if (null == component)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Component ",
				type.Name,
				" is required by ",
				this.PrintScriptLocation(),
				" but the component is missing"
			}));
		}
		return component;
	}

	public T[] GetRequiredComponents<T>() where T : Component
	{
		T[] components = base.GetComponents<T>();
		if (components == null || components.Length == 0)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Component ",
				typeof(T).Name,
				"is required by ",
				this.PrintScriptLocation(),
				" but the component is missing"
			}));
		}
		return components;
	}

	public Component[] GetRequiredComponents(Type type)
	{
		Component[] components = base.GetComponents(type);
		if (components == null || components.Length == 0)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Component ",
				type.Name,
				" is required by ",
				this.PrintScriptLocation(),
				" but the component is missing"
			}));
		}
		return components;
	}

	private void LinkRequiredComponents()
	{
		this.LinkRequiredComponentsOnHierarchyLevel(base.GetType());
	}

	private void LinkRequiredComponentsOnHierarchyLevel(Type level)
	{
		RequireComponent[] required = Attribute.GetCustomAttributes(level, typeof(RequireComponent)) as RequireComponent[];
		FieldInfo[] fields = level.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (Attribute.IsDefined(fieldInfo, typeof(AutoLink)))
			{
				if (!fieldInfo.FieldType.IsArray)
				{
					Type fieldType = fieldInfo.FieldType;
					if (this.IsComponentAmongRequired(fieldType, required))
					{
						fieldInfo.SetValue(this, this.GetRequiredComponent(fieldInfo.FieldType));
					}
				}
				else
				{
					Type elementType = fieldInfo.FieldType.GetElementType();
					if (this.IsComponentAmongRequired(elementType, required))
					{
						Component[] requiredComponents = this.GetRequiredComponents(fieldInfo.FieldType.GetElementType());
						Array array2 = Array.CreateInstance(elementType, requiredComponents.Length);
						Array.Copy(requiredComponents, array2, requiredComponents.Length);
						fieldInfo.SetValue(this, array2);
					}
				}
			}
		}
		this.LinkComponentsInBaseClassOf(level);
	}

	private bool IsComponentAmongRequired(Type type, RequireComponent[] required)
	{
		foreach (RequireComponent requireComponent in required)
		{
			if (type == requireComponent.m_Type0 || type == requireComponent.m_Type1 || type == requireComponent.m_Type2)
			{
				return true;
			}
		}
		return false;
	}

	private void LinkComponentsInBaseClassOf(Type level)
	{
		if (level.BaseType.IsSubclassOf(typeof(CheckedMonoBehaviour)))
		{
			this.LinkRequiredComponentsOnHierarchyLevel(level.BaseType);
		}
	}

	private void InitializeOtherScriptsOnThisObject()
	{
		base.gameObject.BroadcastMessage("InitCheckedMonoBehaviour", SendMessageOptions.DontRequireReceiver);
	}

	private void InitializeLinkedObjects()
	{
		FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(CheckedMonoBehaviour)))
			{
				CheckedMonoBehaviour checkedMonoBehaviour = fieldInfo.GetValue(this) as CheckedMonoBehaviour;
				if (null != checkedMonoBehaviour)
				{
					checkedMonoBehaviour.InitCheckedMonoBehaviour();
				}
			}
			else if (fieldInfo.FieldType.IsArray && fieldInfo.FieldType.GetElementType().IsSubclassOf(typeof(CheckedMonoBehaviour)))
			{
				Array array2 = fieldInfo.GetValue(this) as Array;
				if (array2 != null)
				{
					IEnumerator enumerator = array2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							CheckedMonoBehaviour checkedMonoBehaviour2 = obj as CheckedMonoBehaviour;
							if (null != checkedMonoBehaviour2)
							{
								checkedMonoBehaviour2.InitCheckedMonoBehaviour();
							}
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
		}
	}

	private bool isInitialized;
}
