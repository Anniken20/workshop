using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrappleable{
    void Grappled(bool active, GameObject hitObject, Transform startPos);
}