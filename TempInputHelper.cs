using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomControls;

public class TempInputHelper : MonoBehaviour
{
    private CustomCarController controller;


    // Start is called before the first frame update
    private void OnEnable() =>
                              controller = CustomCarController.Instance;


    internal float x;
    internal float y;
    internal float brake;

    // Update is called once per frame
    void Update(){
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
            brake = 1;
        else brake = 0;

        controller.MoveCar(y, x, brake);
    }


}
