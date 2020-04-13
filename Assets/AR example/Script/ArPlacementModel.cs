using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using ArResource;

namespace ArExample {
    public class ArPlacementModel : MonoBehaviour {
        // Vars
        [SerializeField]
        private GameObject pointer = null;
        
        [SerializeField]
        private GameObject model = null;

        [SerializeField]
        private GameObject uiScreen = null;

        private Camera cameraMain;

        private ARRaycastManager arReycastManager;
        private List<ARRaycastHit> arReycastHits;

        private ARPlaneManager arPlaneManager;

        private ArResourceList arResourceList;

        private Text uiPointerPosition;
        private Text uiModelPosition;
        private Text uiModelName;
        private GameObject uiSearchPlane;

        private Pose placementPose;
        private bool placementIsEnabled;
        
        private int modelSwitchCount;
        
        // Properties
        public bool isEnabled { get; set; }

        // Functions public
        public void switchModel() {
            model = arResourceList.prefabs[modelSwitchCount];

            uiModelName.text = model.name;

            modelSwitchCount ++;

            if (modelSwitchCount >= arResourceList.prefabs.Count)
                modelSwitchCount = 0;
        }

        public void insertModel() {
            if (placementIsEnabled == true) {
                uiModelPosition.text = pointer.transform.position.ToString();

                float heightHalf = model.GetComponent<Renderer>().bounds.size.y / 2;

                Instantiate(model, new Vector3(placementPose.position.x, placementPose.position.y + heightHalf, placementPose.position.z), Quaternion.identity);
            }
        }

        // Functions private
        private void Awake() {
            cameraMain = Camera.main;

            arReycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
            arReycastHits = new List<ARRaycastHit>();

            arPlaneManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();

            arResourceList = GameObject.Find("Script container").GetComponent<ArResourceList>();

            uiPointerPosition = uiScreen.transform.Find("Debug panel/Body/Pp").GetComponent<Text>();
            uiModelPosition = uiScreen.transform.Find("Debug panel/Body/Mp").GetComponent<Text>();
            uiModelName = uiScreen.transform.Find("Debug panel/Body/Mn").GetComponent<Text>();
            uiSearchPlane = uiScreen.transform.Find("Animation/Search plane").gameObject;
            
            placementPose = Pose.identity;
            placementIsEnabled = false;
            
            modelSwitchCount = 0;

            isEnabled = true;

            switchModel();
        }

        private void Update() {
            _raycast();

            _updatePointer();
        }

        private void _raycast() {
            if (isEnabled == true) {
                arPlaneManager.SetTrackablesActive(true);
                
                Vector3 screenPoint = cameraMain.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

                if (arReycastManager.Raycast(screenPoint, arReycastHits, TrackableType.Planes) == true)
                    placementIsEnabled = true;
                else
                    placementIsEnabled = false;
            }
            else {
                arPlaneManager.SetTrackablesActive(false);
                
                placementIsEnabled = false;

                uiSearchPlane.SetActive(false);
            }
        }

        private void _updatePointer() {
            if (placementIsEnabled == true) {
                uiSearchPlane.SetActive(false);

                pointer.SetActive(true);

                placementPose = arReycastHits[0].pose;

                uiPointerPosition.text = $"{placementPose.position} --- {placementPose.rotation}";
                
                Vector3 cameraForward = cameraMain.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, -90.0f, cameraForward.z).normalized;

                pointer.transform.position = new Vector3(placementPose.position.x, placementPose.position.y, placementPose.position.z);
                pointer.transform.rotation = Quaternion.LookRotation(cameraBearing);
            }
            else {
                if (isEnabled == true)
                    uiSearchPlane.SetActive(true);

                pointer.SetActive(false);
            }
        }
    }
}