using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DamageTable : MonoBehaviour
{
    private Dictionary<string,int> damageTable = new Dictionary<string, int>();
    [Serializable]
    public struct DamageMapping{
        public string tag;
        public int damage;
    }

    [SerializeField] DamageMapping[] mappings;

    // Start is called before the first frame update
    void Start()
    {
        //print("DT start");
        //damageTable[""];
        foreach (DamageMapping m in mappings)
        {
            
            damageTable[m.tag] = m.damage;
            //print(damageTable[m.tag]);
        }
    }

    public int GetDamage(string tag){
        //print("Damage lookup: "+ tag);
        int val = 0;
        if (!damageTable.TryGetValue(tag, out val)){
            return 0;
        } else{
            return val;
        }
    }
}
