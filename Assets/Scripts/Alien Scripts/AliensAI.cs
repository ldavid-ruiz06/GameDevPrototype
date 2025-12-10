using UnityEngine;
using UnityEngine.AI;

public class AliensAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Transform playerTransform;
    public LayerMask whatIsGround, playerLayer;
    public GameObject projectile;

    public float walkPointRange = 10f;
    public float timeBetweenAttacks = 2f;
    public float shootingOffset = 2f;        // cuánto adelante sale la bala
    public float sightRange = 15f;
    public float attackRange = 8f;
    public bool stoleSomething = false;
    public GameObject posReference;

    private bool playerInSightRange, playerInAttackRange;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private float nextPatrolSearch = 0f;
    private bool inContactAttack = false;
    


    // finite state machine 
    public enum State
    {
      Idle, Patrolling, Escaping, ChasingPlayer, StealingPlayer  
    };

    public State alienState;

    private void Awake()
    {
        
        player = GameObject.Find("Player");
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.stoppingDistance = 2f;   // se acerca mucho pero no se pega
            agent.updateRotation = true;   // gira automaticamente
            agent.autoBraking = false;     // no frena entre waypoints
        }

        // Aviso si no encuentra al jugador al iniciar
        if (player == null) Debug.LogError("No se encontro Player1");

        // iniciar state machine
        alienState = State.Patrolling;
    }

    private void Update()
    {
        // Salida rapida si falta algo o esta huyendo tras robar un body part
        if (player == null || agent == null || inContactAttack) return;


        // state maching
        switch (alienState)
        {
            //case State.Idle: Idle(); break;
            case State.Patrolling: Patrolling(); break;
            case State.Escaping: Escape(); break;
            case State.ChasingPlayer: ChasePlayer(); break;
            case State.StealingPlayer: StealPlayer(); break;
        }

        // // Detecta al jugador por LayerMask
        // playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // // Maquina de estados
        // if (!playerInSightRange && !playerInAttackRange)
        //     Patrolling();
        // else if (playerInSightRange && !playerInAttackRange)
        //     ChasePlayer();
        // else
        //     AttackPlayer();
    }

    void checkForPlayer()
    {
        // checar si esta al alcance del jugador
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        if(!playerInSightRange)
        {
            print("No veo al player. Patrolling.");
            alienState = State.Patrolling;
            return;
        }
        
        print("Veo el player!");

        // chechar si el jugador puede verlo
        bool playerCanSeeThem = player.GetComponent<Player>().inPlayerSight(gameObject);
        if(playerCanSeeThem || stoleSomething)
        {
            print("El player me ve! Huyendo.");
            alienState = State.Escaping;
            return;
        }
        else if(!playerCanSeeThem)
        {
            print("El player no me ve! Atacando.");
            alienState = State.ChasingPlayer;
            return;
        }
    }

    private void Idle()
    {
        checkForPlayer();
    }

    void Escape()
    {
        // Calcula direccion opuesta al jugador
        Vector3 dirHuir = (transform.position - playerTransform.position).normalized * 40f;

        // Encuentra punto valido en NavMesh para huir
        NavMesh.SamplePosition(transform.position + dirHuir, out NavMeshHit hit, 40f, NavMesh.AllAreas);
        agent.SetDestination(hit.position);

        // Vuelve normal en 8 segundos
        Invoke(nameof(ResetContact), 8f);
    }

    void StealPlayer()
    {
        print("Robando jugador!");

        inContactAttack = true;        // Bloquea mas robos hasta que termine de huir
        Debug.Log("ALIEN TOCA AL JUGADOR!");

        // Array con nombres exactos de cosas robables
        string[] robables = {"Arm", "Legs", "Head"};
        string elegido = robables[Random.Range(0, robables.Length - 1)];

        GameObject robado = GameObject.Find(elegido); // player ya es la referencia al Player1

        if (robado != null)
        {
            stoleSomething = true;
            // desactiva el objecto
            player.GetComponent<PlayerBodyManager>().StealBody(elegido, this.gameObject);
        }
        else
        {
            Debug.Log($"No encontró '{elegido}'");
        }

        alienState = State.Escaping;
        Escape();
        return;
    }

    private void Patrolling()
    {
        // checar el jugador
        checkForPlayer();
        // si se detecto el jugador, salir de esta accion
        if(alienState != State.Patrolling) return;


        // Si no tiene punto de patrulla, busca uno nuevo
        if (!walkPointSet) SearchWalkPoint();

        // Si ya tiene punto, va hacia el
        if (walkPointSet)
        {
            // Cuando ya llego (o esta muy cerca), busca otro punto
            agent.SetDestination(walkPoint);
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // Evita buscar puntos cada frame
        if (Time.time < nextPatrolSearch) return;
        nextPatrolSearch = Time.time + 0.5f;

        // Metodo 1: genera punto random y lo fuerza al NavMesh mas cercano
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * walkPointRange;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, walkPointRange, 1))
        {
            walkPoint = hit.position;
            walkPointSet = true;
            return;
        }

        // Metodo 2 (fallback): genera punto random y baja rayo para ver si hay suelo
        float rz = Random.Range(-walkPointRange, walkPointRange);
        float rx = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + rx, transform.position.y + 2f, transform.position.z + rz);
        if (Physics.Raycast(walkPoint, Vector3.down, 5f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        checkForPlayer();

        if(alienState != State.ChasingPlayer) return;

        // Cada frame le dice al agente que vaya a donde esta el jugador
        if(alienState == State.ChasingPlayer) agent.SetDestination(playerTransform.position);

        // check si el jugador esta en alcance
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance < attackRange) alienState = State.StealingPlayer;
    }

    private void AttackPlayer()
    {
        // Sigue persiguiendo mientras dispara
        agent.SetDestination(playerTransform.position);

        // Gira el cuerpo hacia el jugador, pero solo en el eje Y (evita que se incline si el jugador esta arriba/abajo
        Vector3 lookDir = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(lookDir);

        // Si ya puede disparar de nuevo
        if (!alreadyAttacked)
        {
            // Posicion de la bala un poco delante y arriba para que no choque con el collider del alien
            Vector3 spawnPos = transform.position + transform.forward * shootingOffset + Vector3.up * 1f;
            GameObject proj = Instantiate(projectile, spawnPos, Quaternion.LookRotation(playerTransform.position - spawnPos));

            // Ignora colisión con el propio alien
            Collider alienCol = GetComponent<Collider>();
            Collider bulletCol = proj.GetComponent<Collider>();
            if (alienCol != null && bulletCol != null)
                Physics.IgnoreCollision(alienCol, bulletCol, true);

            // Cooldown del disparo
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // CONTACTO (robar parte + huir)
//     private void OnTriggerEnter(Collider other)
// {
//     // Detecta solo por LAYER (no importa tag ni nombre)
//     if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !inContactAttack)
//     {
//         inContactAttack = true;        // Bloquea mas robos hasta que termine de huir
//         Debug.Log("ALIEN TOCA AL JUGADOR!");

//         // Array con nombres exactos de cosas robables
//         string[] robables = { "Arm","Legs", "Head"};    
//         string elegido = robables[Random.Range(0, robables.Length)];

//         GameObject robado = GameObject.Find(elegido); // player ya es la referencia al Player1

//         if (robado != null)
//         {
//             Debug.Log($"¡ROBADO! → {elegido}");
//             //robado.gameObject.SetActive(false);
//         }
//         else
//         {
//             Debug.Log($"No encontró '{elegido}'");
//         }

//         // // Calcula direccion opuesta al jugador
//         // Vector3 dirHuir = (transform.position - playerTransform.position).normalized * 40f;
        
//         // // Encuentra punto valido en NavMesh para huir
//         // NavMesh.SamplePosition(transform.position + dirHuir, out NavMeshHit hit, 40f, NavMesh.AllAreas);
//         // agent.SetDestination(hit.position);

//         // // Vuelve normal en 8 segundos
//         // Invoke(nameof(ResetContact), 8f);

//         // after succesfully stealing from the player, run away
//         alienState = State.Escaping;
//         return;
//     }
// }

    private void ResetContact()
    {
        // Termina el estado de huida, vuelve a patrullar/normal
        inContactAttack = false;
        alienState = State.Patrolling;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja rangos del sight range y el attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
