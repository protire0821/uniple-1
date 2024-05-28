using UnityEngine;
using UnityEngine.UI;
using System;

public class AngleDisplay : MonoBehaviour
{
    public Text angleText; // 顯示角度的文本
    public Text angle12Text; // 顯示角度12的文本
    public Text timerText; // 顯示計時器的文本
    public Text breathText; // 顯示深呼吸狀態的文本
    public Text stageText; // 顯示關卡的文本
    public Slider energyBar; // 能量條
    public Slider extraEnergyBar; // 新增的能量條
    public Slider angleEnergyBar; // 與角度相關的能量條
    public Slider angle12EnergyBar; // 與角度12相關的能量條

    private float elapsedTime = 0f; // 累積經過的時間
    private bool isBreathing = false; // 是否正在深呼吸
    private bool isDischarging = false; // 是否正在放電
    private bool isRecharging = false; // 是否正在重新充能
    private bool isExtraCharging = false; // 是否正在額外充能
    private bool isPaused = false; // 是否暫停計時器
    private bool isWaitingForSafetyBelt = false; // 是否等待安全帶計時
    private bool isWaitingForAngle12 = false; // 是否等待角度12超過165度

    private bool isCharging = false; // 是否正在充能
    private int chargeCount = 0; // 充能計數
    private int currentStage = 1; // 當前關卡

    void Start()
    {
        // 開始時將能量條歸零
        energyBar.value = 0f;
        extraEnergyBar.value = 0f;
        angleEnergyBar.value = 0f;
        angle12EnergyBar.value = 0f;
        stageText.text = "Stage: 1"; // 設置初始關卡
    }

