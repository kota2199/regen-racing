using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageData
{
    public string mainMassage;
    public string subMassage;
    public AudioClip radio;
}

[CreateAssetMenu(fileName = "PopUpMessageData", menuName = "ScriptableObjects/PopUpMessageData", order = 3)]
public class PopUpMessageData : ScriptableObject
{
    public List<MessageData> Datas;
}
