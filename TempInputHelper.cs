using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomControls;

public class TempInputHelper : MonoBehaviour
{
    private CustomCarController controller;


    // Start is called before the first frame update
    private void OnEnable() =>
                             GrabCar();

    private void Start(){
        print(controller);
        
    }

    

    internal float x;
    internal float y;
    internal float brake;

    // Update is called once per frame
    private void Update(){
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
            brake = 1;
        else brake = 0;


        // print("Car Speed : " + controller.CarSpeed());

        controller.MoveCar(y, x, brake);
    }

    private void GrabCar(){
        controller = CustomCarController.Instance;
        if(controller == null){
            controller=FindObjectOfType<CustomCarController>();
        }
    }
}
