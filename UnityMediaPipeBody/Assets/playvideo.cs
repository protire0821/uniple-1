using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ContinuousVideoPlayer : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip[] clips;
    public Text stageText; // UI Text ����
    public Text angleText; // UI Text ����
    private int currentClipIndex = 0;
    private bool isPlaying = true;

    void Start()
    {
        // �ˬd VideoPlayer �M clips �O�_�w���t
        if (vp != null && clips != null && clips.Length > 0)
        {
            // �]�m�Ĥ@�Ӽv�����q�ü���
            vp.clip = clips[currentClipIndex];
            vp.Play();
            // ��s UI ��r
            UpdateStageText();
            // �q�\�v�����񧹦��ƥ�
            vp.loopPointReached += OnVideoLoopPointReached;
        }
        else
        {
            Debug.LogError("VideoPlayer �� VideoClip �}�C�����t�I");
        }
    }

    // �v���`�����񧹦��ɪ��^�ը��
    void OnVideoLoopPointReached(VideoPlayer player)
    {
        // �W�[���e�v������
        currentClipIndex++;
        // �p�G���e�v�����޶W�L�}�C�d��A���m�� 0
        if (currentClipIndex >= clips.Length)
        {
            currentClipIndex = 0;
        }
        // �]�m�U�@�Ӽv�����q�ü���
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // ��s UI ��r
        UpdateStageText();
    }

    // �b�}���Q�T�ΩξP���ɨ����q�\�ƥ�
    void OnDisable()
    {
        if (vp != null)
        {
            vp.loopPointReached -= OnVideoLoopPointReached;
        }
    }

    void Update()
    {

        // �����q PipeServer ������� nodeAngles �}�C
        float[] nodeAngles = PipeServer.nodeAngles;
        if (nodeAngles != null && nodeAngles.Length > 12)
        {
            // ���o���ר��ഫ�����
            int angle = Mathf.RoundToInt(nodeAngles[11]);
            int angle12 = Mathf.RoundToInt(nodeAngles[12]);
            // �N������ܦb�C���e����
            Debug.Log(angle);
        }
       
        // �˴���L��J�ð���������ާ@
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �p�G���b����A�h�Ȱ��F�p�G�w�Ȱ��A�h��_����
            if (isPlaying)
            {
                vp.Pause();
            }
            else
            {
                vp.Play();
            }
            // �������񪬺A
            isPlaying = !isPlaying;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �p�G���b����A�h�Ȱ�
            if (isPlaying)
            {
                vp.Pause();
                isPlaying = false;
            }
        }
        else if (AngleDisplay.isIncreaseStage)
        {
            // ���U�@���v��
            NextVideo();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ���W�@���v��
            PreviousVideo();
        }
    }

    void NextVideo()
    {
        // �W�[���e�v������
        currentClipIndex++;
        // �p�G���e�v�����޶W�L�}�C�d��A���m�� 0
        if (currentClipIndex >= clips.Length)
        {
            currentClipIndex = 0;
        }
        // �]�m�U�@�Ӽv�����q�ü���
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // ��s UI ��r
        UpdateStageText();
    }

    void PreviousVideo()
    {
        // ��ַ��e�v������
        currentClipIndex--;
        // �p�G���e�v�����ޤp�� 0�A���m���}�C���̫�@�ӯ���
        if (currentClipIndex < 0)
        {
            currentClipIndex = clips.Length - 1;
        }
        // �]�m�W�@�Ӽv�����q�ü���
        vp.clip = clips[currentClipIndex];
        vp.Play();
        // ��s UI ��r
        UpdateStageText();
    }

    // ��s UI ��r
    void UpdateStageText()
    {
        if (stageText != null)
        {
            stageText.text = "Stage:" + (currentClipIndex + 1);
        }
    }
}


