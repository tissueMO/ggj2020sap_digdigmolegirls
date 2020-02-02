using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    [SerializeField] private int maxInventory = 1;
    public List<IItem> items;
    [SerializeField] private AudioClip getSE = default;
    [SerializeField] private AudioClip buildSE = default;

    /// <summary>
    /// プレイヤー用のスプライトを格納したゲームオブジェクト
    /// </summary>
    private GameObject playerAnimatorObject;

    /// <summary>
    /// プレイヤースプライトのインデックス紐づけ
    /// </summary>
    public enum SpritePatterns {
        Normal,
        HavingItem,
        Count
    }

    public PlayerInventory() {
        items = new List<IItem>();
    }

    /// <summary>
    /// 現在のインベントリにあるアイテムを取得する
    /// </summary>
    /// <returns></returns>
    public List<IItem> GetItemsInInventory() {
        return items;
    }

    /// <summary>
    /// アイテムの拾得
    /// </summary>
    /// <param name="item"></param>
    public void TakeItem(IItem item) {
        if (item == null) return;
        AddInventory(item);
        // item.Use();
    }

    /// <summary>
    /// インベントリにアイテムを追加
    /// </summary>
    /// <param name="item">アイテム</param>
    private void AddInventory(IItem item) {
        if (items.Count < maxInventory) {
            items.Add(item);
            item.Use();

            // アイテム所持状態のグラフィックに差し替え
            this.ChangePlayerSprite(SpritePatterns.HavingItem);

            SoundManager.Instance.PlaySE(getSE);
        } else {
            Debug.Log($"Can't add item due to occupied inventory: {item}");
        }
    }

    /// <summary>
    /// インベントリ内のアイテムを消費
    /// </summary>
    /// <returns>消費したアイテムの数</returns>
    public int ConsumeItem() {
        Debug.Log("ConsumeItem()");
        foreach(IItem item in this.items) {
            item.Consume();
        }
        var itemLength = items.Count;
        items.Clear();

        // グラフィック通常に戻す
        this.ChangePlayerSprite(SpritePatterns.Normal);

        SoundManager.Instance.PlaySE(buildSE);
        return itemLength;
    }

    /// <summary>
    /// インベントリにアイテムがあるかどうか
    /// </summary>
    /// <returns>アイテムの有無</returns>
    public bool HasInventory() {
        return items.Count > 0;
    }

    /// <summary>
    /// 初回処理
    /// </summary>
    public void Start() {
        this.playerAnimatorObject = this.GetComponentInChildren<Animator>().gameObject;

        // アニメーション通常状態でスタート
        this.ChangePlayerSprite(SpritePatterns.Normal);
    }

    ///// <summary>
    ///// デバッグ用GUI
    ///// </summary>
    //private void OnGUI() {
    //    if (GUILayout.Button("show inventory")) {
    //        foreach (var item in items) {
    //            Debug.Log($"item: {item}");
    //        }
    //    }
    //}

    /// <summary>
    /// プレイヤーのスプライトとアニメーションを切り替えます。
    /// </summary>
    private void ChangePlayerSprite(SpritePatterns pattern) {
        switch (pattern) {
            case SpritePatterns.Normal:
                this.playerAnimatorObject.GetComponent<Animator>().SetBool("isHave", false);
                break;
            case SpritePatterns.HavingItem:
                this.playerAnimatorObject.GetComponent<Animator>().SetBool("isHave", true);
                break;
        }

        // 静止時向けアニメーター更新の予約
        this.GetComponent<PlayerMove>().IsWaitForAnimatorRefresh = true;
    }

}