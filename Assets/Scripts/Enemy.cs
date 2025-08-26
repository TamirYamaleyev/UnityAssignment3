using System.Collections;
using Mono.Cecil;
using UnityEngine;

public enum EnemyState
{
    None,
    Patrol,
    WaitingToShoot
}

public class Enemy : MonoBehaviour
{
    public AudioSource shootSFX;

    public float health = 100f;

    public GameObject target;
    public float speed = 1f;

    public GameObject projectile;
    public float bulletSpeed = 10f;

    public EnemyState state = EnemyState.None;

    public Transform patrolWaypoint1;
    public Transform patrolWaypoint2;
    private Transform patrolTarget;

    private Coroutine attackCoroutine;

    void Start()
    {
        shootSFX = GetComponent<AudioSource>();

        state = EnemyState.Patrol;
        patrolTarget = patrolWaypoint1;

        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                PatrolBetweenWaypoints();
                break;

            case EnemyState.WaitingToShoot:
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(AttackSequence());
                break;
        }
    }

    void PatrolBetweenWaypoints()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolTarget.position) < 0.1f)
        {
            patrolTarget = patrolTarget == patrolWaypoint1 ? patrolWaypoint2 : patrolWaypoint1;
        }
    }

    IEnumerator AttackSequence()
    {
        FacePlayer();

        Debug.Log("Taking Aim...");
        yield return new WaitForSeconds(2f);

        if (state == EnemyState.WaitingToShoot)
        {
            Debug.Log("Shooting 1 bullet!");
            Shoot();
        }
        else
        {
            EndAttack();
            yield break;
        }

        yield return new WaitForSeconds(2f);

        if (state == EnemyState.WaitingToShoot)
        {
            Debug.Log("Shooting 2 bullets!");
            Shoot();
            yield return new WaitForSeconds(0.5f);
            Shoot();
        }
        else
        {
            EndAttack();
            yield break;
        }

        yield return new WaitForSeconds(1f);
        EndAttack();
    }

    void EndAttack()
    {
        state = EnemyState.Patrol;
        attackCoroutine = null;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (shootSFX != null)
        {
            shootSFX.Play();
        }
    }

    void FacePlayer()
    {
        Vector3 currentRotation = transform.eulerAngles;

        if (target == null) return;

        if (transform.position.x > target.transform.position.x)
            currentRotation.y = 0f;
        else
            currentRotation.y = 180f;

        transform.eulerAngles = currentRotation;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerZone") && state == EnemyState.Patrol)
        {
            state = EnemyState.WaitingToShoot;
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackSequence());
        }
    }
}
