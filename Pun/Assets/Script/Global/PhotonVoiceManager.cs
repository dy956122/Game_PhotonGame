using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.Unity;
public class PhotonVoiceManager : MonoBehaviour
{
    Recorder recorder;
    void Start()
    {
        recorder= gameObject.AddComponent<Recorder>();
        recorder.TransmitEnabled = true;
        recorder.VoiceDetection = true;
    }
}
