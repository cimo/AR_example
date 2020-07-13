using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using ArInput;

namespace ArAnchor {
    public class ArLogicAnchor : MonoBehaviour {
        // Vars
        [SerializeField]
        private GameObject uiScreen = null;

        private ARRaycastManager arReycastManager;
        private List<ARRaycastHit> arReycastHits;
        
        private ARPointCloudManager arPointCloudManager;

        private ARAnchorManager arAnchorManager;

        private ArInputTouch arInputTouch;

        private List<ARAnchor> anchors;

        private GameObject uiTap;

        private bool placementIsEnabled;

        // Properties
        public bool isEnabled { get; set; }

        // Functions public

        // Functions private
        private void Awake() {
            arReycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
            arReycastHits = new List<ARRaycastHit>();

            arPointCloudManager = GameObject.Find("AR Session Origin").GetComponent<ARPointCloudManager>();

            arAnchorManager = GameObject.Find("AR Session Origin").GetComponent<ARAnchorManager>();

            arInputTouch = GameObject.Find("Script container").GetComponent<ArInputTouch>();

            anchors = new List<ARAnchor>();

            uiTap = uiScreen.transform.Find("Animation/Tap").gameObject;
            
            placementIsEnabled = false;

            isEnabled = false;
        }

        private void Update() {
            _raycast();

            _insert();
        }

        private void _raycast() {
            if (isEnabled == true) {
                arPointCloudManager.SetTrackablesActive(true);

                Vector2 position = arInputTouch.touchCheck(0, TouchPhase.Began);

                if (arReycastManager.Raycast(position, arReycastHits, TrackableType.FeaturePoint) == true && position != Vector2.zero)
                    placementIsEnabled = true;
                else
                    placementIsEnabled = false;
            }
            else {
                arPointCloudManager.SetTrackablesActive(false);

                placementIsEnabled = false;
            }
        }

        private void _insert() {
            if (placementIsEnabled == true) {
                uiTap.SetActive(false);

                ARAnchor anchor = arAnchorManager.AddAnchor(arReycastHits[0].pose);
            
                if (anchor != null)
                    anchors.Add(anchor);
            }
        }
    }
}