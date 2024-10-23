using UnityEngine;

[CreateAssetMenu(fileName = "IndexDatabase", menuName = "Data/IndexDatabase")]
public class IndexDatabase : ScriptableObject
{
    public SpeakerIndexData Speakers;
    public CutinIndexData Cutins;

    public static IndexDatabase Instance => Resources.Load<IndexDatabase>("Database/Database");
}
