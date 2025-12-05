using UnityEngine;
using UnityEngine.AI;

public class AliensAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;

    public float walkPointRange = 10f;
    public float timeBetweenAttacks = 2f;
    public float shootingOffset = 2f;        // cuánto adelante sale la bala
    public float sightRange = 15f;
    public float attackRange = 8f;

    private bool playerInSightRange, playerInAttackRange;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private float nextPatrolSearch = 0f;
    private bool inContactAttack = false;

    private void Awake()
    {
        
        player = GameObject.Find("Player1")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.stoppingDistance = 2f;   // se acerca mucho pero no se pega
            agent.updateRotation = true;   // gira automaticamente
            agent.autoBraking = false;     // no frena entre waypoints
        }

        // Aviso si no encuentra al jugador al iniciar
        if (player == null) Debug.LogError("No se encontro Player1");
    }

    private void Update()
    {
        // Salida rapida si falta algo o esta huyendo tras robar un body part
        if (player == null || agent == null || inContactAttack) return;

        // Detecta al jugador por LayerMask
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Maquina de estados
        if (!playerInSightRange && !playerInAttackRange)
            Patrolling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else
            AttackPlayer();
    }

    private void Patrolling()
    {
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
        // Cada frame le dice al agente que vaya a donde esta el jugador
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Sigue persiguiendo mientras dispara
        agent.SetDestination(player.position);

        // Gira el cuerpo hacia el jugador, pero solo en el eje Y (evita que se incline si el jugador esta arriba/abajo
        Vector3 lookDir = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookDir);

        // Si ya puede disparar de nuevo
        if (!alreadyAttacked)
        {
            // Posicion de la bala un poco delante y arriba para que no choque con el collider del alien
            Vector3 spawnPos = transform.position + transform.forward * shootingOffset + Vector3.up * 1f;
            GameObject proj = Instantiate(projectile, spawnPos, Quaternion.LookRotation(player.position - spawnPos));

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
    private void OnTriggerEnter(Collider other)
{
    // Detecta solo por LAYER (no importa tag ni nombre)
    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !inContactAttack)
    {
        inContactAttack = true;        // Bloquea mas robos hasta que termine de huir
        Debug.Log("ALIEN TOCA AL JUGADOR!");

        // Array con nombres exactos de cosas robables
        string[] robables = { "Arm1","AssaultRifle" };    
        string elegido = robables[Random.Range(0, robables.Length)];

        Transform robado = player.Find(elegido); // player ya es la referencia al Player1

        if (robado != null)
        {
            Debug.Log($"¡ROBADO! → {elegido}");
            robado.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"No encontró '{elegido}'");
        }

        // Calcula direccion opuesta al jugador
        Vector3 dirHuir = (transform.position - player.position).normalized * 40f;
        
        // Encuentra punto valido en NavMesh para huir
        NavMesh.SamplePosition(transform.position + dirHuir, out NavMeshHit hit, 40f, NavMesh.AllAreas);
        agent.SetDestination(hit.position);

        // Vuelve normal en 8 segundos
        Invoke(nameof(ResetContact), 8f);
    }
}

    private void ResetContact()
    {
        // Termina el estado de huida, vuelve a patrullar/normal
        inContactAttack = false;
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
