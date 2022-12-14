using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TankDatabase : ScriptableObject{
    
    public Tanks[] tank;
    public int TankCount{
        get{
            return tank.Length; 
        }
    }
    public Tanks GetTank(int index ){
        return tank[index];
    }
}
