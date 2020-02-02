using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePhaseManager : MonoBehaviour
{
    [SerializeField] private AudioClip acceptSE = default;

    /// <summary>
    /// 次のフェーズの引き継ぎ先となる管理オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject battleUIManager;

    /// <summary>
    /// タイトルヘッダーを描画するキャンバス
    /// </summary>
    [SerializeField]
    private GameObject titleUICanvas;

    //　カウントダウンのキャンバス
    [SerializeField]
    private GameObject countDownCanvas;

    /// <summary>
    /// 開始位置オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] startingTriggerAreas;

    /// <summary>
    /// 初期表示を隠すためだけの暗幕オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject blackBackground;

    /// <summary>
    /// トランジションを行う管理オブジェクト
    /// </summary>
    [SerializeField]
    private Fade fader;

    /// <summary>
    /// 開始位置に居る準備完了プレイヤーの数
    /// </summary>
    private int readyForPlayerCount;

    public void Start() {
        this.readyForPlayerCount = 0;

        // フェーズ遷移時の共通処理呼び出し
        this.StartTitle(1.0f, new Action(() => {
            this.blackBackground.SetActive(false);
        }));
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            this.StartBattle();
        }
    }

    public void IncrementReadyForPlayerCount() {
        this.readyForPlayerCount++;
        if (this.readyForPlayerCount == 2) {
            // 双方のプレイヤーの開始準備が整ったとみなす
            this.StartBattle();
        }
    }

    public void DecrementReadyForPlayerCount() {
        this.readyForPlayerCount--;
        if (this.readyForPlayerCount < 0) {
            this.readyForPlayerCount = 0;
        }
    }

    /// <summary>
    /// タイトルフェーズに遷移（するためのトランジション暗転・明転を行う）
    /// </summary>
    public void StartTitle(float time, Action beforeFadeInAction = null, Action afterFadeInAction = null) {
        this.fader.FadeIn(time, new Action(() => {
            beforeFadeInAction?.Invoke();

            // カウントダウン非表示
            this.countDownCanvas.SetActive(false);

            // タイトルロゴ表示
            this.titleUICanvas.SetActive(true);

            // 開始位置オブジェクト表示
            foreach (var obj in this.startingTriggerAreas) {
                obj.SetActive(true);
                obj.GetComponent<StartingTrigger>().Start();
            }

            this.fader.FadeOut(time, new Action(() => {
                afterFadeInAction?.Invoke();
            }));

            // プレイヤー移動解禁
            this.StartCoroutine(this.allowPlayerMove());
        }));
    }

    private IEnumerator allowPlayerMove() {
        yield return new WaitForSeconds(1.0f);

        // プレイヤー移動開放
        GameObject.Find("Player1").GetComponent<PlayerMove>().CanMove = true;
        GameObject.Find("Player2").GetComponent<PlayerMove>().CanMove = true;
    }

    /// <summary>
    /// バトルフェーズに遷移
    /// </summary>
    public void StartBattle() {
        SoundManager.Instance.PlaySE(acceptSE);
        // 開始位置オブジェクト非表示
        foreach (var obj in this.startingTriggerAreas) {
            obj.SetActive(false);
        }

        // 双方の基地を爆破する演出
        GameObject.Find("Home1").GetComponent<MyHome>().Explosion();
        GameObject.Find("Home2").GetComponent<MyHome>().Explosion(new Action(() => {
            // トランジション
            this.fader.FadeIn(0.5f, new Action(() => {
                // プレイヤー再生成
                foreach (var obj in GameObject.FindObjectsOfType<GameObject>()) {
                    if (obj.name == "Player1" || obj.name == "Player2") {
                        GameObject.Destroy(obj);
                    }
                }
                GameObject.Find("PlayerInputManager").GetComponent<InputManager>().Start();

                // 木材生成開始
                GameObject.Find("LogGenerater").GetComponent<LogsGenerater>().enabled = true;

                // タイトルロゴ非表示
                this.titleUICanvas.SetActive(false);

                // 移動禁止
                this.fader.FadeOut(0.5f, new Action(() => {
                    //カウントダウン表示
                    this.battleUIManager.SetActive(true);
                    this.countDownCanvas.SetActive(true);
                    this.battleUIManager.GetComponent<BattleUIManager>().BeginBattle();
                    this.gameObject.SetActive(false);
                }));
            }));
        }));
    }

}
