using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class Obstacles_Manager:MonoBehaviour{
    public const int MaximumObstacle=8;
    public const int MaximumObstacleOnField=5;
    public const float SpawnInterval=1f;

    public const float GapMinY=2f,GapMaxY=5f;
    public const float GapBaseY=-4.5f,GapHeightLimitY=4.5f;
    public const float GapMinWidthX=1.5f,GapMaxWidthX=2.5f;
    public const float ObstacleMinSpeed=3f,ObstacleMaxSpeed=5.5f;

    [Header("Obstacle")]
    public Sprite obstacleSquareSprite;

    [Header("Spawn")]
    [Range(5,15)]public float screenRangeX;
    [Range(-12,12)]public float spawnSmallerThanX;

    private class Obstacle{
        public Transform top,bottom;
        public Rigidbody2D topRB,bottomRB;

        public Obstacle(Sprite sprite,Transform folder){
            top=CreateNewObstacle("top obstacle",sprite,folder,out topRB);
            bottom=CreateNewObstacle("bottom obstacle",sprite,folder,out bottomRB);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Transform CreateNewObstacle(in string name,Sprite sprite,Transform folder,out Rigidbody2D RB){
            BoxCollider2D collider;
            SpriteRenderer renderer;
            GameObject GO;
            GO=new GameObject(name);
            GO.SetActive(false);
            GO.layer=6;//hard code value
            GO.tag="obstacle";
            renderer=GO.AddComponent<SpriteRenderer>();
            renderer.sprite=sprite;
            renderer.color=Color.red;//change this line if i have sprite for obstacle
            renderer.sortingLayerName="obstacle";
            collider=GO.AddComponent<BoxCollider2D>();
            GO.transform.parent=folder;
            RB=GO.AddComponent<Rigidbody2D>();
            RB.velocity=Vector2.zero;
            RB.rotation=0;
            RB.angularVelocity=0;
            RB.constraints=RigidbodyConstraints2D.FreezeRotation|RigidbodyConstraints2D.FreezePositionY;
            RB.gravityScale=0;
            return GO.transform;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartMove(float _speed){
            top.gameObject.SetActive(true);
            bottom.gameObject.SetActive(true);
            Vector2 speed=-Vector2.right*_speed;
            topRB.velocity=speed;
            bottomRB.velocity=speed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetX(){
            return top.position.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUnactive(){
            bottom.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
            topRB.velocity=Vector2.zero;
            bottomRB.velocity=Vector2.zero;
            topRB.angularVelocity=0;
            bottomRB.angularVelocity=0;
        }
    }
    private Obstacle[] obstacles;
    private int obstacleCount;

    private int failCounter;
    private float time;

    void Awake(){
        obstacles=new Obstacle[MaximumObstacle];
        int i;
        for(i=0;i<MaximumObstacle;i++){
            obstacles[i]=new Obstacle(obstacleSquareSprite,transform);
        }
        obstacleCount=0;

        failCounter=0;
        time=0;
    }

    void Update(){
        float rightestX=float.MinValue;
        for(int i=0;i<obstacleCount;){
            float x=obstacles[i].GetX();
            rightestX=x>rightestX?x:rightestX;
            if(x<-screenRangeX){
                obstacleCount--;

                Obstacle obstacle=obstacles[i];
                obstacle.SetUnactive();
                obstacles[i]=obstacles[obstacleCount];
                obstacles[obstacleCount]=obstacle;
            }
            else{
                i++;
            }
        }

        time+=Time.deltaTime;
        if(time<SpawnInterval)return;
        time-=SpawnInterval;
        if(rightestX>spawnSmallerThanX)return;

        if(obstacleCount<MaximumObstacleOnField&&(failCounter>=3||Random.Range(0,10)>2)){//70% spawn a new obstacle or fail to spawn obstacle 3 times
            float gapWidth=Random.Range(GapMinWidthX,GapMaxWidthX);
            float gapHeight=Random.Range(GapMinY,GapMaxY);
            float gapBottom=Random.Range(GapBaseY,GapHeightLimitY-gapHeight);
            
            float tem;

            tem=gapBottom+6;
            obstacles[obstacleCount].bottom.localScale=new Vector2(gapWidth,tem);
            obstacles[obstacleCount].bottom.position=new Vector2(screenRangeX,(gapBottom-6)/2);

            tem=6-(gapBottom+gapHeight);
            obstacles[obstacleCount].top.localScale=new Vector2(gapWidth,tem);
            obstacles[obstacleCount].top.position=new Vector2(screenRangeX,(tem/2)+gapBottom+gapHeight);

            obstacles[obstacleCount].StartMove(Random.Range(ObstacleMinSpeed,ObstacleMaxSpeed));
            obstacleCount++;

            failCounter=0;//restet counter
        }
        else{
            failCounter++;
        }
    }

}
