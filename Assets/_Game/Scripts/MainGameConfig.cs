using UnityEngine;

namespace TaoistFlip
{
    public class MainGameConfig : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
        }
    }
}