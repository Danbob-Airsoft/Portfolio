using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FriendlyMarker : MonoBehaviour
{
    private GameObject Player;
    private Camera PlayerCam;
    private Vector3 VectToPlayer;

    private Vector3 MarkerPosition;
    public GameObject UIMarkerPrefab;
    public Image MyMarker;

    public float MaxDistance;
    public float MinDistance;

    private FighterHealth ThisHealth;

    public bool MarkerActive;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "ControlTestScene")
        {
            Destroy(this);
        }

        Player = GameObject.FindGameObjectWithTag("Player");

        ThisHealth = this.GetComponent<FighterHealth>();
        MyMarker = Instantiate(UIMarkerPrefab, GameObject.Find("Canvas(FriendlyMarkers)").transform).GetComponent<Image>();
        MyMarker.enabled = false;
    }

    private void Look()
    {
        if (MarkerActive)
        {
            if (PlayerCam == null)
            {
                return;
            }

            if (MyMarker == null)
            {
                return;
            }

            if (VectToPlayer.magnitude > MinDistance && VectToPlayer.magnitude < MaxDistance)
            {
                Vector3 MyPosition = PlayerCam.WorldToViewportPoint(transform.position);
                if (MyPosition.z > 0.0f)
                {
                    if (MyPosition.x > 0.0f && MyPosition.x < 1.0f)
                    {
                        if (MyPosition.y > 0.0f && MyPosition.y < 1.0f)
                        {
                            if (!MyMarker.enabled)
                            {
                                MyMarker.enabled = true;
                            }
                            return;
                        }
                    }
                }
            }
            if (MyMarker.enabled)
            {
                MyMarker.enabled = false;
            }
        }
    }

    void Update()
    {
        PlayerCam = Player.GetComponent<PlayerMovement>().ActiveCamera;
        VectToPlayer = Player.transform.position - this.transform.position;

        Look();

        if (PlayerCam && MyMarker)
        {
            //Update Position
            float DistToPlayer = VectToPlayer.magnitude;
            //Set Marker to above player
            MarkerPosition = this.transform.position + (this.transform.up * 5);
            //Turn location into screen point and round
            MarkerPosition = PlayerCam.WorldToScreenPoint(MarkerPosition);
            MarkerPosition.x = Mathf.Round(MarkerPosition.x);
            MarkerPosition.y = Mathf.Round(MarkerPosition.y);
            //Set Position
            MyMarker.transform.position = new Vector3(MarkerPosition.x, MarkerPosition.y, 0);
            MyMarker.GetComponent<Image>().SetAllDirty();

            //Update Opacity
            float Opacity = ((DistToPlayer - MinDistance) / MaxDistance * 0.85f);
            MyMarker.color = new Color(MyMarker.color.r, MyMarker.color.g, MyMarker.color.b, Opacity);

            //Update Fill for health
            /*float FillAmount = ThisHealth.ShipHealth / ThisHealth.MaxHealth;
            MyMarker.fillAmount = FillAmount;*/

            //Update Scale
            if(Opacity > 0.25f)
            {
                Opacity = 0.25f;
            }
            else
            {
                Opacity = (float)System.Math.Round(Opacity, 2);
            }
            MyMarker.gameObject.transform.localScale = new Vector3(Opacity, Opacity, Opacity);
        }
    }
}
