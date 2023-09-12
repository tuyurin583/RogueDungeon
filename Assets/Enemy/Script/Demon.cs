using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : BaseEnemy
{
    public enum DemonState
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Attack3,
        ModeChange,
        Damage,
        Death,
    }
    public DemonState state = DemonState.Idle;

    //罠のプレハブ
    [SerializeField]
    public GameObject bombPrefab;
	//罠の生成範囲
    [SerializeField]
	public float bombSpawnRange = 10f;
	//罠の生成感覚
	[SerializeField]
    public float bombSpawnInterval = 5f;
    //罠の生成タイマー
    private float bombSpawnTimer;
    //罠の最大値
    [SerializeField]
    public int MaxBombCount = 10;
    //罠の生成数
    public int currnetBombCount = 0;
   

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DemonState.Idle:
                IdleState();
                break;
            case DemonState.Move:
                MoveState();
                break;
        }
    }

    private void IdleState()
    {
        //タイマーを減らす
        bombSpawnTimer -= Time.deltaTime;

        //爆弾を生成するか判定
        if (bombSpawnTimer <= 0f&&currnetBombCount<MaxBombCount)
        {
            //爆弾を生成する関数呼び出し
            SpawnBomb();
            //タイマーリセット
            bombSpawnTimer=bombSpawnInterval;
            //生成数を増やす
            currnetBombCount++;
        }
        state = DemonState.Move;
    }

    private void SpawnBomb()
    {
		//デーモンの現在の位置からランダムな位置にオフセットを計算する
	    Vector3 offset = new Vector3(Random.Range(-bombSpawnRange, bombSpawnRange), 0, Random.Range(-bombSpawnRange, bombSpawnRange));
		//オフセットした位置にY軸+1する
		Vector3 spawnPosition = transform.position + offset + Vector3.up;
		//爆弾のプレハブをスポーン位置にインスタンス化する（別の変数に代入する）
		GameObject bomb = Instantiate(bombPrefab, spawnPosition, transform.rotation);
		//爆弾に自分を親として設定する（爆弾が消えたときに生成数を減らすため）
		bomb.transform.parent = transform;
	}


	private void MoveState()
    {
        state = DemonState.Idle;
    }
}
