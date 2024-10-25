using UnityEngine;

public abstract class NoiseSource : MonoBehaviour
{
    public abstract void SetFloat(string name, float value);
    public abstract void SetInt(string name, int value);

    public abstract void GenerateNoise(float[] noise);
}
