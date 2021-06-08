using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GuestSpawner : MonoBehaviour
{

    //Level difficulty
    public float LevelDifficulty;

    public GameObject GuestPrefab;
    //Min and Max delay between waves
    public float MinDelay;
    public float MaxDelay;
    //Min and Max number of guests per wave
    public int MinGuestNumber;
    public int MaxGuestNumber;
    //Entrance position
    public Transform SpawnPosition;

    //Parent object for guests
    public GameObject GuestStorageOBJ;
    //Next spawn wave time
    private float NextSpawnTime;

    //Seating variables
    public List<Transform> SeatingPositions;
    [HideInInspector] public int NumberOfSeatsRemaining;

    //Level complete checker
    private LevelCompleteScript CompleteChecker;

    //Head Sprites for guests
    public List<Sprite> GuestHeads;

    // Start is called before the first frame update
    void Start()
    {
        //Set variables
        CompleteChecker = this.GetComponent<LevelCompleteScript>();
        NextSpawnTime = Time.time + Random.Range(5f, 10f);

        //Generate seats from positions
        if(SeatingPositions.Count > 0)
        {
            NumberOfSeatsRemaining = SeatingPositions.Count;
        }
        else
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If time to spawn next wave of guests and player still has books to return
        if(Time.time >= NextSpawnTime && CompleteChecker.AllBooksReturned == false)
        {
            //Reset timer
            NextSpawnTime = Time.time + Random.Range(MinDelay, MaxDelay);

            //Select Random Number of guests to spawn
            int NumberOfGuestsToSpawn = Random.Range(MinGuestNumber, MaxGuestNumber);

            //Start spawning guests
            StartCoroutine(SpawnGuests(NumberOfGuestsToSpawn));
        }
    }

    private IEnumerator SpawnGuests(int NumberOfGuestsToSpawn)
    {
        //Loop through spawn code for number selected
        for (int i = 0; i < NumberOfGuestsToSpawn; i++)
        {
            //If Seating is available for the guest
            if (NumberOfSeatsRemaining > 0)
            {
                //Spawn Guest
                GameObject SpawnedGuest = Instantiate(GuestPrefab, SpawnPosition.position, this.transform.rotation);

                //Set Head Sprite
                SpawnedGuest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GuestHeads[Random.Range(0, GuestHeads.Count - 1)];

                //Set Object Parent
                SpawnedGuest.transform.SetParent(GuestStorageOBJ.transform);

                //Pass Guest Difficulty
                SpawnedGuest.GetComponent<GuestTaskScript>().DifficultyLevel = LevelDifficulty;
                SpawnedGuest.GetComponent<GuestTaskScript>().SpawnPosition = SpawnPosition;

                NumberOfSeatsRemaining--;

                //Set Seat Target to untaken seat
                for (int s = 0; s < SeatingPositions.Count; s++)
                {
                    GameObject CurrentSeat = SeatingPositions[s].gameObject;
                    SeatStruct SeatData = CurrentSeat.GetComponent<SeatStruct>();
                    //SeatStruct CurrentSeat = Seating[s];
                    if (!SeatData.Taken)
                    {
                        //Set Seat To Taken
                        SeatData.Taken = true;
                        SeatData.TakenBy = SpawnedGuest;
                        SpawnedGuest.GetComponent<GuestTaskScript>().OccupiedSeat = CurrentSeat;

                        //Set Guest AI Target
                        SpawnedGuest.GetComponent<Pathfinding.AIDestinationSetter>().target = CurrentSeat.transform;

                        //Stop Looping
                        break;
                    }
                }

                yield return new WaitForSeconds(0.6f);
            }
            //Else no seats available, stop trying to spawn Guests for this loop
            else
            {
                break;
            }
        }

    }
}
