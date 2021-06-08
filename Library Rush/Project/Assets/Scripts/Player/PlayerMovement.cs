using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Get Player Rigidbody for movement
    private Rigidbody2D PlayerBody;
    //Move Speed
    public float MoveSpeed;
    public float RotationSpeed;
    private Vector3 MousePos;

    private GameObject Sprite;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = this.GetComponent<Rigidbody2D>();
        Sprite = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float MoveDirection = 0;

        //Move Up / Down
        if (Input.GetAxis("Vertical") != 0 && Vector2.Distance(MousePos, this.transform.position) > 0.1f)
        {
            MoveDirection = Input.GetAxis("Vertical");

            //Add Force
            PlayerBody.AddForce(this.transform.up * (MoveDirection * MoveSpeed), ForceMode2D.Force);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetComponent<MenuScript>().PauseGame();
        }

        //Point at Mouse
        if(Time.timeScale != 0)
        {
            MousePos = Input.mousePosition;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);

            if(Vector2.Distance(MousePos, this.transform.position) > 0.1f)
            {
                Vector2 Direction = new Vector2(MousePos.x - this.transform.position.x, MousePos.y - this.transform.position.y);

                this.transform.up = Direction;
            }
        }
    }
}
