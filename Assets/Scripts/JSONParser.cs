using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    // Takes the Json TextAsset File and returns a string of the text.
    public string jsonParser(TextAsset filename){
        string json = filename.ToString();
        return json;
    }
}