
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

 public class CustomCarController : MonoBehaviour{
        #region Variables

        [SerializeField] private Transform[] wheelMeshs;
        [SerializeField] private WheelCollider[] wheelColliders;
        [SerializeField] private Transform com;
        
        [SerializeField] private float enginePower;
        [SerializeField] private float brakeTorque;
        [SerializeField] private float maxSteerAngle;

        public Rigidbody carRb { get; private set; }
        public bool canMove { get; set; }
        
        public event EventHandler<Collision> OnCarCollide;
        public event EventHandler<Collider> OnCarTrigger;
        public static CustomCarController Instance { get; private set; } // active Car Instance
        #endregion

        
        #region BuildInMethods

        private void Awake  () =>
        Instance = this;

        private void Start  () =>
        SetUp();    
       
        private void Update () =>
        AlignWheels();

        #endregion

        #region Custom Func

        private void SetUp(){

            carRb.centerOfMass = com.position;

            // setting Parent to null so, wheels can move freely
            foreach (Transform t in wheelMeshs)
                t.parent = null;
        }

        internal Quaternion rot;
        internal Vector3 pos;

        private void AlignWheels(){
               for(int i = 0; i < wheelColliders.Length; ++i){
                wheelColliders[i].GetWorldPose(out pos, out rot);
                wheelMeshs[i].position = pos;
                wheelMeshs[i].rotation = rot;
               }
        }

        // MoveCar
        public void MoveCar(float accl,float steer,float brake){
            
            accl = Mathf.Clamp(accl, -1, 1);
            steer = Mathf.Clamp(steer, -maxSteerAngle, maxSteerAngle);
            brake = Mathf.Clamp(brake, -1, 1);

            if (canMove){
                Engine(accl, steer, brake);
            }
            else{
                KillEngine();
            }
        }

        

        internal void Engine(float accl,float steer,float brake){
            
        }

        internal void KillEngine(){
            carRb.angularDrag = 0.3f;
            foreach(WheelCollider wc in wheelColliders){
                wc.brakeTorque = brakeTorque/wheelColliders.Length;
            }
        }
        #endregion

        private void OnCollisionEnter(Collision collision){
            OnCarCollide?.Invoke(this, collision);
        }

        private void OnTriggerEnter(Collider other){
            OnCarTrigger?.Invoke(this, other);
        }
    }
}// namespace
