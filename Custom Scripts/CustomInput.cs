using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomControls{
    public enum ControlType{
        SteeringControl,
        BtnControls,
        GyroControls,
        KeyboardControl
    }

    public class CustomInput : MonoBehaviour{

        #region Instance
        private static CustomInput instance;
        public static CustomInput Instance{
            get{
                if(instance == null){
                     instance = FindObjectOfType<CustomInput>();
                }
                return instance;
            }
            private set { }
        }

        private CustomInput() { }
        #endregion

        #region Var

        private Button gearBtn;
        private bool isReversing;

        public CustomUIButton raceBtn , brakeBtn , leftMoveBtn , rightMoveBtn;
        public SteeringWheel steer;
        [SerializeField]private  CustomCarController controller;

        [SerializeField] private ControlType controlType;
        [SerializeField]private float gyroScopeSens = 2f;

        public event EventHandler<ControlType> OnControlChange;

        internal float accl, brakeFlag, steerInput , left , right;
        
        #endregion

        #region Build In
        private void Awake()
        => instance = this;

        private void OnEnable()
            => 
        SetUp();

        private void Update()
            => InputUpdate();

        #endregion

        #region Custom Method
        private void SetUp(){
            
            /*
            // gear Toggler
            gearBtn.onClick.AddListener(() =>{
                isReversing = !isReversing;
                // More Actions
            });
                
            */
          //  controller = CustomCarController.Instance;
           // if (controller == null) controller = FindObjectOfType<CustomCarController>();
            
        }

        private void InputUpdate(){

            switch (controlType){
                case ControlType.SteeringControl:
                   accl = (raceBtn.isPressing) ? (isReversing) ? -1 : 1: 0f;
                   brakeFlag = (brakeBtn.isPressing) ? 1f : 0f;
                   steerInput = steer.GetClampedValue();
                break;
                case ControlType.BtnControls:
                    accl = (raceBtn.isPressing) ? (isReversing) ? -1 : 1 : 0f;
                    brakeFlag = (brakeBtn.isPressing) ? 1f : 0f;

                    left = (leftMoveBtn.isPressing) ? -1 : 0f;
                    right = (rightMoveBtn.isPressing) ? 1 : 0f;
                    
                    steerInput = (left > right) ? left : right;
                break;
                case ControlType.GyroControls:
                    accl = (raceBtn.isPressing) ? (isReversing) ? -1 : 1 : 0f;
                    brakeFlag = (brakeBtn.isPressing) ? 1f : 0f;
                    steerInput = Input.acceleration.x * gyroScopeSens;
                break;
                case ControlType.KeyboardControl:
                    accl = Input.GetAxis("Vertical");
                    steerInput = Input.GetAxis("Horizontal");
                    brakeFlag = (Input.GetKey(KeyCode.Space)) ? 1f : 0f;
                    if (Input.GetKeyDown(KeyCode.R)) isReversing = !isReversing;
                    
                break;
            }
            
            controller.MoveCar(accl, steerInput, brakeFlag);
            
        }
        #endregion

        #region Public Methods
        public void SetControlsType(ControlType controlType) {
            this.controlType = controlType;
            OnControlChange?.Invoke(this, this.controlType);
        }
        #endregion
    }
}