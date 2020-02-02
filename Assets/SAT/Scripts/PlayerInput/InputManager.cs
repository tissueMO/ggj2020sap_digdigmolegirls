using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    /// <summary>
    /// プレイヤー数
    /// </summary>
    public const int PlayerCount = 2;

    /// <summary>
    /// [デバッグ用] キーボードで操作するモードに強制変更するかどうか
    /// </summary>
    [SerializeField]
    private bool isKeyboardMode;

    /// <summary>
    /// 生成するプレイヤーのプレハブ
    /// ＊インスペクターにて設定
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private BattleUIManager battleUIManager;

    /// <summary>
    /// プレイヤーごとの初期Rotation
    /// </summary>
    private float[] PlayerRotationPatterns = new float[] {
        -90f, 90f,
    };

    /// <summary>
    /// プレイヤーごとのスピード
    /// </summary>
    private float[] PlayerSpeeds = new float[] {
        7.5f, 7.5f,
    };

    /// <summary>
    /// プレイヤーごとの回転スピード
    /// </summary>
    private float[] PlayerRotationSpeeds = new float[] {
        150f, 150f,
    };

    /// <summary>
    /// キーボード制御用InputAction
    /// </summary>
    [SerializeField]
    private InputActionAsset[] PlayerKeyboardInputActions;

    /// <summary>
    /// プレイヤー初期スプライト
    /// </summary>
    [SerializeField]
    private Sprite[] PlayerDefaultSprites;

    /// <summary>
    /// プレイヤーアニメーター
    /// </summary>
    [SerializeField]
    private RuntimeAnimatorController[] PlayerAnimators;

    [SerializeField] private CinemachineTargetGroup targetGroup;
    
    /// <summary>
    /// ゲーム開始の準備が整っているかどうか
    /// </summary>
    public bool IsReadyFor {
        get => this.GetComponent<PlayerInputManager>().playerCount == InputManager.PlayerCount;
    }

    /// <summary>
    /// 初期処理
    /// </summary>
    public void Start() {
        if (Gamepad.all.Count < PlayerCount) {
            Debug.LogWarning($"Start(): 必要なゲームパッドの台数分接続されていません。キーボード専用モードで始めます。");
            this.isKeyboardMode = true;
        }

        // ゲームプレイに必要なプレイヤーを生成
        for (int i = 0; i < PlayerCount; i++) {
            GameObject newPlayer;

            // NOTE: 新しいオブジェクトの生成方法が異なる
            if (this.isKeyboardMode) {
                newPlayer = GameObject.Instantiate(this.playerPrefab);
            } else {
                newPlayer = PlayerInput.Instantiate(
                    this.playerPrefab, i, null, -1, Gamepad.all[i]
                ).gameObject;
            }
            newPlayer.name = $"Player{i + 1}";

            if (this.isKeyboardMode) {
                // キーボードで操作する場合に限り InputAction を直接変更する
                newPlayer.GetComponent<PlayerInput>().actions = this.PlayerKeyboardInputActions[i];
            }

            // 生成直後のフレームで初期化処理を適用
            this.StartCoroutine(this.NewPlayerInitializeCoroutine(newPlayer, i));
        }
    }

    /// <summary>
    /// プレイヤー初期化
    /// </summary>
    private IEnumerator NewPlayerInitializeCoroutine(GameObject newPlayer, int index) {
        // 移動禁止
        newPlayer.GetComponent<PlayerMove>().CanMove = false;

        // 初期位置をセット
        newPlayer.transform.position = GameObject.Find($"Home{index + 1}").transform.position;
        newPlayer.transform.position += new Vector3(0, -1.4f, 0);
        // 初期Rotationをセット
        newPlayer.transform.rotation = Quaternion.Euler(0, 0, this.PlayerRotationPatterns[index]);
        // 移動速度をセット
        newPlayer.GetComponent<PlayerMove>().Speed = this.PlayerSpeeds[index];
        // 回線速度をセット
        newPlayer.GetComponent<PlayerMove>().RotateSpeed = this.PlayerRotationSpeeds[index];
        // プレイヤー番号を更新
        newPlayer.GetComponent<PlayerControl>().playerNumber = (PlayerNumber) Enum.ToObject(typeof(PlayerNumber), index);

        // 初期グラをセット
        newPlayer.GetComponentInChildren<SpriteRenderer>().sprite = this.PlayerDefaultSprites[index];
        // 現在のプレイヤーグラフィックに対応したランタイムアニメーションコントローラーをセット
        newPlayer.GetComponentInChildren<Animator>().runtimeAnimatorController = this.PlayerAnimators[index];

        // バトルUI管理オブジェクトをセット
        newPlayer.GetComponent<PlayerMove>().battleUIManager = this.battleUIManager;

        if (index == 1) {
            // 2P専用の特殊処理
            newPlayer.GetComponentInChildren<SpriteRenderer>(false).flipY = true;
        }

        if (targetGroup != null)
        {
            var target = new CinemachineTargetGroup.Target()
            {
                target = newPlayer.transform,
                radius = 2f,
                weight = 1f
            };
            targetGroup.m_Targets[index] = target;
        }

        yield return new WaitForEndOfFrame();
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        Debug.Log("OnPlayerJoined(): 新しいプレイヤーが参加");
        foreach (var device in playerInput.devices) {
            Debug.Log($"OnPlayerJoined(): 操作デバイス {device}");
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput) {
        foreach (var device in playerInput.devices) {
            Debug.LogWarning($"OnPlayerLeft(): ゲーム中にプレイヤーコントローラーが外されました: {device}");
        }
    }

}
