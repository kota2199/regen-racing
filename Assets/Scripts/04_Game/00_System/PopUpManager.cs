using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks; // Taskを使用するために必要
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    private PopUpMessageData messageData;

    [SerializeField]
    private Text t_main, t_sub;

    [SerializeField]
    private RectTransform PopUpRect;

    [SerializeField]
    private Vector2 basePos, targetPos;

    [SerializeField]
    private float interval, time;

    // 処理中かどうかを判定するフラグ
    private bool isProcessing = false;

    private bool batteryPopped = false, positionPopped = false;

    [SerializeField]
    private BatterySystem playerBattery;

    [SerializeField]
    private AudioSource audioSource;

    private CountDown countDown;

    [SerializeField]
    private RaceData raceData;

    private void Awake()
    {
        countDown = GetComponent<CountDown>();
    }
    void Start()
    {
        PopUpRect.anchoredPosition = basePos;
    }

    private void Update()
    {
        CallPopUp();
    }

    private void CallPopUp()
    {
        if (raceData.isPlay)
        {
            if (playerBattery.remainBattery <= 30 && !batteryPopped)
            {
                PopUp(1);
                batteryPopped = true;
            }
            else if (playerBattery.remainBattery >= 100 && !batteryPopped && countDown.isPlay)
            {
                PopUp(2);
                batteryPopped = true;
            }
            else if (70 > playerBattery.remainBattery && playerBattery.remainBattery > 30 && batteryPopped)
            {
                batteryPopped = false;
            }
        }
    }

    // 非同期メソッドに変更
    public async void PopUp(int index)
    {
        // 処理中なら待機
        while (isProcessing)
        {
            await Task.Yield();
        }

        // 処理開始
        isProcessing = true;

        // メッセージを設定
        PopUpRect.anchoredPosition = basePos;
        t_main.text = messageData.Datas[index].mainMassage;
        t_sub.text = messageData.Datas[index].subMassage;
        audioSource.PlayOneShot(messageData.Datas[index].radio);

        // DoTweenシーケンスの完了を待機
        await ExecutePopUpAnimation();

        // 処理終了
        isProcessing = false;
    }

    // アニメーション処理をタスクとして分離
    private Task ExecutePopUpAnimation()
    {
        // 完了を監視するためのTaskCompletionSourceを使用
        var tcs = new TaskCompletionSource<bool>();

        DOTween.Sequence()
            .Append(PopUpRect.DOAnchorPos(targetPos, time).SetEase(Ease.OutExpo))
            .AppendInterval(interval)
            .Append(PopUpRect.DOAnchorPos(basePos, time).SetEase(Ease.OutExpo))
            .OnComplete(() =>
            {
                // アニメーション完了時にTaskを完了
                tcs.SetResult(true);
            });

        return tcs.Task;
    }
}
