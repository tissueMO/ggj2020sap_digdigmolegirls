using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour {
    public float Speed;
    public float RotateSpeed = 150f;
    private InputAction _rotate, _forward, _pause;
    private Vector3 _direction;

    /// <summary>
    /// 移動中のアニメーション速度
    /// </summary>
    public float AnimationSpeed = 0;

    /// <summary>
    /// プレイヤー用のスプライトを格納したゲームオブジェクト
    /// </summary>
    private Animator playerAnimator;

    /// <summary>
    /// プレイヤー専用レイヤー番号
    /// </summary>
    public const int PlayerLayer = 8;

    /// <summary>
    /// 現在移動可能かどうか
    /// </summary>
    public bool CanMove;

    public BattleUIManager battleUIManager;

    /// <summary>
    /// アニメーターのアニメーション切替待ち状態であるかどうか
    /// </summary>
    public bool IsWaitForAnimatorRefresh {
        get {
            return this.isWaitForAnimatorRefresh;
        }
        set {
            this.isWaitForAnimatorRefresh = value;
            if (value == true) {
                this.startedWaitForAnimationTime = Time.time;
            } else {
                this.startedWaitForAnimationTime = null;
            }
        }
    }
    private bool isWaitForAnimatorRefresh;

    /// <summary>
    /// アニメーターのアニメーション切替待ち状態を開始した時刻
    /// </summary>
    private float? startedWaitForAnimationTime;

    /// <summary>
    /// アニメーターのアニメーション切替待ち状態を行う時間
    /// ＊およそアニメーション1周分の時間を想定
    /// </summary>
    public const float WaitForAnimatorRefreshTime = 0.5f;

    /// <summary>
    /// プレイヤー同士が重なりあえるかどうかの設定
    /// </summary>
    [SerializeField]
    private bool canPlayerOverlap = false;

    private void Start() {
        var _input = GetComponent<PlayerInput>();
        _rotate = _input.actions["Rotate"];
        _forward = _input.actions["Forward"];
        _pause = _input.actions["Pause"];

        this.playerAnimator = this.GetComponentInChildren<Animator>();
        this.playerAnimator.speed = 0;
        if (this.AnimationSpeed == 0) {
            this.AnimationSpeed = this.Speed;
        }
    }

    private void Update() {
        if (_pause.triggered && this.battleUIManager.enabled) {
            // コントローラーの＋ボタンを押したとき
            this.battleUIManager.GetComponent<BattleUIManager>().PressedPause();
            // Debug.Log("Paaaaaaause");
            return;
        }

        if (!this.CanMove) {
            // 移動が禁止されているときは操作を受け付けない
            return;
        }

        // var rotateValue = _input.actions["Rotate"].ReadValue<float>();
        transform.Rotate(Vector3.forward * (-_rotate.ReadValue<float>() * (RotateSpeed * Time.deltaTime)));

        // var forwardValue = _input.actions["Forward"].ReadValue<float>();
        Vector3 velocity = gameObject.transform.rotation * new Vector3(0, Speed * _forward.ReadValue<float>(), 0);
        Vector3 direction;
        if (0 < _forward.ReadValue<float>()) {
            direction = Vector3.up;
            if (velocity.x < 0) {
                direction += Vector3.right;
            } else if (0 < velocity.x) {
                direction += Vector3.left;
            }
        } else if (_forward.ReadValue<float>() < 0) {
            direction = -Vector3.up;
            if (velocity.x < 0) {
                direction += Vector3.right;
            } else if (0 < velocity.x) {
                direction += Vector3.left;
            }
        } else {
            // 移動無し
            this.playerAnimator.speed = this.IsWaitForAnimatorRefresh ? 0.5f : 0;
            if (WaitForAnimatorRefreshTime <= Time.time - this.startedWaitForAnimationTime) {
                this.IsWaitForAnimatorRefresh = false;
            }
            return;
        }

        if (!this.canPlayerOverlap) {
            // 移動先に別のプレイヤーがいないかどうか判定
            var ray = new Ray2D(this.transform.position, Vector2.one * velocity);
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, 1f, 1 << PlayerMove.PlayerLayer);
            if (hits.Length != 0) {
                // 移動したい先にプレイヤーがいる場合は中断
                foreach (var hit in hits) {
                    if (hit.collider.name != this.name) {
                        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);
                        this.playerAnimator.speed = this.IsWaitForAnimatorRefresh ? 0.5f : 0;
                        if (WaitForAnimatorRefreshTime <= Time.time - this.startedWaitForAnimationTime) {
                            this.IsWaitForAnimatorRefresh = false;
                        }

                        if (hit.collider.CompareTag("Player")) {
                            hit.collider.gameObject.GetComponent<PlayerMove>().Inversion();
                        }
                        Inversion(); //プレイヤー反転
                        return;
                    }
                }
            }
            // Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 0.5f);
        }
        transform.position += velocity * Time.deltaTime;
        this.playerAnimator.speed = this.AnimationSpeed;

        /// <summary>
        /// 行動範囲制限
        /// </summary>
        gameObject.transform.position = new Vector3(
            Mathf.Clamp(gameObject.transform.position.x, -8.44f, 8.44f),
            Mathf.Clamp(gameObject.transform.position.y, -4.7f, 2.15f),
            transform.position.z
        );
    }

    /// <summary>
    /// プレイヤーの向き反転
    /// </summary>
    public void Inversion() {
        Speed = Speed * -1;
        Vector2 temp = gameObject.transform.localScale;
        temp.y *= -1;
        gameObject.transform.localScale = temp;
    }
}