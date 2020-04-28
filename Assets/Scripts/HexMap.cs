using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handler for all Hexes in the map
public class HexMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GenerateMap();
	}

    public GameObject HexPrefab;

    public Mesh MeshWater;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;

    public Material MatOcean;
    public Material MatGrass;
    public Material MatPlains;
    public Material MatMarsh;
    public Material MatDesert;
    public Material MatTundra;
    public Material MatStone;

    
    public readonly int NumColumns = 74;
    public readonly int NumRows = 46;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObject = new Dictionary<Hex, GameObject>();

    public Hex GetHexAt(int x, int y) {
        if (hexes == null) {
            Debug.LogError("Hexes array not yet instantiated!");
            throw new UnityException("Hexes array not yet instantiated!");
        }
        return hexes[Utility.mod(x, NumColumns), y];
    }
    
    virtual public void GenerateMap(){

        hexes = new Hex[NumColumns, NumRows];

        //Generate All Ocean;
        for (int col = 0; col < NumColumns; col++) {
            for (int row = 0; row < NumRows; row++) {

                //Instantiate a hex
                Hex h = new Hex(col, row);
                h.Elevation = -0.5f;
                h.Moisture = 0;
                h.Temperature = 10;

                hexes[col, row] = h;

                Vector3 pos = h.PositionFromCamera(
                    Camera.main.transform.position,
                    NumRows,
                    NumColumns
                );

                GameObject hex_go = (GameObject) Instantiate(
                    HexPrefab, 
                    pos,
                    Quaternion.identity,
                    this.transform
                );

                hexToGameObject[h] = hex_go;

                hex_go.name = string.Format("HEX: {0},{1}", col, row);
                hex_go.GetComponent<HexComponent>().Hex = h;
                hex_go.GetComponent<HexComponent>().HexMap = this;

                hex_go.GetComponentInChildren<TextMesh>().text = string.Format("C{0},R{1}", col, row);

            }
        }
        UpdateHexVisuals();
    }

    public void UpdateHexVisuals() {
        for (int col = 0; col < NumColumns; col++) {
            for (int row = 0; row < NumRows; row++) 
            {
                Hex h = hexes[col, row];
                GameObject hex_go = hexToGameObject[h];


                MeshRenderer mr = hex_go.GetComponentInChildren<MeshRenderer>();
                if (h.Elevation >= 0 ) {
                    mr.material = MatGrass;
                }
                else {
                    mr.material = MatOcean;
                }

                MeshFilter mf = hex_go.GetComponentInChildren<MeshFilter>();
                mf.mesh = MeshWater;
            }
        }
    }

    public Hex[] GetHexesWithinRadiusOf(Hex centerHex, int radius) {

        List<Hex> results = new List<Hex>();

        for (int dx = -radius; dx <= radius; dx++) 
        {
            for (int dy = Mathf.Max(-radius, -dx - radius); dy <= Mathf.Min(radius, -dx + radius); dy++)
            {
                int x = Utility.mod(centerHex.Q + dx, NumColumns);
                int y = centerHex.R + dy;

                if (y >= 0 && y < NumRows) {
                    try {
                        results.Add(hexes[x, y]);
                    }
                    catch(Exception e) {
                        Debug.LogError(String.Format("Could not find hex at {0}, {1}.", x, y));
                    }
                }

            }
        }
        return results.ToArray();
    }

	
}
