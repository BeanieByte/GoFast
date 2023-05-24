using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScript : MonoBehaviour
{

    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
    }
}

