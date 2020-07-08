using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace ArInput {
    public class ArInputTouch : MonoBehaviour {
        // Vars

        // Properties

        // Functions public
        public Vector2 touchCheck(int count, TouchPhase touchPhase) {
            if (Input.touchCount == count)
                return Vector2.zero;

            Touch touch = Input.GetTouch(0);

            if (touch.phase != touchPhase)
                return Vector2.zero;
            
            #if UNITY_ANDROID
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) == true)
                    return Vector2.zero;
            #else
                if (EventSystem.current.IsPointerOverGameObject() == true)
                    return Vector2.zero;
            #endif

            return touch.position;
        }

        // Functions private
    }
}