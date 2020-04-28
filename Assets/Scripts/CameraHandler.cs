using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    Vector3 prevPosition;
    HexComponent[] hexes;

	// Use this for initialization
	void Start () {
        prevPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //WASD

        //Zoom

        CheckIfCameraMoved();
	}

    public void PanToHex(Hex hex)
    {

    }

    void CheckIfCameraMoved()
    {
        if (prevPosition != this.transform.position)
        {
            prevPosition = this.transform.position;

            if (hexes == null) 
            {
                hexes = GameObject.FindObjectsOfType<HexComponent>();
            }

            foreach (HexComponent hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
