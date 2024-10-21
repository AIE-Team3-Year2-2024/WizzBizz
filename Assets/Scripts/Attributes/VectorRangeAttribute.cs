using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false)]
public class VectorRangeAttribute : PropertyAttribute
{
    // TODO: Integer vector types.
    public Vector4 min4, max4;
    public Vector3 min3, max3;
    public Vector2 min2, max2;

    public VectorRangeAttribute(params float[] list)
    {
        if (list.Length > 1 && list.Length <= 8)
        {
            this.min2 = new Vector2(list[0], list[1]);
            this.max2 = new Vector2(list[2], list[3]);

            this.min3 = new Vector3(list[0], list[1], list[2]);
            this.max3 = new Vector3(list[3], list[4], list[5]);

            this.min4 = new Vector4(list[0], list[1], list[2], list[3]);
            this.max4 = new Vector4(list[5], list[5], list[6], list[7]);
        }
    }
}
