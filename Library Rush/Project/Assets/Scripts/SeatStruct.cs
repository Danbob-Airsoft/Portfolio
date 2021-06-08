using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatStruct : MonoBehaviour
{
    //public Transform Position;
    public bool Taken;
    public GameObject TakenBy;
    //Selectable tasks from this seat
    public List<GameObject> SeatTasks;

}
