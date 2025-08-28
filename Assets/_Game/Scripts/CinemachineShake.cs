using System;
using UnityEngine;
using Unity.Cinemachine;


namespace FeedbackUtil
{
    [RequireComponent(typeof(CinemachineBasicMultiChannelPerlin))]
    public class CinemachineShake : MonoBehaviour
    {
        // public static CinemachineShake Instance {get; private set;}

        // private CinemachineCamera cVCam;
        private float shakeTimer;
        private float shakeTimerTotal;

        private float startingAmplitude;
        private float startingFrequency;

        private CinemachineBasicMultiChannelPerlin cBMCPerlin;
        private float defaultAmplitude;
        private float defaultFrequency;

        // Start is called before the first frame update
        void Awake()
        {
            Reset();
        }
        public void Reset()
        {
            // cVCam = GetComponent<CinemachineCamera>();
            cBMCPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
            

            if (cBMCPerlin != null)
            {
                defaultAmplitude = cBMCPerlin.AmplitudeGain;
                defaultFrequency = cBMCPerlin.FrequencyGain;
            }
        }
        private void Start() 
        {
            ShakeUtil.OnScreenShake += ShakeCamera;
            ShakeUtil.OnStopShaking += StopShake;
        }
        private void OnDestroy()
        {
            ShakeUtil.OnScreenShake -= ShakeCamera;
            ShakeUtil.OnStopShaking -= StopShake;
        }
        public void ShakeCamera(float intensity, float time)
        {
            if (cBMCPerlin == null)
                return;

            cBMCPerlin.AmplitudeGain = intensity;            
            shakeTimer = time;
            shakeTimerTotal = shakeTimer;
        }
        public void ShakeCamera(float intensity, float frequency, float time)
        {
            if (cBMCPerlin == null)
                return;

            startingAmplitude = intensity;
            startingFrequency = frequency;
            cBMCPerlin.AmplitudeGain = startingAmplitude;
            cBMCPerlin.FrequencyGain = startingFrequency;
            shakeTimer = time;
            shakeTimerTotal = shakeTimer;
        }
        public void StopShake()
        {
            if (cBMCPerlin == null)
                return;

            shakeTimer = 0;
            cBMCPerlin.AmplitudeGain = defaultAmplitude;
            cBMCPerlin.FrequencyGain = defaultFrequency;
        }

        // Update is called once per frame
        void Update()
        {
            if(cBMCPerlin != null && shakeTimer > 0)
            {
                if(shakeTimer >= 0)
                {
                    cBMCPerlin.AmplitudeGain = Mathf.Clamp(cBMCPerlin.AmplitudeGain - ((startingAmplitude - defaultAmplitude)/shakeTimerTotal) * Time.deltaTime, defaultAmplitude, startingAmplitude);
                    cBMCPerlin.FrequencyGain = Mathf.Clamp(cBMCPerlin.FrequencyGain - ((startingFrequency - defaultFrequency)/shakeTimerTotal) * Time.deltaTime, defaultFrequency, startingFrequency);
                }
                shakeTimer -= Time.deltaTime;
                shakeTimer = Mathf.Clamp(shakeTimer, 0f, 10f);
            }
        }
        public void ShakeTrigger()
        {
            if (cBMCPerlin == null)
                return;

            cBMCPerlin.AmplitudeGain = 2;
            shakeTimer = 0.1f;
        }
    }
    public class ShakeUtil
    {
        /// <summary>
        /// Use for a simple shake on the CinemachineVirtualCamera that has the CinemachineShake component.
        /// Parameters: intensity, frequency, duration.
        /// </summary>
        public static Action<float, float, float> OnScreenShake;
        public static Action OnStopShaking;
    }
}
