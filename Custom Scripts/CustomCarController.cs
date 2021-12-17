
/*
 *       ------ CustomControls  ------
 *       Contact Me : fy01608@gmail.com
 *       GitHub : https://github.com/Zain-ul-din
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomControls{
    enum DriveType{
        FourWheelDrive,
        RearWheelDrive
    }

 public class CustomCarController : MonoBehaviour{

        #region Singleton Design
        private CustomCarController() { }
        private static CustomCarController instance;
        public static CustomCarController Instance{
            get{
                if (instance == null)
                    instance = FindObjectOfType<CustomCarController>();
                return instance;
            }
            private set { }
        }
        #endregion

        #region Variables


        [SerializeField] private Transform[] wheelMeshs;
        [SerializeField] private WheelCollider[] wheelColliders;
        [SerializeField] private bool moveWheelFreely = false;
        [SerializeField] private Transform com;
        [SerializeField] private DriveType driveType;

        [SerializeField] private Light[] reverseLights;
        [SerializeField] private float intensityOfLight;

        public Rigidbody carRb { get; private set; }
        public bool canMove;


        // Car Engine Var
        [SerializeField] private float enginePower = 3400f;
        [SerializeField] private float brakeTorque = 3000f;
        [SerializeField] private float maxSteerAngle = 30f;
        [SerializeField] private float maxSpeed = 100f;
        [SerializeField] private bool useMaxBrakeTorque =false;


        // Car Physics Var
        [Space(10)][Header("Vehical Physics")]
        [SerializeField] private float downForce = 50f;
        private Rigidbody[] frontWheelsRb = new Rigidbody[2];

        [Space(10)][Header("Vehical Gears")]
        [SerializeField] private List<Gear> gears;
        private Gear currentGear;
        private IEnumerator gearHandler;

        public event EventHandler<Collision> OnCarCollide;
        public event EventHandler<Collider> OnCarTrigger;

        
        #endregion

        
        #region BuildInMethods

        private void Awake  () =>
        instance = this;

        private void Start  () =>
        SetUp();    

       
        private void Update () =>
        AlignWheels();

        private void FixedUpdate () =>
        ManageCarPhysics();

        private void OnCollisionEnter (Collision collision) =>
        OnCarCollide?.Invoke(this, collision);

        private void OnTriggerEnter (Collider other) =>
        OnCarTrigger?.Invoke(this, other);
        
        
        #endregion

        #region Custom Func

        private void SetUp(){
            carRb = GetComponent<Rigidbody>();
            carRb.centerOfMass = com.localPosition;

            isBraking = false;
            wheelColliders[0].attachedRigidbody.centerOfMass = Vector3.zero;
            wheelColliders[1].attachedRigidbody.centerOfMass = Vector3.zero;


            brakeTorque = (useMaxBrakeTorque) ? float.MaxValue : brakeTorque;

            // setting Parent to null so, wheels can move freely along colliders
            foreach (Transform t in wheelMeshs)
                if (moveWheelFreely) t.parent = null;

            if(gears == null){
                currentGear = new Gear { gearPower = 1.298f,speedReq = 20f };
                return;
            }

            currentGear = gears[0];

            gearHandler = changingGear(0.5f);
            StartCoroutine(gearHandler);
        }

        internal Quaternion rot;
        internal Vector3 pos;

        private void AlignWheels(){
               for(int i = 0; i < wheelColliders.Length; ++i){
                 wheelColliders[i].GetWorldPose(out pos, out rot);
                 wheelMeshs[i].position = Vector3.Lerp(wheelMeshs[i].position, pos, CarSpeed() * Time.smoothDeltaTime);
                 wheelMeshs[i].rotation = Quaternion.Lerp(wheelMeshs[i].rotation, rot, CarSpeed() * Time.smoothDeltaTime);
               }
               ManageCarPhysics();
        }

        internal bool isBraking;

        // MoveCar
        public void MoveCar(float accl,float steer,float brake){
            
            accl = Mathf.Clamp(accl, -1, 1) * enginePower * currentGear.gearPower;
            steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
            brake = Mathf.Clamp(brake, 0, 1) * brakeTorque;

            isBraking = (brake > 0) ? true : false;

            if (canMove){
                switch (driveType){
                    case DriveType.FourWheelDrive:
                        Engine(accl, steer, brake, isBraking);
                        return;
                    case DriveType.RearWheelDrive:
                        RearWheelsEngine(accl, steer, brake, isBraking);
                        return;
                }
            }
            else 
                KillEngine(); 
        }

        
        // Four Wheel Drive Engine
        internal void Engine(float accl,float steer,float brake,bool isBraking){

            
            if (!isBraking){
                carRb.drag = 0.1f;
                foreach (WheelCollider wc in wheelColliders){
                    if (CarSpeed() >= maxSpeed){
                        wc.motorTorque = 0f;
                        carRb.drag = 0.3f;
                    }

                    wc.motorTorque = accl;
                    wc.brakeTorque = 0;
                }
                TurnBackLights(false);
            }
            else { Brake();  }


            // Steering Control
            for (int i = 0; i < wheelColliders.Length - 2; i++){
                wheelColliders[i].steerAngle = steer;
            }
        }

        internal void RearWheelsEngine(float accl,float steer,float brake,bool isBraking){
            
            if (!isBraking){

                carRb.drag = 0.1f;

                for (int i = 2; i < wheelColliders.Length; i++){

                    if (CarSpeed() >= maxSpeed){
                        wheelColliders[i].motorTorque = 0f;
                        carRb.drag = 0.3f;
                    }
                    else{
                      wheelColliders[i].motorTorque = accl;
                      wheelColliders[i].brakeTorque = 0;
                    }

                }

                foreach (WheelCollider l in wheelColliders)
                    l.brakeTorque = 0;

                TurnBackLights(false);
            }
            else { Brake(); }
            


            // Steering Control
            for (int i = 0; i < wheelColliders.Length - 2; i++)
                wheelColliders[i].steerAngle = steer;
        }

        // Brake
        internal void Brake(){
            carRb.drag = 0.3f;

            TurnBackLights(true);

            if (useMaxBrakeTorque) {

                if (driveType == DriveType.RearWheelDrive){
                    foreach (WheelCollider wc in wheelColliders)
                        wc.brakeTorque = brakeTorque;
                    return;
                }

                for (int i = 0; i < wheelColliders.Length - 2; i++){
                    wheelColliders[i].brakeTorque = brakeTorque;
                   // frontWheelsRb[i].AddForce(-Vector3.up * 30f);
                }
                return;
            }

            foreach(WheelCollider wc in wheelColliders){
                wc.brakeTorque = brakeTorque;
            }
        }

        // Stops Engine
        internal void KillEngine(){
            foreach(WheelCollider wc in wheelColliders){
                wc.brakeTorque = brakeTorque/wheelColliders.Length;
            }
        }

        // Car Physics Handler
        private void ManageCarPhysics(){
            carRb.AddForce(-Vector3.up * downForce * carRb.velocity.magnitude);
        }

        // back light toggler
        internal void TurnBackLights(bool canOn){
            if (canOn)
                foreach (Light l in reverseLights)
                    l.intensity = Mathf.Lerp(l.intensity, intensityOfLight, 2.003f * Time.deltaTime);
            else
                foreach (Light l in reverseLights)
                    l.intensity = 0f;
        }
        #endregion

        #region CustomControl Utilities
        // Car Speed
        public float CarSpeed() => carRb.velocity.magnitude * 3.02f;

        #endregion

        #region Ext
        private IEnumerator changingGear(float gearShiftingDelay){
            while(true){
                yield return new WaitForSeconds(gearShiftingDelay);

                foreach (Gear gear in gears)
                    if (CarSpeed() >= gear.speedReq)
                        currentGear = gear;
            }
        }
        

        private void OnDestroy(){
            StopAllCoroutines();
        }

        private void OnDisable(){
            StopAllCoroutines();
        }

        #endregion
    } // class
}// namespace

// Car Gear
[System.Serializable]
public class Gear{
    public float speedReq;
    public float gearPower;
}



