using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    private const int NumberOfSamples = 2048;

    [SerializeField]
    private GameObject m_prefab = null;

    [SerializeField]
    private float m_scalar = 64f;

    [SerializeField]
    [Range(32, 1024)] 
    private int m_numberOfObjects = 64;

    [SerializeField]
    private float m_radius = 8f;

    private GameObject[] m_sampleObjects;
    private float[] m_samples;

    protected virtual void Awake()
    {
        m_sampleObjects = new GameObject[m_numberOfObjects];
        m_samples = new float[NumberOfSamples];
    }

    protected virtual void Start()
    {
        this.InstantiatePrefabsInCircle();
    }

    protected virtual void Update()
    {
        AudioListener.GetSpectrumData(m_samples, 0, FFTWindow.Hamming);

        for (int index = 0; index < m_sampleObjects.Length; index++)
        {
            this.UpdateSpectrumObjectScale(index);
        }
    }

    private void InstantiatePrefabsInCircle()
    {
        for (int i = 0; i < m_numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2f / m_numberOfObjects;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * m_radius;
            GameObject sampleObject = Instantiate(m_prefab, position, Quaternion.identity, this.transform);

            m_sampleObjects[i] = sampleObject;
        }
    }

    private void UpdateSpectrumObjectScale(int index)
    {
        var sampleObject = m_sampleObjects[index];
        var sample = m_samples[index];

        var newScale = sampleObject.transform.localScale;
        newScale.y = sample * m_scalar;

        sampleObject.transform.localScale = newScale;
    }
}
