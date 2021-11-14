
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

        public Rigidbody carRb { get; private set; }
        public bool canMove { get; set; }

        // Car Engine Var
        [SerializeField] private float enginePower = 3400;
        [SerializeField] private float brakeTorque = 3000;
        [SerializeField] private float maxSteerAngle = 30;

        // Car Physics Var
        [SerializeField] private float downForce = 50;
        

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

        internal bool isBraking;

        // MoveCar
        public void MoveCar(float accl,float steer,float brake){
            
            accl = Mathf.Clamp(accl, -1, 1) * enginePower;
            steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
            brake = Mathf.Clamp(brake, 0, 1) * brakeTorque;

            isBraking = (brake > 0) ? true : false;

            if (canMove){
                if (!isBraking){
                   Engine(accl, steer, brake);
                }
                else{
                    Brake();
                }
            }
            else {
                KillEngine();
            }
            
        }

        

        internal void Engine(float accl,float steer,float brake){
            foreach(WheelCollider wc in wheelColliders) {
                wc.motorTorque = accl;
                wc.brakeTorque = 0;
            }

            for(int i = 0; i < wheelColliders.Length - 2; i++){
                wheelColliders[i].steerAngle = steer;
            }
        }

        internal void KillEngine(){
            foreach(WheelCollider wc in wheelColliders){
                wc.brakeTorque = brakeTorque/wheelColliders.Length;
            }
        }

        internal void Brake(){
            foreach(WheelCollider wc in wheelColliders){
                wc.brakeTorque = brakeTorque;
            }
        }

        private void ManageCarPhysics(){
            carRb.AddForce(-Vector3.up * downForce * carRb.velocity.magnitude);
        }
        #endregion

        
    }
}// namespace
