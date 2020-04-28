using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data Representation of an individual hex.
/// The Hex class defines the grid position, world space position, size, 
/// neighbours, etc. of a hex tile
/// </summary>
public class Hex
{
    //Q + R + S = 0;
    //S = -(Q + R);

    public readonly int Q; //Column
    public readonly int R; //Row
    public readonly int S; //Sum

    public float radius = 1f;
    static float WIDTH_RATIO = Mathf.Sqrt(3) / 2;

    public float Elevation;
    public float Moisture;
    public float Temperature;



    public Hex (int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    //Return world-space position;
    public Vector3 Position()
    {
        float horiz = HexHorizontalSpacing();
        float vert = HexVerticalSpacing();

        return new Vector3(
            horiz * (this.Q + this.R/2f),
            0,
            vert * this.R
            );
    }

    public float HexHeight()
    {
        return radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_RATIO * HexHeight();
    }

    public float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    public float HexHorizontalSpacing()
    {
        return HexWidth();
    }

    public Vector3 PositionFromCamera( Vector3 cameraPosition, int numColumns, int numRows)
    {
        float mapHeight = numColumns * HexHorizontalSpacing();
        float mapWidth = numRows * HexHorizontalSpacing();

        Vector3 position = Position();

        float widthsFromCamera = (position.x - cameraPosition.x) / mapWidth;

        //widthsFromCamera to be between -0.5 to 0.5;
        if( Mathf.Abs(widthsFromCamera) <= 0.5f )
        {
            return position;
        }
        
        if (widthsFromCamera > 0)
        {
            widthsFromCamera += 0.5f;
        }
        else
        {
            widthsFromCamera -= 0.5f;
        }

        int widthsToFix = (int) widthsFromCamera;
        position.x -= widthsToFix * mapWidth;

        return position;

    }

    
    public static float Distance(Hex a, Hex b) {
        //TODO FIXME: WRAPPING
        return Mathf.Max(Mathf.Abs(a.Q - b.Q), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));
    }

}