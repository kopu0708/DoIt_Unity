using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int maxLevel;
    [SerializeField]
    private float[] maxExperience;

    public int MaxLevel => maxLevel;
    public float[] MaxExperience => maxExperience;
}