    void Update()
    {
        // 更新計時器
        if (!isPaused)
        {
            UpdateTimer();
        }

        // 更新能量條
        UpdateEnergyBar();

        // 直接從 PipeServer 類中獲取 nodeAngles 陣列
        float[] nodeAngles = PipeServer.nodeAngles;

        // 確保 nodeAngles 已初始化並且長度足夠
        if (nodeAngles != null && nodeAngles.Length > 12)
        {
            // 取得角度並轉換為整數
            int angle = Mathf.RoundToInt(nodeAngles[11]);
            int angle12 = Mathf.RoundToInt(nodeAngles[12]);
            // 將角度顯示在遊戲畫面中
            angleText.text = "角度左: " + angle.ToString() + " 度";
            angle12Text.text = "角度右: " + angle12.ToString() + " 度";

            // 更新與角度相關的能量條，每10度為一個進度單位
            angleEnergyBar.value = Mathf.Clamp01(angle / 170f);
            angle12EnergyBar.value = Mathf.Clamp01(angle12 / 170f);

            // 第一關：檢查角度是否超過165度
            if (currentStage == 1 && angle > 165 && !isWaitingForSafetyBelt)
            {
                IncreaseStage();
            }

            // 第二關：檢查角度12是否超過165度
            if (currentStage == 2 && angle12 > 165 && !isWaitingForAngle12)
            {
                IncreaseStage();
            }
        }
        else
        {
            Debug.LogWarning("Node angles 陣列未初始化或其長度不足。");
        }

        // 檢測鍵盤按鍵事件
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DecreaseStage();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            IncreaseStage();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            TogglePauseTimer();
        }
    }

    // 更新計時器
    void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

        // 檢查是否達到 20 秒且正在等待安全帶
        if (elapsedTime >= 20f && isWaitingForSafetyBelt)
        {
            isWaitingForSafetyBelt = false; // 停止等待安全帶
            // 檢查是否角度超過165度，這時可以切換關卡
            if (PipeServer.nodeAngles[11] > 165)
            {
                IncreaseStage();
            }
        }

        // 檢查是否達到 20 秒且正在等待角度12
        if (elapsedTime >= 20f && isWaitingForAngle12)
        {
            isWaitingForAngle12 = false; // 停止等待角度12
            // 檢查是否角度12超過165度，這時可以切換關卡
            if (PipeServer.nodeAngles[12] > 165)
            {
                IncreaseStage();
            }
        }

        // 檢查是否達到 20 秒
        if (elapsedTime >= 20f && !isCharging && !isBreathing)
        {
            isCharging = true;
            InvokeRepeating("ChargeEnergyBar", 0f, 3f);
            StartBreathing();
        }

        // 檢查是否達到 35 秒（20 秒 + 15 秒充能）
        if (elapsedTime >= 35f && !isDischarging)
        {
            StartDischarging();
            breathText.text = "吐氣";
        }

        // 檢查是否達到 47 秒，重新開始充能
        if (elapsedTime >= 47f && !isRecharging)
        {
            isRecharging = true;
            StartRecharging();
            breathText.text = "";
        }

        // 檢查是否在35-47秒之間
        if (elapsedTime >= 35f && elapsedTime <= 47f)
        {
            // 將能量條歸零
            energyBar.value = 0f;
            breathText.text = "吐氣";
        }

        // 檢查是否在47-62秒之間
        if (elapsedTime >= 47f && elapsedTime <= 62f)
        {
            if (!isExtraCharging)
            {
                isExtraCharging = true;
                InvokeRepeating("ChargeExtraEnergyBar", 0f, 3f);
            }
            breathText.text = "深呼吸";
        }
    }

    // 開始深呼吸
    void StartBreathing()
    {
        isBreathing = true;
        breathText.text = "深呼吸";
        Invoke("EndBreathing", 15f);
    }

    // 結束深呼吸
    void EndBreathing()
    {
        isBreathing = false;
        breathText.text = "";
    }

    // 開始放電
    void StartDischarging()
    {
        isDischarging = true;
        InvokeRepeating("DischargeEnergyBar", 0f, 1f);
    }

    // 放電能量條
    void DischargeEnergyBar()
    {
        if (energyBar.value > 0f)
        {
            energyBar.value -= 1f / 5f; // 在五秒內將能量條值減少至零
        }
        else
        {
            isDischarging = false;
            CancelInvoke("DischargeEnergyBar");
        }
    }

    // 充能能量條
    void ChargeEnergyBar()
    {
        if (chargeCount < 5)
        {
            energyBar.value = (chargeCount + 1) * (1f / 5f); // 使用整數計算能量條值
            chargeCount++;
        }
        else
        {
            CancelInvoke("ChargeEnergyBar");
        }
    }

    // 充能額外能量條
    void ChargeExtraEnergyBar()
    {
        if (extraEnergyBar.value < 1f)
        {
            extraEnergyBar.value += 1f / 5f; // 每 3 秒將額外能量條的值增加至滿
        }
        else
        {
            isExtraCharging = false;
            CancelInvoke("ChargeExtraEnergyBar");
        }
    }

    // 更新能量條
    void UpdateEnergyBar()
    {
        // 如果充能完畢，將能量條設置為滿
        if (chargeCount >= 5)
        {
            energyBar.value = 1f;
        }
    }

    // 開始重新充能
    void StartRecharging()
    {
        InvokeRepeating("RechargeEnergyBar", 0f, 1f);
    }

    // 重新充能能量條
    void RechargeEnergyBar()
    {
        if (energyBar.value < 1f)
        {
            energyBar.value += 1f / 15f; // 在十五秒內將能量條值增加至滿
        }
        else
        {
            isRecharging = false;
            CancelInvoke("RechargeEnergyBar");
        }
    }

    // 重新啟動計時器
    void RestartTimer()
    {
        elapsedTime = 0f;
        isCharging = false;
        isBreathing = false;
        isDischarging = false;
        isRecharging = false;
        isExtraCharging = false;
        chargeCount = 0;
        energyBar.value = 0f;
        extraEnergyBar.value = 0f;
        angleEnergyBar.value = 0f;
        angle12EnergyBar.value = 0f;
    }

    // 暫停或繼續計時器
    void TogglePauseTimer()
    {
        isPaused = !isPaused;
    }

    // 增加關卡
    void IncreaseStage()
    {
        if (currentStage < 3)
        {
            currentStage++;
            stageText.text = "Stage: " + currentStage.ToString();
            RestartTimer(); // 每次增加關卡時重置計時器

            // 更新呼吸文本和狀態
            UpdateBreathText();

            if (currentStage == 2)
            {
                isWaitingForSafetyBelt = true;
                isWaitingForAngle12 = false;
            }
            else if (currentStage == 3)
            {
                isWaitingForAngle12 = true;
                isWaitingForSafetyBelt = false;
            }

            // 觸發鍵盤右鍵邏輯
            HandleRightArrowKey();
        }
    }

    // 減少關卡
    void DecreaseStage()
    {
        if (currentStage > 1)
        {
            currentStage--;
            stageText.text = "Stage: " + currentStage.ToString();
            RestartTimer(); // 每次減少關卡時重置計時器

            // 更新呼吸文本和狀態
            UpdateBreathText();

            if (currentStage == 2)
            {
                isWaitingForSafetyBelt = true;
                isWaitingForAngle12 = false;
            }
            else if (currentStage == 1)
            {
                isWaitingForSafetyBelt = false;
                isWaitingForAngle12 = false;
            }
        }
    }

    // 處理右箭頭鍵邏輯
    void HandleRightArrowKey()
    {
        // 可以在這裡放置任何需要在右箭頭鍵按下時執行的邏輯
        Debug.Log("Right arrow key logic triggered");
    }

    // 更新呼吸文本
    void UpdateBreathText()
    {
        if (currentStage == 2)
        {
            breathText.text = "安全帶";
        }
        else if (currentStage == 3)
        {
            breathText.text = "拔收劍";
        }
        else
        {
            breathText.text = "";
        }
    }
}
































