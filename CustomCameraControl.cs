using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomControls{
    public class CustomCameraControl : MonoBehaviour{
        #region Var
        [SerializeField] private Transform target;
        private Camera camera;

        #endregion

        #region Custom Method

        private void Start() =>
        Setup();

        private void Update() =>
            FollowTarget();
        #endregion

        #region Custom Methods
        private void Setup(){
            camera = GetComponent<Camera>();
            if (camera == null) camera = Camera.main;

        }

        private void FollowTarget(){
            transform.LookAt(target);
        }
        #endregion
    } // class
}// namespace
