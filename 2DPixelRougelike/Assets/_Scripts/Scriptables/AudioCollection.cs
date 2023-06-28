using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "DataSet/AudioCollection")]
public class AudioCollection : ScriptableObject
{
    public List<AudioClip> clipList = new List<AudioClip>();
}
