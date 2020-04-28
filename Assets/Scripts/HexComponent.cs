using UnityEngine;


//Unity logic for an individual hex.
public class HexComponent : MonoBehaviour
{
    public Hex Hex;
    public HexMap HexMap;

    public void UpdatePosition()
    {
        this.transform.position = Hex.PositionFromCamera(
            Camera.main.transform.position,
            HexMap.NumRows,
            HexMap.NumColumns
        );
    }
}