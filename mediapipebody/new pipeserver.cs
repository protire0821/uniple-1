using System.Collections;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class PipeServer : MonoBehaviour
{
    public Transform parent;
    public GameObject characterPrefab; // 新的3D人物模型
    public bool anchoredBody = false;
    public float multiplier = 10f;
    public float maxSpeed = 50f;
    public int samplesForPose = 1;

    private Body body;
    private NamedPipeServerStream server;

    const int LANDMARK_COUNT = 33;

    public struct AccumulatedBuffer
    {
        public Vector3 value;
        public int accumulatedValuesCount;
        public AccumulatedBuffer(Vector3 v, int ac)
        {
            value = v;
            accumulatedValuesCount = ac;
        }
    }

    public class Body
    {
        public Transform parent;
        public AccumulatedBuffer[] positionsBuffer = new AccumulatedBuffer[LANDMARK_COUNT];
        public GameObject character; // 新的3D人物模型實例

        public bool active;

        public Body(Transform parent, GameObject characterPrefab)
        {
            this.parent = parent;
            character = Instantiate(characterPrefab); // 實例化新的3D人物模型
            character.transform.parent = parent;
        }
    }

    private void Start()
    {
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        body = new Body(parent, characterPrefab);

        Thread t = new Thread(new ThreadStart(Run));
        t.Start();
    }

    private void Update()
    {
        UpdateBody(body);
    }

    private void UpdateBody(Body b)
    {
        if (b.active == false) return;

        // 更新身體的其他邏輯...

        // 以下是示例代碼，假設需要更新3D人物的位置和旋轉
        // 你需要根據你的模型結構和需要進行相應的更新

        for (int i = 0; i < LANDMARK_COUNT; ++i)
        {
            if (b.positionsBuffer[i].accumulatedValuesCount < samplesForPose)
                continue;
            // 更新新的3D人物模型的位置
            b.character.transform.position = b.positionsBuffer[i].value / (float)b.positionsBuffer[i].accumulatedValuesCount * multiplier;
            b.positionsBuffer[i] = new AccumulatedBuffer(Vector3.zero, 0);
        }

        // 更新其他3D人物模型的相關邏輯...
    }

    private void Run()
    {
        // 原始的命名管道通信邏輯...
    }

    private void OnDisable()
    {
        // 原始的關閉命名管道邏輯...
    }
}

