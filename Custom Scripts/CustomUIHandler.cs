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

        
        [SerializeField] private SteerPos steerPos;

        // Toggler
        [SerializeField] private UiTogglerBtn steerToggler, btnToggler, gyroToggler;
        private UiTogglerBtn activeBtn;


        private void Start()
        => SetUp();

        // adds event handlers
        private void SetUp(){

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

            customInput = CustomInput.Instance;
            customInput.OnControlChange += CustomInput_OnControlChange;
        }

        // On Control Change
        private void CustomInput_OnControlChange(object sender, ControlType e) => UpdateUI(e);
        

        // Update Change
        private void UpdateUI(ControlType control){
            switch (control){
                case ControlType.SteeringControl:

                    break;
                case ControlType.BtnControls:

                    break;
                case ControlType.GyroControls:

                    break;
                case ControlType.KeyboardControl:

                    break;
            }
        }

        // Toggle Btn Active Before
        private void Toggle(ref UiTogglerBtn active , ref UiTogglerBtn toToggle){
            active.SetBtnSprite(UiTogglerBtn.SpriteType.OnUnActive);
            active = toToggle;
            toToggle.SetBtnSprite(UiTogglerBtn.SpriteType.OnActive);
        }
    }

    
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
                image = btn.gameObject.GetComponent<Image>();
                return image;
            }
            private set{}
        }
        public ControlType btnControlType;

        public void SetBtnSprite(SpriteType state){
            if (image == null) image = btn.gameObject.GetComponent<Image>();
            if (state == SpriteType.OnActive)
                image.sprite = onActive;
            else image.sprite = onUnActive;
        }

    }

} // namespace