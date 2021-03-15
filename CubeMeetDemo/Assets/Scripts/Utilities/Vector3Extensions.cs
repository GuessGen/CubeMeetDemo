using UnityEngine;

public static class Vector3Extensions { 

    public static Vector3 GetAbsRounded(this Vector3 input)
    {
        return  new Vector3(
               Mathf.Round(Mathf.Abs(input.x)),
               Mathf.Round(Mathf.Abs(input.y)),
               Mathf.Round(Mathf.Abs(input.z))
           );
    }

}



