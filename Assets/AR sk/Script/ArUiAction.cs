using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ArLight;
using ArModel;
using ArAnchor;
using ArImageTracking;

namespace ArUi {
    public class ArUiAction : MonoBehaviour {
        // Vars
        [SerializeField]
        private Camera uiCamera = null;

        [SerializeField]
        private GameObject uiScreen = null;

        [SerializeField]
        private GameObject modelContainer = null;

        private Toggle settingToggle;
        private Button quitButton;
        private Toggle anchorToggle;
        private Button switchModelButton;
        private Button addModelButton;

        private GameObject settingPanel;
        private Toggle debugToggle;
        private Dropdown sceneDropdown;
        private Toggle ambientLightToggle;
        private Button settingCloseButton;

        private GameObject debugPanel;

        private GameObject uiTap;

        private Transform interactiveModel;

        private ArLightEstimation arLightEstimation;
        private ArLogicModel arLogicModel;
        private ArLogicAnchor arLogicAnchor;
        private ArLogicImageTracking arLogicImageTracking;

        // Properties
        
        // Functions public
        
        // Functions private
        private void Awake() {
            settingToggle = uiScreen.transform.Find("Setting toggle").GetComponent<Toggle>();
            quitButton = uiScreen.transform.Find("Bottom bar/Exit button").GetComponent<Button>();
            anchorToggle = uiScreen.transform.Find("Bottom bar/Anchor toggle").GetComponent<Toggle>();
            switchModelButton = uiScreen.transform.Find("Bottom bar/Switch model button").GetComponent<Button>();
            addModelButton = uiScreen.transform.Find("Bottom bar/Add model button").GetComponent<Button>();

            settingPanel = uiScreen.transform.Find("Setting panel").gameObject;
            debugToggle = settingPanel.transform.Find("Body/Debug toggle").GetComponent<Toggle>();
            sceneDropdown = settingPanel.transform.Find("Body/Scene dropdown").GetComponent<Dropdown>();
            ambientLightToggle = settingPanel.transform.Find("Body/Ambient light toggle").GetComponent<Toggle>();
            settingCloseButton = settingPanel.transform.Find("Footer/Close button").GetComponent<Button>();

            debugPanel = uiScreen.transform.Find("Debug panel").gameObject;

            uiTap = uiScreen.transform.Find("Animation/Tap").gameObject;

            interactiveModel = modelContainer.transform.Find("Interactive").GetComponent<Transform>();

            arLightEstimation = GameObject.Find("Script container").GetComponent<ArLightEstimation>();
            arLogicModel = GameObject.Find("Script container").GetComponent<ArLogicModel>();
            arLogicAnchor = GameObject.Find("Script container").GetComponent<ArLogicAnchor>();
            arLogicImageTracking = GameObject.Find("Script container").GetComponent<ArLogicImageTracking>();
        }

        private void Start() {
            #if UNITY_ANDROID
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            #endif

            settingToggle.onValueChanged.AddListener(delegate {
                _eventOpenSetting(settingToggle);
            });

            debugToggle.onValueChanged.AddListener(delegate {
                _eventOpenDebug(debugToggle);
            });

            sceneDropdown.onValueChanged.AddListener(delegate {
                _eventChangeScene(sceneDropdown);
            });

            ambientLightToggle.onValueChanged.AddListener(delegate {
                _eventAmbientLight(ambientLightToggle);
            });

            settingCloseButton.onClick.AddListener(_eventSettingClose);

            quitButton.onClick.AddListener(_eventQuit);
            
            anchorToggle.onValueChanged.AddListener(delegate {
                _eventAnchor(anchorToggle);
            });

            switchModelButton.onClick.AddListener(_eventSwitchModel);

            addModelButton.onClick.AddListener(_eventAddModel);
        }

        private void OnEnable() {
            Application.onBeforeRender += _onBeforeRender;
        }

        private void OnDisable() {
            Application.onBeforeRender -= _onBeforeRender;
        }

        private void _onBeforeRender() {
            Vector3 position = uiCamera.ViewportToWorldPoint(new Vector3(0.9f, 0.95f, uiCamera.nearClipPlane + 0.25f));

            arLightEstimation.Arrow.transform.position = position;
        }

        private void _eventOpenSetting(Toggle change) {
            settingPanel.SetActive(change.isOn);
        }

        private void _eventOpenDebug(Toggle change) {
            debugPanel.SetActive(change.isOn);
        }

        private void _eventChangeScene(Dropdown change) {
            if (change.value > 0)
                SceneManager.LoadScene(sceneDropdown.options[change.value].text, LoadSceneMode.Single);
        }

        private void _eventAmbientLight(Toggle change) {
            arLightEstimation.isEnabled = change.isOn;
        }

        private void _eventSettingClose() {
            settingToggle.isOn = false;
        }

        private void _eventQuit() {
            Application.Quit();
        }

        private void _eventAnchor(Toggle change) {
            uiTap.SetActive(change.isOn);

            switchModelButton.interactable = !change.isOn;
            addModelButton.interactable = !change.isOn;

            arLogicAnchor.isEnabled = change.isOn;
            arLogicModel.isEnabled = !change.isOn;
        }

        private void _eventSwitchModel() {
            arLogicModel.switchModel();
        }

        private void _eventAddModel() {
            arLogicModel.insertModel();
        }
    }
}