using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {

    override public void GenerateMap(){
        base.GenerateMap(); 

        int numContinents = 3;
        int continentSpacing = NumColumns / numContinents;

        //Make raised area
        for (int c = 0; c < numContinents; c++) {
            int numSplats = Random.Range(5, 7);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(3,8);
                int y = Random.Range(range, NumRows - range);
                int x = Random.Range(0, 10) - y/2 + c*20;

                ElevateArea(x, y, range);
            }
        }

        //Add lumpiness with Perlin Noise
        float noiseReolution = 1f;
        float noiseScale = 1f;
        for (int col = 0; col < NumColumns; col++) 
        {
            for (int row = 0; row < NumRows; row++) 
            {
                Hex h = GetHexAt(col, row);
                h.Elevation += noiseScale * Mathf.PerlinNoise((float) col/NumColumns / noiseReolution, (float) row/NumRows / noiseReolution);
            }
        }

        //set mesh to mounatin/hill/flat/water based on height

        //simulate rainfall/moisture and set plains/grassland + forest

        UpdateHexVisuals();
    }

    void ElevateArea(int q, int r, int range, float height = 1f) {

        Hex centerHex = GetHexAt(q, r);

        Hex[] areaHexes = GetHexesWithinRadiusOf(centerHex, range);

        foreach(Hex h in areaHexes) 
        {
            h.Elevation += height * Mathf.Lerp(1f, 0.25f, Hex.Distance(centerHex, h) / range);
        }
    }

}
