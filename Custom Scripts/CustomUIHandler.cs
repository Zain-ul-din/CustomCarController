using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CustomControls{
    public enum SteerPos{
        LeftSide,
        RightSide
    }

    public class CustomUIHandler : MonoBehaviour{
        private CustomInput customInput;
        // Toggler
        [Space(10)]
        [SerializeField] private UiTogglerBtn steerToggler, btnToggler, gyroToggler;
        [Space(10)]
        [SerializeField] private UiTogglerBtn leftSteerSetter, rightSteerSetter;
         
        [Space(10)][Header("Btn Pos ")]
        [SerializeField] private BtnPos steeringPos, leftBtnPos, rightBtnPos ,raceBtnPos , brakeBtnPos; 
        
        private UiTogglerBtn activeBtn;
        private SteerPos steerPos;
        
        private void Start()
        => SetUp();

        // adds event handlers
        private void SetUp() {
            customInput = CustomInput.Instance;
            // Loaded Data 
            Data loadedData= CustomDataManager.GetData();
            steerPos = loadedData.steerPos;
            
            customInput.SetControlsType(loadedData.controlType);
            activeBtn = steerToggler;
            
            // Event Handler
            steerToggler.btn.onClick.AddListener(() => {
                Toggle(ref activeBtn, ref steerToggler);
                customInput.SetControlsType(activeBtn.btnControlType);
            });

            btnToggler.btn.onClick.AddListener(() => {
                Toggle(ref activeBtn, ref btnToggler);
                customInput.SetControlsType(activeBtn.btnControlType);
            });

            gyroToggler.btn.onClick.AddListener(() => {
                Toggle(ref activeBtn, ref gyroToggler);
                customInput.SetControlsType(activeBtn.btnControlType);
            });
            
            // Steer Toggler
            leftSteerSetter.btn.onClick.AddListener(() => {
                steerPos = SteerPos.LeftSide;
                leftSteerSetter.SetBtnSprite(UiTogglerBtn.SpriteType.OnActive);
                rightSteerSetter.SetBtnSprite(UiTogglerBtn.SpriteType.OnUnActive);
                ChangeSteerPos(SteerPos.LeftSide);
                
            });
            
            // Steer Toggler
            rightSteerSetter.btn.onClick.AddListener(() => {
                steerPos = SteerPos.RightSide;
                leftSteerSetter.SetBtnSprite(UiTogglerBtn.SpriteType.OnUnActive);
                rightSteerSetter.SetBtnSprite(UiTogglerBtn.SpriteType.OnActive);
                ChangeSteerPos(SteerPos.RightSide);
            });
            
            customInput = CustomInput.Instance;
            customInput.OnControlChange += CustomInput_OnControlChange;
        }

        // On Control Change
        private void CustomInput_OnControlChange(object sender, ControlType e) => UpdateUI(e);
        

        // Update Change
        private void UpdateUI(ControlType control) {
            CustomDataManager.controlType = control;
            switch (control){
                case ControlType.SteeringControl:
                   // switch to Steer Controls
                   customInput.leftMoveBtn.gameObject.SetActive(false);
                   customInput.rightMoveBtn.gameObject.SetActive(false);
                   customInput.steer.gameObject.SetActive(true);
                    break;
                case ControlType.BtnControls:
                    // Switch to btn control
                    customInput.leftMoveBtn.gameObject.SetActive(true);
                    customInput.rightMoveBtn.gameObject.SetActive(true);
                    customInput.steer.gameObject.SetActive(false);
                    break;
                case ControlType.GyroControls:
                    customInput.steer.gameObject.SetActive(false);
                    customInput.leftMoveBtn.gameObject.SetActive(false);
                    customInput.rightMoveBtn.gameObject.SetActive(false);
                    break;
                case ControlType.KeyboardControl:
                    // No Btn
                    customInput.leftMoveBtn.gameObject.SetActive(false);
                    customInput.rightMoveBtn.gameObject.SetActive(false);
                    customInput.raceBtn.gameObject.SetActive(false);
                    customInput.brakeBtn.gameObject.SetActive(false);
                    customInput.steer.gameObject.SetActive(false);
                    break;
            }

            
        }

        void ChangeSteerPos(SteerPos steerPos) {
            if (activeBtn.btnControlType == ControlType.KeyboardControl) return;
            switch (steerPos) {
                case SteerPos.LeftSide:
                    // Set Btns to Right
                        customInput.leftMoveBtn.GetComponent<RectTransform>().position = leftBtnPos.onLeft.position;
                        customInput.rightMoveBtn.GetComponent<RectTransform>().position = rightBtnPos.onLeft.position;
                        customInput.raceBtn.GetComponent<RectTransform>().position = raceBtnPos.onLeft.position;
                        customInput.brakeBtn.GetComponent<RectTransform>().position = brakeBtnPos.onLeft.position;
                        customInput.steer.gameObject.GetComponent<RectTransform>().position = steeringPos.onLeft.position;
                    return;

                        break;
                case SteerPos.RightSide:
                    // set btn to right side
                        customInput.leftMoveBtn.GetComponent<RectTransform>().position= leftBtnPos.onRight.position;
                        customInput.rightMoveBtn.GetComponent<RectTransform>().position = rightBtnPos.onRight.position;
                        customInput.raceBtn.GetComponent<RectTransform>().position = raceBtnPos.onRight.position;
                        customInput.brakeBtn.GetComponent<RectTransform>().position = brakeBtnPos.onRight.position;
                        customInput.steer.gameObject.GetComponent<RectTransform>().position = steeringPos.onLeft.position;
                        return;

                        break;
            }

            CustomDataManager.steerPos = steerPos;
        }
        
        // Helper Toggle Btn Active Before
        private void Toggle(ref UiTogglerBtn active , ref UiTogglerBtn toToggle){
            active.SetBtnSprite(UiTogglerBtn.SpriteType.OnUnActive);
            active = toToggle;
            toToggle.SetBtnSprite(UiTogglerBtn.SpriteType.OnActive);
        }
    } //  class

    
    // Container For Toggler Class
    [System.Serializable]
    public class UiTogglerBtn {
        public enum SpriteType{
            OnActive,
            OnUnActive
        }

        public Button btn;
        public Sprite onActive, onUnActive;
        public Image image{
            get{
                if(image == null)
                image = btn.gameObject.GetComponent<Image>();
                return image;
            }
            private set {  }
        }
        public ControlType btnControlType;

        public void SetBtnSprite(SpriteType state){
            image = btn.gameObject.GetComponent<Image>();
            if (state == SpriteType.OnActive)
                image.sprite = onActive;
            else image.sprite = onUnActive;
        }
        
    }

    [System.Serializable]
    class BtnPos {
        public RectTransform onLeft, onRight;
    }
} // namespace