using UnityEngine;
using UnityEngine.UI;

public class PoseImageSwitcher : MonoBehaviour
{
    public Texture2D[] poseImages; // 姿勢圖片數組
    public RawImage poseRawImage; // 顯示姿勢圖片的 Raw Image UI 元素
    public RawImage stageRawImage; // 顯示舞台圖片的 Raw Image UI 元素
    public Text stageText; // 顯示當前舞台的文字 UI 元素
    public float imageSwitchInterval = 5f; // 圖片切換間隔時間
    private int currentPoseIndex = 0; // 目前姿勢圖片的索引

    void Start()
    {
        // 開始時顯示第一張姿勢圖片
        ShowPoseImage(currentPoseIndex);
        // 每隔一定時間切換圖片
        InvokeRepeating("SwitchImage", imageSwitchInterval, imageSwitchInterval);
    }

    void SwitchImage()
    {
        // 切換到下一張姿勢圖片
        currentPoseIndex = (currentPoseIndex + 1) % poseImages.Length;
        // 顯示新的姿勢圖片
        ShowPoseImage(currentPoseIndex);
    }

    void ShowPoseImage(int index)
    {
        // 更新 Raw Image 的紋理以顯示新的姿勢圖片
        poseRawImage.texture = poseImages[index];
        // 更新舞台圖片（如果需要的話）
        UpdateStageImage(index);
        // 更新當前舞台的文字
        stageText.text = "Stage: " + GetStageName(index);
    }

    void UpdateStageImage(int poseIndex)
    {
        // 根據姿勢索引更新舞台圖片
        switch (poseIndex)
        {
            case 0:
                stageRawImage.texture = poseImages[poseIndex];
                break;
            case 1:
                stageRawImage.texture = poseImages[poseIndex];
                break;
            // 添加其他舞台圖片的情況
            default:
                // 如果沒有特定的舞台圖片，可以留空或者做其他處理
                break;
        }
    }

    string GetStageName(int poseIndex)
    {
        // 根據姿勢索引獲取對應的舞台名稱
        switch (poseIndex)
        {
            case 0:
                return "Number 1";
            case 1:
                return "Number 2";
            // 添加其他舞台的名稱
            default:
                return "Unknown";
        }
    }
}
