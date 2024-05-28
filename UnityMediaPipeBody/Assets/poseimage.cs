using UnityEngine;
using UnityEngine.UI;

public class PoseImageSwitcher : MonoBehaviour
{
    public Texture2D[] poseImages; // ���չϤ��Ʋ�
    public RawImage poseRawImage; // ��ܫ��չϤ��� Raw Image UI ����
    public RawImage stageRawImage; // ��ܻR�x�Ϥ��� Raw Image UI ����
    public Text stageText; // ��ܷ�e�R�x����r UI ����
    public float imageSwitchInterval = 5f; // �Ϥ��������j�ɶ�
    private int currentPoseIndex = 0; // �ثe���չϤ�������

    void Start()
    {
        // �}�l����ܲĤ@�i���չϤ�
        ShowPoseImage(currentPoseIndex);
        // �C�j�@�w�ɶ������Ϥ�
        InvokeRepeating("SwitchImage", imageSwitchInterval, imageSwitchInterval);
    }

    void SwitchImage()
    {
        // ������U�@�i���չϤ�
        currentPoseIndex = (currentPoseIndex + 1) % poseImages.Length;
        // ��ܷs�����չϤ�
        ShowPoseImage(currentPoseIndex);
    }

    void ShowPoseImage(int index)
    {
        // ��s Raw Image �����z�H��ܷs�����չϤ�
        poseRawImage.texture = poseImages[index];
        // ��s�R�x�Ϥ��]�p�G�ݭn���ܡ^
        UpdateStageImage(index);
        // ��s��e�R�x����r
        stageText.text = "Stage: " + GetStageName(index);
    }

    void UpdateStageImage(int poseIndex)
    {
        // �ھګ��կ��ާ�s�R�x�Ϥ�
        switch (poseIndex)
        {
            case 0:
                stageRawImage.texture = poseImages[poseIndex];
                break;
            case 1:
                stageRawImage.texture = poseImages[poseIndex];
                break;
            // �K�[��L�R�x�Ϥ������p
            default:
                // �p�G�S���S�w���R�x�Ϥ��A�i�H�d�ũΪ̰���L�B�z
                break;
        }
    }

    string GetStageName(int poseIndex)
    {
        // �ھګ��կ�������������R�x�W��
        switch (poseIndex)
        {
            case 0:
                return "Number 1";
            case 1:
                return "Number 2";
            // �K�[��L�R�x���W��
            default:
                return "Unknown";
        }
    }
}
