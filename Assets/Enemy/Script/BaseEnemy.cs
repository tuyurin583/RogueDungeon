using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour
{
	//アニメーター
	protected Animator animator;
	//リギッドボディー
    Rigidbody rb;
	//NavMeshAgent
	protected NavMeshAgent agent;
	[SerializeField]
	protected float attackRange=5;
	protected bool IsDead = false;
	[Header("HP")]
	[SerializeField]
	//最大HP
	protected float Maxhp = 50f;
	[SerializeField]
	//現在のHP
	protected float currnethp;
	[Header("TL")]
	[SerializeField]
	protected PlayableDirector[] TLAttack;
	[SerializeField]
	protected PlayableDirector TLDamage;
	[SerializeField]
	protected PlayableDirector TLDeath;
	[SerializeField]
	protected GameObject DeathEffect;
	//ドロップするアイテムのプレハブ
	//public GameObject itemPrefab;
	// ドロップ確率
	//public float dropChance = 0.5f; 


	// Start is called before the first frame update
	void Start()
    {
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
		agent=GetComponent<NavMeshAgent>();
	}

	/*
	public void DropItem()
	{
		//設定したアイテムを確率でドロップさせる
		if (Random.value < dropChance)
		{
			Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
		}
	}*/

	protected void DeathEffectActive()
	{
		// デスエフェクトが設定されていればそれを生成
		if (DeathEffect != null)
		{
			Instantiate(DeathEffect, transform.position, transform.rotation);
		}
	}


}
