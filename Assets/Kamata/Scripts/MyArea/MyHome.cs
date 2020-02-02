using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum HomeProgress {
    NoProgress,
    First,
    Second,
    Completed
}

public class MyHome : MonoBehaviour {
    public PlayerNumber ownerPlayer;
    private HomeProgress _progress;

    /// <summary>
    /// MyHome用のスプライト
    /// ＊Indexは HomeProgress に従う
    /// </summary>
    public Sprite[] sprites;

    [SerializeField] private BattleUIManager battleUIManager = null;

    [SerializeField]
    private GameObject explosion;

    /// <summary>
    /// シーン開始時点のデフォルト用スプライト
    /// </summary>
    private Sprite defaultSprite;

    /// <summary>
    /// 現在のHP、妨害を受けると減少し、0になると1段階前に戻す
    /// </summary>
    private float hp;

    /// <summary>
    /// 妨害に耐えられるHP
    /// </summary>
    public const float HpMax = 200f;

    public MyHome() {
        _progress = HomeProgress.NoProgress;
    }

    public void Start() {
        // 初期状態の表示からスタート
        this.defaultSprite = this.GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// このおうちにダメージを与える
    /// </summary>
    /// <param name="point">ダメージ量</param>
    /// <returns>1段階破壊成功したかどうか</returns>
    public bool Damage(float point) {
        this.hp -= point;
        Debug.Log($"Damage(): {this.ownerPlayer} HP={this.hp}");
        if (this.hp <= 0) {
            Debug.Log($"Damage(): 崩落、1段階戻る");
            this.hp = HpMax;
            this.MakeProgress(-1);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 進捗ゼロに戻す
    /// </summary>
    public void ResetProgress() {
        this.hp = HpMax;
        this._progress = HomeProgress.NoProgress;
        this.GetComponent<SpriteRenderer>().sprite = this.sprites[(int) HomeProgress.NoProgress];
    }

    /// <summary>
    /// おうちを爆発
    /// </summary>
    public void Explosion(Action callback = null) {
        // TODO: 爆発SE
        this.explosion.SetActive(true);
        this.explosion.GetComponent<SpriteRenderer>().enabled = true;

        iTween.ScaleFrom(this.explosion, iTween.Hash(
            "x", 0,
            "y", 0,
            "z", 0,
            "time", 0.5f,
            "EaseType", iTween.EaseType.easeOutQuad,
            "oncomplete", new Action<object>((obj) => {
                this.OnExplosionCompletedPhase1(callback);
            })
        ));
    }

    private void OnExplosionCompletedPhase1(Action callback) {
        this.ResetProgress();
        Debug.Log("爆炎戻す");

        iTween.ScaleTo(this.explosion, iTween.Hash(
            "x", 0,
            "y", 0,
            "z", 0,
            "time", 0.5f,
            "EaseType", iTween.EaseType.easeInQuad,
            "oncomplete", new Action<object>((obj) => {
                this.OnExplosionCompletedPhase2(callback);
            })
        ));
    }

    private void OnExplosionCompletedPhase2(Action callback) {
        // Debug.Log("爆炎Closing...");
        this.explosion.GetComponent<SpriteRenderer>().enabled = false;

        iTween.ScaleTo(this.explosion, iTween.Hash(
            "x", 0.4f,
            "y", 0.4f,
            "z", 0.4f,
            "time", 1f,
            "EaseType", iTween.EaseType.linear,
            "oncomplete", new Action<object>((obj) => {
                // Debug.Log("爆炎Closed.");
                this.explosion.SetActive(false);
                callback?.Invoke();
            })
        ));
    }

    /// <summary>
    /// おうちを修復
    /// </summary>
    public void Repair() {
        this._progress = HomeProgress.Completed;
        this.GetComponent<SpriteRenderer>().sprite = this.defaultSprite;
    }

    /// <summary>
    /// 進捗
    /// </summary>
    /// <param name="cnt">すすめる進捗の数</param>
    public void MakeProgress(int cnt) {
        if (_progress == HomeProgress.Completed) {
            return;
        }

        _progress = (HomeProgress) Mathf.Clamp((int) _progress + cnt, (int) HomeProgress.NoProgress, (int) HomeProgress.Completed);
        this.hp = HpMax;

        // グラフィック更新
        this.GetComponent<SpriteRenderer>().sprite = this.sprites[(int) this._progress];

        OnProgressChanged();
        if (_progress == HomeProgress.Completed) {
            OnCompleted();
        }
    }

    private void OnProgressChanged() {
        Debug.Log($"OnProgressChanged(): current progress is {_progress}");
    }

    private void OnCompleted() {
        Debug.Log("OnCompleted(): _progress = Completed");
        this.battleUIManager.FinishBattle(this.ownerPlayer);
    }
}
