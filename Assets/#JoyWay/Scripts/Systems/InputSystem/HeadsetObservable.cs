using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

namespace JoyWay.Systems.InputSystem
{
    public class HeadsetObservable : MonoBehaviour
    {
        [Header("Time for Unpause")] public float timeUnpause = 1f;
        
        private SteamVR_Action_Boolean headsetOnHead;
        
        private bool isOculus;
        private bool headsetState;
        private void Start()
        {
            OVRManager.HMDUnmounted += HeadsetOff;
            OVRManager.HMDMounted += HeadsetOn;
            PauseManager.instance.onPause += UnPause;
            // isOculus = Platforms.isOculus;
            isOculus = true;
            if (isOculus) return;
            headsetOnHead = SteamVR_Input.GetBooleanAction("HeadsetOnHead");
        }

        private void UnPause(bool _state)
        {
            if (!_state)
                headsetState = false;
        }

        private void HeadsetOn()
        {
            
        }
        
        private void HeadsetOff()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            PauseManager.instance.Pause();
        }
        
        private void Update()
        {
            if (isOculus) return;
            
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            Timer();
            if (!headsetState && !headsetOnHead.state && Time.timeScale != 0.0f)
            {
                headsetState = true;
                time = timeUnpause;
                HeadsetOff();
            }
        }
        
        private float time = 0;
        void Timer()
        {
            if (time <= 0)
                return;
            time -= Time.unscaledDeltaTime;
            if (headsetOnHead.state)
            {
                time = 0;
                PauseManager.instance.UnPause();
            }
        }
        
        private void OnDestroy()
        {
            OVRManager.HMDUnmounted -= HeadsetOff;
            OVRManager.HMDMounted -= HeadsetOn;
            PauseManager.instance.onPause -= UnPause;
        }
    }
}