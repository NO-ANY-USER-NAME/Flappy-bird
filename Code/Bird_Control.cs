using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;


public class Bird_Control : MonoBehaviour{
    public static bool GameActive{get;private set;}

    public Rigidbody2D rb2D;
    public float upStrength,gravityStrength;
    public Transform gravitySource;
    [Header("Gravity Source")]
    public float sourceMinX;
    public float sourceMaxX;
    public float sourceSpeed;

    [Header("Live's Text")]
    public TMP_Text liveText;

    [Header("Obstacle Generator")]
    public Obstacles_Manager obstaclesManager;


    private int live;
    private Vector3 birdStartPosition,gravitySourceStartPosition;

    void Awake(){
        GameActive=false;
        Time.timeScale=0;
        live=5;
        liveText.text=live.ToString();
        birdStartPosition=transform.position;
        gravitySourceStartPosition=gravitySource.position;
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
        if(!GameActive){
            return;
        }
        rb2D.AddForce(Vector2.down*gravityStrength,ForceMode2D.Force);
        Vector2 pos=rb2D.transform.position;
        pos.x=gravitySource.position.x;
        rb2D.transform.position=pos;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameActive=!GameActive;
            Time.timeScale=GameActive?1:0;
        }
        if(!GameActive){
            return;
        }

        obstaclesManager.ObstacleUpdate();
        MoveGravitySource();
        if(Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)){
            rb2D.AddForce(Vector2.up*upStrength,ForceMode2D.Impulse);
        }
        if(transform.position.y>6){
            Debug.Log("too high");
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("obstacle")){
            Debug.Log("hit obstacle");
            live--;
            liveText.text=live.ToString();
            if(live<=0)RestartGame();
        }
        else if(other.CompareTag("ground")){
            Debug.Log("fall to ground");
            RestartGame();
        }
    }

    private void RestartGame(){
        GameActive=false;
        Time.timeScale=0;
        obstaclesManager.RestartGame();
        Debug.Log("Game Over, Press esc to start");

        live=5;
        liveText.text=live.ToString();
        transform.position=birdStartPosition;
        gravitySource.position=gravitySourceStartPosition;
        rb2D.velocity=Vector2.zero;
        rb2D.angularVelocity=0;
    }

}
