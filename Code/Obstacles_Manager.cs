using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class Obstacles_Manager:MonoBehaviour{
    public const int MaximumObstacle=16;
    public const int MaximumObstacleOnField=5;
    public const float SpawnInterval=1f;

    public Transform Folder;
    public Sprite obstacleSquareSprite;

    public class Obstacle{
        public Transform top,bottom;
        public Rigidbody2D topRB,bottomRB;
        public readonly int id;

        public Obstacle(int _id,Sprite sprite,Transform folder){
            id=_id;
            top=CreateNewObstacle("top obstacle",sprite,folder);
            bottom=CreateNewObstacle("bottom obstacle",sprite,folder);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Transform CreateNewObstacle(in string name,Sprite sprite,Transform folder){
            Rigidbody2D RB;
            BoxCollider2D collider;
            SpriteRenderer renderer;
            GameObject GO;
            GO=new GameObject(name);
            GO.SetActive(false);
            renderer=GO.AddComponent<SpriteRenderer>();
            renderer.sprite=sprite;
            renderer.color=Color.red;//change this line if i have sprite for obstacle
            collider=GO.AddComponent<BoxCollider2D>();
            GO.transform.parent=folder;
            RB=GO.AddComponent<Rigidbody2D>();
            RB.velocity=Vector2.zero;
            RB.rotation=0;
            RB.angularVelocity=0;
            RB.constraints=RigidbodyConstraints2D.FreezeRotation|RigidbodyConstraints2D.FreezePositionY;
            return GO.transform;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartMove(float _speed){
            Vector2 speed=Vector2.right*_speed;
            topRB.velocity=speed;
            bottomRB.velocity=speed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetX(){
            return top.position.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetActive(bool active){
            bottom.gameObject.SetActive(active);
            top.gameObject.SetActive(active);
            topRB.velocity=Vector2.zero;
            bottomRB.velocity=Vector2.zero;
            topRB.angularVelocity=0;
            bottomRB.angularVelocity=0;
        }
    }

    public Obstacle[] obstacles;
    public int[] obstacleIDtoIndex;
    public int obstacleCount;

    public float screenRangeX;
    public float spawnSmallerThanX;

    private int failCounter;
    private float time;

    void Awake(){
        obstacles=new Obstacle[MaximumObstacle];
        int i;
        for(i=0;i<MaximumObstacle;i++){
            obstacles[i]=new Obstacle(i,obstacleSquareSprite,transform);
        }
        obstacleIDtoIndex=new int[MaximumObstacle];
        obstacleCount=0;

        failCounter=0;
        time=0;
    }

    void Update(){
        if(obstacleCount>=MaximumObstacleOnField)return;

        float rightestX=float.MinValue;
        for(int i=0;i<obstacleCount;){
            if(obstacles[i].GetX()<-screenRangeX){
                obstacleCount--;
                obstacles[i].SetActive(false);
                obstacles[i]=obstacles[obstacleCount];
            }
            else{
                i++;
            }
        }

        time+=Time.deltaTime;
        if(time<SpawnInterval)return;
        time-=SpawnInterval;
        if(rightestX>spawnSmallerThanX)return;

        if(failCounter>=3||Random.Range(0,10)>3){//60% spawn a new obstacle or fail to spawn obstacle 3 times
        
            failCounter=0;//restet counter
        }
        else{
            failCounter++;
        }
    }

}
