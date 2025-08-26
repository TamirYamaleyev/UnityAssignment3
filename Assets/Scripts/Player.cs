using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public enum CharacterState
{
    None,
    Idle,
    Walk,
    JumpStart,
    JumpEnd,
    Hurt,
    Punch,
    Kick,
    Dead,
    TotallyDead
}

[Serializable]
class PlayerData
{
    public string name;
    public int score;
}

public class Player : MonoBehaviour
{
    private Animator animator;
    public float speed = 5.0f;
    public PolygonCollider2D walkableArea;

    public CharacterState state;

    public LayerMask obstacleMask;
    public Collider2D FeetArea;

    Vector3 deltaMove = Vector3.zero;

    private float horizontal = 0;

    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

       /*
        // Examples we made in class to save-load data
        
        score = ReadScoreFromPlayerPrefs();
        Debug.Log(score);
        
        SaveScoreWithPlayerPrefs();
        
        SaveDateToJson();

        PlayerData getNewData = ReadDataFromJson();
        Debug.Log(getNewData.name + "," + getNewData.score);
        */
    }

    int ReadScoreFromPlayerPrefs()
    {
        return PlayerPrefs.GetInt("PlayerScore", -1000);
    }

    void SaveScoreWithPlayerPrefs()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
    }

    void SaveDateToJson()
    {
        PlayerData data = new PlayerData();
        data.name = "Avihoo";
        data.score = 100000;

        string dataJson = JsonUtility.ToJson(data);
        File.WriteAllText("Origins.erez", dataJson);
    }

    PlayerData ReadDataFromJson()
    {
        string getFromFile = File.ReadAllText("Origins.erez");
        return JsonUtility.FromJson<PlayerData>(getFromFile);
    }

// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && state != CharacterState.JumpStart)
        {
            state = CharacterState.JumpStart;
            //   _animator.Play(_state.ToString());
            
            GetComponent<JumpAction>().JumpStart(horizontal);
        }

        if (state == CharacterState.JumpStart) return;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            state = CharacterState.Punch;

            score += 10;
            
           
            animator.SetTrigger(state.ToString());

        }
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            /*   PlayerPrefs.SetInt("PlayerScore", score);
               PlayerPrefs.Save(); */
        }

        if (IsAnimationRunning("Punch")) return;
            
         horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            deltaMove =  Vector3.right * (horizontal * speed * Time.deltaTime);
            if (CanMove(deltaMove))
                transform.position += deltaMove;
         
            transform.localScale = new Vector3(
                horizontal * Mathf.Abs(transform.localScale.x), 
                transform.localScale.y, 
                transform.localScale.z
                );
            
            state = CharacterState.Walk;

        }
        else
        {
            state = CharacterState.Idle;
        }
        
        
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0)
        {
            deltaMove = Vector3.up * vertical * speed * Time.deltaTime;
            if (CanMove(deltaMove))
                transform.position += deltaMove;

            state = CharacterState.Walk;
        }

        if (vertical != 0 || horizontal != 0)
        {
            if (walkableArea.OverlapPoint(transform.position) == false)
            {
                Vector2 newPostion = walkableArea.ClosestPoint(transform.position);
                transform.position = new Vector3(newPostion.x, newPostion.y, transform.position.z);
            }
        }
        
        animator.Play(state.ToString());


    }

    bool IsAnimationRunning(string animationNAme)
    {
       AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
       if (currentStateInfo.IsName(animationNAme))
       {
           if (currentStateInfo.normalizedTime < 0.95f)
           {
               return true;
           }
       }

       return false;
    }

    bool CanMove(Vector3 deltaMove)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = obstacleMask;
        filter.useLayerMask = true;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        
        Vector2 direction = new Vector2(deltaMove.x, deltaMove.y);
        int hitCount = FeetArea.Cast(direction, filter, hits, direction.magnitude + 0.1f);
        return hitCount == 0;
    }

    public void PlayAttackSFX(AudioClip clip)
    {
        AudioManager.Instance.PlaySFXOneShot(clip);
      
        // Examples to try
        //AudioManager.Instance.PlaySFXCustom(clip, 0.2f, 0.5f);
        //AudioManager.Instance.PlaySFXCustom(clip, 1, -3.0f);
    }

    public void JumpEnd()
    {
        state = CharacterState.None;
        //   _animator.SetTrigger(_state.ToString());
    }
}
