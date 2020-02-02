using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingTrigger : MonoBehaviour {

    [SerializeField]
    private TitlePhaseManager titlePhaseManager;

    [SerializeField]
    private PlayerNumber playerNumber;

    /// <summary>
    /// 現在プレイヤーがこの枠の中にいるかどうか
    /// </summary>
    private bool isContainsPlayer;

    /// <summary>
    /// プレイヤーがこの枠の中に入ってから経過した秒数
    /// </summary>
    private float containsTimeSeconds;
    
    /// <summary>
    /// この枠の中に入っているプレイヤーが準備完了となったかどうか
    /// </summary>
    private bool isReadyFor;

    /// <summary>
    /// プレイヤーがこの枠に入ってから何秒経ってから準備完了とみなすかを表す秒数
    /// </summary>
    public const float ReadyForSeconds = 3.0f;

    public void Start() {
        this.isContainsPlayer = false;
        this.isReadyFor = false;
    }

    public void Update() {
        if (!this.isContainsPlayer || this.isReadyFor) {
            return;
        }

        this.containsTimeSeconds += Time.deltaTime;
        if (ReadyForSeconds <= this.containsTimeSeconds) {
            this.isReadyFor = true;
            this.titlePhaseManager.IncrementReadyForPlayerCount();
            Debug.Log($"OnTriggerEnter2D(): プレイヤー{(int) this.playerNumber + 1} 準備完了");
        }
    }

    /// <summary>
    /// プレイヤー侵入時
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == $"Player{(int)this.playerNumber + 1}") {
            this.isContainsPlayer = true;
            this.containsTimeSeconds = 0;
            Debug.Log($"OnTriggerEnter2D(): プレイヤー{(int)this.playerNumber + 1} 接触");
        }        
    }

    /// <summary>
    /// プレイヤー離脱時
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == $"Player{(int) this.playerNumber + 1}") {
            this.titlePhaseManager.DecrementReadyForPlayerCount();
            this.isContainsPlayer = false;
            this.isReadyFor = false;
            Debug.Log($"OnTriggerExit2D(): プレイヤー{(int) this.playerNumber + 1} 離脱");
        }
    }

}
