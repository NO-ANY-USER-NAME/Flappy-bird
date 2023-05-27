using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bird_Control : MonoBehaviour{
    public Rigidbody2D rb2D;
    public float upStrength,gravityStrength;
    public Transform gravitySource;
    [Header("Gravity Source")]
    public float sourceMinX;
    public float sourceMaxX;
    public float sourceSpeed;


    private float gravityStrengthtoSource;
    void Awake(){
        gravityStrengthtoSource=gravityStrength/5;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MoveGravitySource(){
        float xMove=Input.GetAxis("Horizontal")*sourceSpeed;
        if(xMove<0&&gravitySource.position.x+xMove>=sourceMinX){
            //Debug.Log("left");
            gravitySource.Translate(xMove,0,0);
        }
        else if(xMove>0&&gravitySource.position.x+xMove<=sourceMaxX){
            //Debug.Log("right");
            gravitySource.Translate(xMove,0,0);
        }
    }

    void FixedUpdate(){
        rb2D.AddForce((gravitySource.position-transform.position).normalized*gravityStrengthtoSource+Vector3.down*gravityStrength,ForceMode2D.Force);
        //rb2D.AddForce(Vector2.down*gravityStrength,ForceMode2D.Force);
    }

    void Update(){
        MoveGravitySource();
        if(Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)){
            rb2D.AddForce(Vector2.up*upStrength,ForceMode2D.Impulse);
        }
        if(transform.position.y>6){

        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("obstacle")){

        }
        else if(other.CompareTag("ground")){

        }
    }

}
