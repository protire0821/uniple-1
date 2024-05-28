using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ContinuousVideoPlayer : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip[] clips;
    public Text stageText; // UI Text 元素
    public Text angleText; // UI Text 元素
    private int currentClipIndex = 0;
    private bool isPlaying = true;

    void Start()
    {
        // 檢查 VideoPlayer 和 clips 是否已分配
        if (vp != null && clips != null && clips.Length > 0)
        {
            // 設置第一個影片片段並播放
            vp.clip = clips[currentClipIndex];
            vp.Play();
            // 更新 UI 文字
            UpdateStageText();
            // 訂閱影片播放完成事件
            vp.loopPointReached += OnVideoLoopPointReached;
        }
        else
        {
            Debug.LogError("VideoPlayer 或 VideoClip 陣列未分配！");
        }
    }

    // 影片循環播放完成時的回調函數
    void OnVideoLoopPointReached(VideoPlayer player)
    {
        // 增加當前影片索引
        currentClipIndex++;
        // 如果當前影片索引超過陣列範圍，重置為 0
        if (currentClipIndex >= clips.Length)
        {
            currentClipIndex = 0;
        }
        // 設置下一個影片片段並播放
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // 更新 UI 文字
        UpdateStageText();
    }

    // 在腳本被禁用或銷毀時取消訂閱事件
    void OnDisable()
    {
        if (vp != null)
        {
            vp.loopPointReached -= OnVideoLoopPointReached;
        }
    }

    void Update()
    {

        // 直接從 PipeServer 類中獲取 nodeAngles 陣列
        float[] nodeAngles = PipeServer.nodeAngles;
        if (nodeAngles != null && nodeAngles.Length > 12)
        {
            // 取得角度並轉換為整數
            int angle = Mathf.RoundToInt(nodeAngles[11]);
            int angle12 = Mathf.RoundToInt(nodeAngles[12]);
            // 將角度顯示在遊戲畫面中
            Debug.Log(angle);
        }
       
        // 檢測鍵盤輸入並執行相應的操作
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 如果正在播放，則暫停；如果已暫停，則恢復播放
            if (isPlaying)
            {
                vp.Pause();
            }
            else
            {
                vp.Play();
            }
            // 切換播放狀態
            isPlaying = !isPlaying;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 如果正在播放，則暫停
            if (isPlaying)
            {
                vp.Pause();
                isPlaying = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 往下一部影片
            NextVideo();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 往上一部影片
            PreviousVideo();
        }
    }

    void NextVideo()
    {
        // 增加當前影片索引
        currentClipIndex++;
        // 如果當前影片索引超過陣列範圍，重置為 0
        if (currentClipIndex >= clips.Length)
        {
            currentClipIndex = 0;
        }
        // 設置下一個影片片段並播放
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // 更新 UI 文字
        UpdateStageText();
    }

    void PreviousVideo()
    {
        // 減少當前影片索引
        currentClipIndex--;
        // 如果當前影片索引小於 0，重置為陣列的最後一個索引
        if (currentClipIndex < 0)
        {
            currentClipIndex = clips.Length - 1;
        }
        // 設置上一個影片片段並播放
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // 更新 UI 文字
        UpdateStageText();
    }

    // 更新 UI 文字
    void UpdateStageText()
    {
        if (stageText != null)
        {
            stageText.text = "Stage:" + (currentClipIndex + 1);
        }
    }
}


