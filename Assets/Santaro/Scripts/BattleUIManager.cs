using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private GameObject winTextCanvas = null;
    [SerializeField] private GameObject countDownCanvas = null;

    [SerializeField] private AudioClip bgm_0 = default;
    [SerializeField] private AudioClip bgm_1 = default;
    [SerializeField] private AudioClip acceptSE = default;

    private int defaultCountDownFontSize;
    private Text countDownText;
    private Image countDownImage;
    public Sprite countDown1;
    public Sprite countDown2;
    public Sprite countDown3;
    public Sprite Go;
    public bool isPausing = false;
    private int defaultCountDownImageScale = 100;
    /// <summary>
    /// タイトルフェーズ管理オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject titlePhaseManaer;

    [SerializeField] private List<AudioClip> countSE = default;

    private void Start() {
        countDownImage.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) {
            PressedPause();
        }
    }

    public void BeginBattle() {
        this.enabled = true;
        this.countDownText = this.countDownCanvas.transform.Find("CountDownText").GetComponent<Text>();
        this.countDownImage = this.countDownCanvas.transform.Find("CountDownImage").GetComponent<Image>();
        this.defaultCountDownFontSize = this.countDownText.fontSize;
        Time.timeScale = 0;

        SoundManager.Instance.StopBGM();

        RectTransform countDownImageTransform = this.countDownCanvas.transform.Find("CountDownImage").GetComponent<RectTransform>();
        int upScale = 0;

        bool playedCountDownSE = false;

        for (float i = 0f; i < 5f; i += 0.1f) {
            StartCoroutine(DelayMethodRealTime(i, () => {
                upScale += 20;
                countDownImageTransform.sizeDelta = new Vector2(defaultCountDownImageScale + upScale, defaultCountDownImageScale + upScale);
            }));
        }
        StartCoroutine(DelayMethodRealTime(1f, () => {
            // カウントダウンボイス
            if (!playedCountDownSE) {
                SoundManager.Instance.PlaySE(countSE[UnityEngine.Random.Range(0, countSE.Count - 1)]);
                playedCountDownSE = true;
            }

            countDownImageTransform.sizeDelta = new Vector2(defaultCountDownImageScale, defaultCountDownImageScale);
            upScale = 0;
            countDownImage.enabled = true;
        }));
        StartCoroutine(DelayMethodRealTime(2f, () => {
            countDownImageTransform.sizeDelta = new Vector2(defaultCountDownImageScale, defaultCountDownImageScale);
            upScale = 0;
            countDownImage.sprite = countDown2;
        }));
        StartCoroutine(DelayMethodRealTime(3f, () => {
            countDownImageTransform.sizeDelta = new Vector2(defaultCountDownImageScale, defaultCountDownImageScale);
            upScale = 0;
            countDownImage.sprite = countDown1;
            SoundManager.Instance.PlayBGM(bgm_0);
        }));
        StartCoroutine(DelayMethodRealTime(4f, () => {

            countDownImageTransform.sizeDelta = new Vector2(defaultCountDownImageScale + 200, defaultCountDownImageScale + 200);
            upScale = 200;
            countDownImage.sprite = Go;
            Time.timeScale = 1;
        }));
        StartCoroutine(DelayMethodRealTime(5f, () => {
            countDownImage.enabled = false;
            countDownImage.sprite = countDown3;
            countDownText.text = "";

            // プレイヤー移動開放
            GameObject.Find("Player1").GetComponent<PlayerMove>().CanMove = true;
            GameObject.Find("Player2").GetComponent<PlayerMove>().CanMove = true;
        }));
    }

    public void FinishBattle(PlayerNumber playerNum) {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySE(bgm_1);
        Time.timeScale = 0f;
        if (playerNum == PlayerNumber.Player1) this.winTextCanvas.transform.Find("P1WinText").gameObject.SetActive(true);
        else this.winTextCanvas.transform.Find("P2WinText").gameObject.SetActive(true);
        this.winTextCanvas.transform.Find("SmogPanel").gameObject.SetActive(true);
        StartCoroutine(DelayMethodRealTime(2f, () => {
            GameObject goTitleButton = this.winTextCanvas.transform.Find("GoTitleButton").gameObject;
            goTitleButton.SetActive(true);
            goTitleButton.GetComponent<Button>().Select();
        }));
    }

    public void PressedPause() {
        if (!this.isPausing && Time.timeScale > 0) {
            Time.timeScale = 0;
            SoundManager.Instance.PlaySE(acceptSE);
            SoundManager.Instance.PauseBGM();
            this.winTextCanvas.transform.Find("PausePanel").gameObject.SetActive(true);
            GameObject goTitleButton = this.winTextCanvas.transform.Find("GoTitleButton").gameObject;
            goTitleButton.SetActive(true);
            goTitleButton.GetComponent<Button>().Select();
            this.isPausing = true;
        } else if (this.isPausing && Time.timeScale == 0) {
            Time.timeScale = 1;
            SoundManager.Instance.PlaySE(acceptSE);
            SoundManager.Instance.ResumeBGM();
            this.winTextCanvas.transform.Find("PausePanel").gameObject.SetActive(false);
            GameObject goTitleButton = this.winTextCanvas.transform.Find("GoTitleButton").gameObject;
            goTitleButton.SetActive(false);
            this.isPausing = false;
        }
    }
    public void PressedGoTitleButton() {
        Debug.Log("goTitleButtonが押されたよ");
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySE(acceptSE);
        this.isPausing = false;
        Time.timeScale = 1;
        this.DisposeBattlePhase();
    }

    /// <summary>
    /// バトルフェーズで発生したオブジェクト群の後始末
    /// </summary>
    private void DisposeBattlePhase() {
        // 木材生成停止
        GameObject.Find("LogGenerater").GetComponent<LogsGenerater>().enabled = false;

        // バトルフェーズ終了
        this.enabled = false;
        Time.timeScale = 1f;

        this.titlePhaseManaer.GetComponent<TitlePhaseManager>().StartTitle(
            1.0f,
            new Action(() => {
            // プレイヤー再生成
            foreach (var obj in GameObject.FindObjectsOfType<GameObject>()) {
                    if (obj.name == "Player1" || obj.name == "Player2" || obj.name == "Log(Clone)") {
                        GameObject.Destroy(obj);
                    }
                }
                GameObject.Find("PlayerInputManager").GetComponent<InputManager>().Start();

            // おうちの修理
            GameObject.Find("Home1").GetComponent<MyHome>().Repair();
                GameObject.Find("Home2").GetComponent<MyHome>().Repair();

            // キャンバス系非表示
            foreach (Transform obj in GameObject.Find("WinTextCanvas").transform) {
                    obj.gameObject.SetActive(false);
                }

            // タイトルロゴ復活してPlayableなタイトル画面へ
            this.titlePhaseManaer.SetActive(true);
            }),
            new Action(() => {
            // プレイヤー移動開放
            GameObject.Find("Player1").GetComponent<PlayerMove>().CanMove = true;
                GameObject.Find("Player2").GetComponent<PlayerMove>().CanMove = true;
            })
        );
    }

    IEnumerator DelayMethodRealTime(float waitTime, Action action) {
        yield return new WaitForSecondsRealtime(waitTime);
        action();
    }
}
