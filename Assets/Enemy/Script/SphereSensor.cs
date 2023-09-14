using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;

public class SphereSensor : MonoBehaviour
{
	//private SphereCollider searchArea = default;
	[SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();
	
	

	private void OnTriggerStay(Collider target)
	{
		if (target.tag == "Player")
		{
			onTriggerStay.Invoke(target);
		}
	}

	[Serializable]
	public class TriggerEvent : UnityEvent<Collider>
	{
	}


}
