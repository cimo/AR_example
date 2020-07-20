using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ArImageTracking {
    public class ArLogicImageTracking : MonoBehaviour {
        // Vars
        [SerializeField]
        private GameObject uiScreen = null;

        private ARTrackedImageManager trackedImageManager;

        private GameObject uiTrackImage;

        // Properties

        // Functions public

        // Functions private
        private void Awake() {
            trackedImageManager = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>();

            uiTrackImage = uiScreen.transform.Find("Animation/Track image").gameObject;
            uiTrackImage.SetActive(true);
        }

        private void OnEnable() {
            trackedImageManager.trackedImagesChanged += _imagesChanged;
        }

        private void OnDisable() {
            trackedImageManager.trackedImagesChanged -= _imagesChanged;
        }

        private void _imagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
            foreach (ARTrackedImage trackedImage in eventArgs.added) {
                _updateImage(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in eventArgs.updated) {
                _updateImage(trackedImage);
            }
        }

        private void _updateImage(ARTrackedImage trackedImage) {
            uiTrackImage.SetActive(false);

            if (trackedImage.trackingState != TrackingState.None) {
                //...
            }   
        }
    }
}