using System;
using UnityEngine;

public enum PlayerNumber {
    Player1,
    Player2
}

public enum DisturbReturnType {
    None,
    PlayerMaskMinimum,
    PlayerMaskNone,
    DisturbControl,
}

public class PlayerControl : MonoBehaviour {
    private PlayerInventory _inventory;

    public PlayerNumber playerNumber;
    
    [SerializeField] private AudioClip breakSE = default;

    /// <summary>
    /// プレイヤーが1回転して攻撃するための時間
    /// </summary>
    private float toAttackTime = 2.5f;
    /// <summary>
    /// 敵陣地の妨害を行った際に発生するプレイヤー本人へのリターンのモード
    /// </summary>
    public DisturbReturnType DisturbReturnMode;

    /// <summary>
    /// 敵陣地の妨害に成功した回数
    /// </summary>
    private int disturbCount;

    /// <summary>
    /// 敵陣地への妨害一回あたりのダメージ量
    /// </summary>
    public const float OneDisturbDamage = 50f;

    /// <summary>
    /// 現在居る家(居なければnull)
    /// </summary>
    private MyHome inHome;

    /// <summary>
    /// プレイヤーオブジェクトのposition(前フレーム記憶用)
    /// </summary>
    private Vector3 positionInBeforeFrame;

    /// <summary>
    /// プレイヤーオブジェクトのrotation(前フレーム記憶用)
    /// </summary>
    private Vector3 rotationInBeforeFrame;

    /// <summary>
    /// プレイヤーが敵陣地で回っているか
    /// </summary>
    private bool isAttackingEnemyHome;

    /// <summary>
    /// プレイヤーが回って攻撃している時間
    /// </summary>
    private float isAttackingTime = 0f;

    /// <summary>
    /// プレイヤーが破壊行為をしたかどうか
    /// </summary>
    private bool attackedEnemyHome = false;

    private void Start()
    {
        _inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (this.inHome != null && !this.attackedEnemyHome && this.inHome.ownerPlayer != this.playerNumber)
        {
            if (this.isAttackingEnemyHome)
            {
                //攻撃が続いているかどうか
                if (positionInBeforeFrame.x == this.transform.position.x && positionInBeforeFrame.y == this.transform.position.y && rotationInBeforeFrame.z != this.transform.rotation.z)
                {
                    this.isAttackingTime += Time.deltaTime;
                    if(this.isAttackingTime > this.toAttackTime)
                    {
                        //破壊処理
                        Debug.Log("破壊処理に成功");
                        this.inHome.MakeProgress(-1);
                        switch (this.DisturbReturnMode)
                        {
                            case DisturbReturnType.PlayerMaskMinimum:
                                this.GetComponentInChildren<SpriteMask>().transform.localScale += new Vector3(-0.28f, -0.28f, -0.28f);
                                break;
                            case DisturbReturnType.PlayerMaskNone:
                                this.GetComponentInChildren<SpriteMask>().enabled = false;
                                break;
                            case DisturbReturnType.DisturbControl:
                                // TODO: 上に進める処理を乱したり、左右への操作をランダムに切り替える
                                break;
                        }
                        this.attackedEnemyHome = true;
                        SoundManager.Instance.PlaySE(breakSE);
                    }
                }
                else
                {
                    this.isAttackingEnemyHome = false;
                    this.isAttackingTime = 0f;
                }
            }
            else 
            {
                //攻撃を始めたかの判定
                if(positionInBeforeFrame.x == this.transform.position.x && positionInBeforeFrame.y == this.transform.position.y && rotationInBeforeFrame.z != this.transform.rotation.z)
                {
                    this.isAttackingEnemyHome = true;
                    this.isAttackingTime = 0f;
                }
            }

            positionInBeforeFrame = this.transform.position;
            rotationInBeforeFrame = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        this.inHome = other.GetComponent<MyHome>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log($"OnTriggerEnter: {other}");

        // アイテムを触ったとき
        _inventory?.TakeItem(other.GetComponent<IItem>());

        // Homeを触ったとき
        var home = other.GetComponent<MyHome>();
        this.inHome = home;
        if (home != null) {
            if (home.ownerPlayer == playerNumber && (_inventory?.HasInventory() ?? false)) {
                var consumedItemCnt = _inventory.ConsumeItem();
                home.MakeProgress(consumedItemCnt);
            } else if (home.ownerPlayer != this.playerNumber) {
                this.positionInBeforeFrame = this.transform.position;
                this.rotationInBeforeFrame = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
                Debug.Log($"OnTriggerEnter: {this.playerNumber}が 敵陣地 {home.ownerPlayer}に 接触");

                // 敵陣地に対する妨害処理
                /*if (home.Damage(OneDisturbDamage)) {
                    // 敵陣地の進捗を1段階落とすことに成功
                    Debug.Log($"OnTriggerEnter: {this.playerNumber}が 敵陣地 {home.ownerPlayer}の 1段階降格に成功");

                    switch(this.DisturbReturnMode) {
                        case DisturbReturnType.PlayerMaskMinimum:
                            this.GetComponentInChildren<SpriteMask>().transform.localScale += new Vector3(-0.28f, -0.28f, -0.28f);
                            break;
                        case DisturbReturnType.PlayerMaskNone:
                            this.GetComponentInChildren<SpriteMask>().enabled = false;
                            break;
                        case DisturbReturnType.DisturbControl:
                            // TODO: 上に進める処理を乱したり、左右への操作をランダムに切り替える
                            break;
                    }
                }*/
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this.inHome = other.GetComponent<MyHome>();
        /*if(home != null)
        {
            if(home.ownerPlayer != this.playerNumber)
            {
                this.InEnemyHome = false;
            }
        }*/
    }
}
