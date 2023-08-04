using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerAttack : MonoBehaviour
{
    //攻撃の範囲
    [SerializeField]
    public float attackRange = 2f;
    //攻撃のダメージ(攻撃力)
    public float atk = 10f;
    //攻撃用のレイヤーマスク
    public LayerMask enemyLayer;
    //アニメーター
    Animator animator;
    //攻撃アクション
    private InputAction attackAction;

    //コンボアクション用
    //攻撃用アクションフラグ
    private bool IsAttack;
    //コンボ判定用フラグ
    private int combo_Flag;
    //コンボ回数
    private int combo_Count;
    //タイムライン
    [SerializeField]
    private PlayableDirector[] tl;
    //アクションフラグ
    private bool avoid;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //攻撃アクションを作成
        attackAction = GetComponent<PlayerInput>().actions["Attack"];
    }

	private void Update()
	{
       
    }

    //攻撃ボタン用
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //攻撃中かつ回避中じゃなかったら
            if (!IsAttack && !avoid)
            {
                //アクションフラグをONにする
                IsAttack = true;
                switch (combo_Count)
                {
                    case 0:
                        //コンボ回数が0の時タイムラインを再生する
                        tl[0].Play();
                        break;
                }
            }
        }
    }

    //コンボ有効化
    public void ComboEnable()
    {
        //コンボ入力が無効(0)の時、
        if (combo_Flag == 0)
        {
            //コンボ入力を有効(未入力)にする
            combo_Flag = 1;
            Debug.Log("Enable");
        }
    }

    //コンボ入力用
    public void OnCombo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //攻撃中かつコンボ入力が(1)の時
            if (IsAttack && combo_Flag == 1)
            {
                //コンボを入力済み(2)にする
                combo_Flag = 2;
            }
        }
    }

    //コンボ判定用
    public void ComboCheak()
    {
        if (combo_Flag == 2)
        {
            switch (combo_Count)
            {
                //コンボ回数が0の時の処理(1回目のコンボ攻撃)
                case 0:
                    //再生中のタイムラインの停止
                    tl[0].Stop();
                    //次のタイムラインを再生
                    tl[1].Play();
                    //コンボ回数の更新
                    combo_Count = 1;
                    break;
                //コンボ回数が1の時の処理(2回目のコンボ攻撃)
                case 1:
                    tl[1].Stop();
                    tl[2].Play();
                    combo_Count = 2;
                    break;
                //コンボ回数が2の時の処理(3回目のコンボ攻撃)
                case 2:
                    tl[2].Stop();
                    tl[3].Play();
                    combo_Count = 3;
                    break;
            }
            //コンボ入力を無効(0)にする
            combo_Flag = 0;
        }
    }

    //攻撃終了用
    public void AttackEnd()
    {
        if (combo_Flag == 1 || combo_Flag == 2)
        {
            //コンボ入力を無効(0)にする
            combo_Flag = 0;
        }
        //攻撃フラグをOFFにする
        IsAttack = false;
        //コンボ回数を0にする
        combo_Count = 0;
    }


    private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
