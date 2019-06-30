using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;

    [HideInInspector]
    public float step_length = 0.4f;

    [HideInInspector]
    public float movement_frequency = 0.1f;

    private float counter;
    private bool move;
    bool create_node_at_trail = false;

    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> delta_position;
    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;


    [SerializeField]
    private GameObject DeathUI;

    private bool create_node_at_tail;

    private void Awake()
    {
        tr = transform;
        main_Body = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        delta_position = new List<Vector3>()
        {
            new Vector3(-step_length*2f,0f),   //LEFT
            new Vector3(0f,step_length*2f),    //UP
            new Vector3(step_length*2f,0f),    //RIGHT
            new Vector3(0f,-step_length*2f),   //DOWN

        };
    }

    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body = nodes[0];
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer()
    {
        SetDirectionRandom();

        switch(direction)
        {
            case PlayerDirection.RIGHT:

                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE* 2f, 0f, 0f);

                break;

            case PlayerDirection.LEFT:

                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.UP:

                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

            case PlayerDirection.DOWN:

                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

        }
    }

    void Move()
    {
        Vector3 dPosition = delta_position[(int)direction];
        Vector3 parentPosition = head_Body.position;
        Vector3 previousPosition;

        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;

        for (int i = 1; i <nodes.Count; i++)
        {
            previousPosition = nodes[i].position ;
            nodes[i].position = parentPosition;
            parentPosition = previousPosition;
        }

        if(create_node_at_tail)
        {
            create_node_at_tail = false;

            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    private void Update()
    {
        CheckMovementFrequency();
    }

    private void FixedUpdate()
    {
        if(move)
        {
            move = false;
            Move();
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;
        if(counter>=movement_frequency)
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
        {
            return;
        }

        direction = dir;

        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if(target.tag==Tags.FRUIT)
        {
            target.gameObject.SetActive(false);
            create_node_at_tail = true;

            GameplayController.instance.IncreaseScore();
            AudioManager.instance.Play_Pickup_Sound();
        }

        if (target.tag == Tags.FRUIT_2)
        {
            target.gameObject.SetActive(false);
            create_node_at_tail = true;

            GameplayController.instance.IncreaseScore_2();
            AudioManager.instance.Play_Pickup_Sound();
        }


        if (target.tag==Tags.WALL || target.tag==Tags.BOMB||target.tag==Tags.TAIL)
        {
            print("Touched the Wall");
            Time.timeScale = 0f;
            AudioManager.instance.Play_Dead_Sound();
            DeathUI.SetActive(true);
        }
    }
}
