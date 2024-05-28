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
    public GameObject characterPrefab; // �s��3D�H���ҫ�
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
        public GameObject character; // �s��3D�H���ҫ����

        public bool active;

        public Body(Transform parent, GameObject characterPrefab)
        {
            this.parent = parent;
            character = Instantiate(characterPrefab); // ��ҤƷs��3D�H���ҫ�
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

        // ��s���骺��L�޿�...

        // �H�U�O�ܨҥN�X�A���]�ݭn��s3D�H������m�M����
        // �A�ݭn�ھڧA���ҫ����c�M�ݭn�i���������s

        for (int i = 0; i < LANDMARK_COUNT; ++i)
        {
            if (b.positionsBuffer[i].accumulatedValuesCount < samplesForPose)
                continue;
            // ��s�s��3D�H���ҫ�����m
            b.character.transform.position = b.positionsBuffer[i].value / (float)b.positionsBuffer[i].accumulatedValuesCount * multiplier;
            b.positionsBuffer[i] = new AccumulatedBuffer(Vector3.zero, 0);
        }

        // ��s��L3D�H���ҫ��������޿�...
    }

    private void Run()
    {
        // ��l���R�W�޹D�q�H�޿�...
    }

    private void OnDisable()
    {
        // ��l�������R�W�޹D�޿�...
    }
}

