using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    private const int NumberOfSamples = 2048;

    [SerializeField]
    private float m_scalar = 50f;

    [SerializeField]
    private GameObject m_prefab = null;

    [SerializeField]
    private int m_numberOfObjects = 64;

    [SerializeField]
    private float m_radius = 8f;

    private IList<GameObject> m_sampleObjects;
    private float[] m_samples;

    protected virtual void Awake()
    {
        m_sampleObjects = new List<GameObject>(m_numberOfObjects);
        m_samples = new float[NumberOfSamples];
    }

    protected virtual void Start()
    {
        this.InstantiatePrefabsInCircle();
    }

    protected virtual void Update()
    {
        AudioListener.GetSpectrumData(m_samples, 0, FFTWindow.Hamming);

        for (int index = 0; index < m_sampleObjects.Count; index++)
        {
            this.UpdateSpectrumObjectScale(index);
        }
    }

    private void InstantiatePrefabsInCircle()
    {
        for (float i = 0; i < m_numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2f / m_numberOfObjects;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * m_radius;
            GameObject clone = Instantiate(m_prefab, position, Quaternion.identity);
            clone.transform.parent = this.transform;
            this.m_sampleObjects.Add(clone);
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
