using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

namespace ArLight {
    public class ArLightEstimation : MonoBehaviour {
        // Vars
        [SerializeField]
        private ARCameraManager arCameraManager = null;
        
        [SerializeField]
        private Light lightMain = null;

        [SerializeField]
        private GameObject ui3d = null;

        [SerializeField]
        private GameObject uiScreen = null;

        private GameObject ui3dArrow;
        
        private Text uiAverageBrightness;
        private Text uiAverageColorTemperature;
        private Text uiColorCorrection;
        private Text uiMainLightColor;
        private Text uiAverageMainLightBrightness;
        private Text uiAmbientSphericalHarmonic;
        private Text uiLightDirection;
        
        private float lightMainIntensity;
        private float lightMainColorTemperature;
        private Color lightMainColor;
        private Quaternion lightMainRotation;

        // Properties
        public bool isEnabled { get; set; }

        public GameObject Arrow {
            get { return ui3dArrow; }
        }

        // Functions public

        // Functions private
        private void Awake() {
            ui3dArrow = ui3d.transform.Find("Arrow").gameObject;

            uiAverageBrightness = uiScreen.transform.Find("Debug panel/Body/Ab").GetComponent<Text>();
            uiAverageColorTemperature = uiScreen.transform.Find("Debug panel/Body/Act").GetComponent<Text>();
            uiColorCorrection = uiScreen.transform.Find("Debug panel/Body/Cc").GetComponent<Text>();
            uiMainLightColor = uiScreen.transform.Find("Debug panel/Body/Mlc").GetComponent<Text>();
            uiAverageMainLightBrightness = uiScreen.transform.Find("Debug panel/Body/Amlb").GetComponent<Text>();
            uiAmbientSphericalHarmonic = uiScreen.transform.Find("Debug panel/Body/Ash").GetComponent<Text>();
            uiLightDirection = uiScreen.transform.Find("Debug panel/Body/Ld").GetComponent<Text>();

            lightMainIntensity = lightMain.intensity;
            lightMainColorTemperature = lightMain.colorTemperature;
            lightMainColor = lightMain.color;
            lightMainRotation = lightMain.transform.rotation;

            isEnabled = true;
        }

        private void OnEnable() {
            arCameraManager.frameReceived += _frameUpdated;
        }

        private void OnDisable() {
            arCameraManager.frameReceived -= _frameUpdated;
        }

        private void _frameUpdated(ARCameraFrameEventArgs args) {
            if (isEnabled == true) {
                ui3dArrow.SetActive(true);

                if (args.lightEstimation.averageBrightness.HasValue) {
                    uiAverageBrightness.text = args.lightEstimation.averageBrightness.Value.ToString();

                    lightMain.intensity = args.lightEstimation.averageBrightness.Value;
                }

                if (args.lightEstimation.averageColorTemperature.HasValue) {
                    uiAverageColorTemperature.text = args.lightEstimation.averageColorTemperature.Value.ToString();

                    lightMain.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
                }

                if (args.lightEstimation.colorCorrection.HasValue) {
                    uiColorCorrection.text = args.lightEstimation.colorCorrection.Value.ToString();
                
                    lightMain.color = args.lightEstimation.colorCorrection.Value;
                }

                if (args.lightEstimation.mainLightColor.HasValue) {
                    uiMainLightColor.text = args.lightEstimation.mainLightColor.Value.ToString();

                    lightMain.color = args.lightEstimation.mainLightColor.Value;
                }

                if (args.lightEstimation.averageMainLightBrightness.HasValue) {
                    uiAverageMainLightBrightness.text = args.lightEstimation.averageMainLightBrightness.Value.ToString();

                    lightMain.intensity = args.lightEstimation.averageMainLightBrightness.Value;
                }

                if (args.lightEstimation.ambientSphericalHarmonics.HasValue) {
                    uiAmbientSphericalHarmonic.text = args.lightEstimation.ambientSphericalHarmonics.Value.ToString();

                    RenderSettings.ambientMode = AmbientMode.Skybox;
                    RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
                }

                if (args.lightEstimation.mainLightDirection.HasValue) {
                    uiLightDirection.text = args.lightEstimation.mainLightDirection.Value.ToString();

                    lightMain.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);

                    ui3dArrow.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
                }
            }
            else {
                ui3dArrow.SetActive(false);

                uiAverageBrightness.text = lightMainIntensity.ToString();
                uiAverageColorTemperature.text = lightMainColorTemperature.ToString();
                uiColorCorrection.text = lightMainColor.ToString();
                uiMainLightColor.text = lightMainColor.ToString();
                uiAverageMainLightBrightness.text = lightMainIntensity.ToString();
                
                lightMain.intensity = lightMainIntensity;
                lightMain.colorTemperature = lightMainColorTemperature;
                lightMain.color = lightMainColor;
                lightMain.transform.rotation = lightMainRotation;
            }
        }
    }
}