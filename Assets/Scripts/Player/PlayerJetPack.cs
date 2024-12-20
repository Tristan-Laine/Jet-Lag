using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ThrustEvent : UnityEvent<float, float> { }

public class PlayerJetPack : MonoBehaviour
{
    public ThrustEvent reservoirThrustChanged;
    public ThrustEvent thrustForceChanged;

    private Rigidbody rb;

    public float smallThrustForce = 1f;
    public float gasConsumRateSmallThrust = 1f;

    public float thrustForce = 0f;
    public float thrustForceMax = 10f;
    private float thrustForceTimer = 0f;
    public float thrustForceTimerMax = 1f;
    public float gasConsumRateThrustForce = 2f;

    public float reservoir = 10f;
    public float reservoirMax = 10f;


    public float gasRefillRate = 3f;
    public float gasRefillDelay = 3f;
    private float timeSinceLastGasUse = 0f;

    private bool isCharging = false;
    private bool isCancelingCharge = false;
    private bool isReleasingThrust = false;

    public float thrustReleaseTime = 1f;
    private float thrustReleaseTimer = 0f;

    public float cooldownTime = 0.2f;
    private float cooldownTimer = 0f;

    public ParticleSystem smallThrustParticles;
    public ParticleSystem bigThrustParticles;

    public AudioJetPack audioJetPack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeReservoirThrustChanged();
        InvokeThrustForceChanged();
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.fixedDeltaTime; 

        bool leftPressed = Input.GetMouseButton(0);
        bool rightPressed = Input.GetMouseButton(1);
        bool leftReleased = Input.GetMouseButtonUp(0);
        bool rightReleased = Input.GetMouseButtonUp(1);

        if (leftPressed && rightPressed && cooldownTimer <= 0)
        {
            if (isCancelingCharge)
            {
                isCharging = true;
                isCancelingCharge = false;
            }
            else if (!isCharging)
            {
                isCharging = true;
            }
            StartCharging();

            if (smallThrustParticles.isPlaying){smallThrustParticles.Stop();}
            if (bigThrustParticles.isPlaying){bigThrustParticles.Stop();}

            audioJetPack.StopSmallThrustSound();
            audioJetPack.StopBigThrustSound();
        }
        else if (isCharging && !leftPressed)
        {
            isCharging = false;
            isCancelingCharge = true;

            if (smallThrustParticles.isPlaying){smallThrustParticles.Stop();}
            if (bigThrustParticles.isPlaying){bigThrustParticles.Stop();}

            audioJetPack.StopSmallThrustSound();
            audioJetPack.StopBigThrustSound();
        }
        else if ((isCharging || isCancelingCharge) && !rightPressed)
        {
            isCharging = false;
            isCancelingCharge = false;
            isReleasingThrust = true;
            thrustReleaseTimer = 0f;
        }
        else if (leftPressed && !rightPressed  && !isReleasingThrust)
        {
            SmallThrust();
            audioJetPack.StopBigThrustSound();
        }
        else if (isCancelingCharge)
        {
            CancelChargingOverTime();
        }
        else if (isReleasingThrust)
        {
            ReleaseThrustOverTime();
            cooldownTimer = cooldownTime;

        }
        else
        {
            GasRefillOverTime();

            if (smallThrustParticles.isPlaying){smallThrustParticles.Stop();}
            if (bigThrustParticles.isPlaying){bigThrustParticles.Stop();}

            audioJetPack.StopSmallThrustSound();
            audioJetPack.StopBigThrustSound();
        }

    }

    //* Petite propultion
    void SmallThrust()
    {
        if (!isCharging && reservoir > 0f)
        {
            ForceModeForce(smallThrustForce);
            GasConsumption(gasConsumRateSmallThrust);
                        
            if (!smallThrustParticles.isPlaying){smallThrustParticles.Play();}
            audioJetPack.PlaySmallThrustSound();
        }
        else
        {
            if (smallThrustParticles.isPlaying){smallThrustParticles.Stop();}
            audioJetPack.StopSmallThrustSound();
        }
    }


    //* Gestion du boost
    void StartCharging()
    {
        if (reservoir > 0f && thrustForceTimer < thrustForceTimerMax)
        {
            thrustForceTimer += Time.fixedDeltaTime;
            thrustForceTimer = Mathf.Clamp(thrustForceTimer, 0f, thrustForceTimerMax);

            thrustForce = thrustForceMax * (thrustForceTimer / thrustForceTimerMax);

            GasConsumption(gasConsumRateThrustForce);
            InvokeThrustForceChanged();
        }
    }

    void ReleaseThrustOverTime()
    {
        if (thrustReleaseTimer < thrustReleaseTime)
        {
            float deltaTime = Time.fixedDeltaTime;
            thrustReleaseTimer += deltaTime;

            float fraction = deltaTime / thrustReleaseTime;

            float totalImpulse = thrustForce;

            float impulseThisFrame = totalImpulse * fraction;

            rb.AddForce(GetThrustDirection() * impulseThisFrame, ForceMode.Impulse);
            InvokeThrustForceChanged();

            if (!bigThrustParticles.isPlaying){bigThrustParticles.Play();}
            audioJetPack.PlayBigThrustSound();
        }
        else
        {
            isReleasingThrust = false;
            thrustReleaseTimer = 0f;
            thrustForceTimer = 0f;
            thrustForce = 0f;
            InvokeThrustForceChanged();

            if (bigThrustParticles.isPlaying){bigThrustParticles.Stop();}
            audioJetPack.StopBigThrustSound();
        }
    }

    void CancelChargingOverTime()
    {
        if (thrustForceTimer > 0f)
        {
            thrustForceTimer -= Time.fixedDeltaTime;
            thrustForceTimer = Mathf.Clamp(thrustForceTimer, 0f, thrustForceTimerMax);

            thrustForce = thrustForceMax * (thrustForceTimer / thrustForceTimerMax);

            float gasReturned = gasConsumRateThrustForce * Time.fixedDeltaTime;
            reservoir += gasReturned;
            reservoir = Mathf.Clamp(reservoir, 0f, reservoirMax);

            timeSinceLastGasUse = 0f;

            InvokeReservoirThrustChanged();
            InvokeThrustForceChanged();
        }
        else
        {
            isCancelingCharge = false;
            timeSinceLastGasUse = gasRefillDelay;
            InvokeThrustForceChanged();
        }
    }


    //* Gestion du gaz 
    void GasConsumption(float gasConsumRate)
    {
        if (reservoir > 0f)
        {
            reservoir -= gasConsumRate * Time.fixedDeltaTime;
            reservoir = Mathf.Clamp(reservoir, 0f, reservoirMax);

            timeSinceLastGasUse = 0f;
            InvokeReservoirThrustChanged();
        }
    }

    void GasRefillOverTime()
    {
        timeSinceLastGasUse += Time.fixedDeltaTime;

        if (timeSinceLastGasUse >= gasRefillDelay)
        {
            reservoir += gasRefillRate * Time.fixedDeltaTime;
            reservoir = Mathf.Clamp(reservoir, 0f, reservoirMax);         

            InvokeReservoirThrustChanged();
        }
    }

    //* Gestion de la force et de la direction pour le joueur
    void ForceModeForce(float force)
    {
        rb.AddForce(GetThrustDirection() * force, ForceMode.Force);
    }

    Vector3 GetThrustDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z));

        Vector3 thrustDirection = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0).normalized;

        return thrustDirection;
    }

    //* Mise à jour de l'interface utilisateur
    private void InvokeReservoirThrustChanged()
    {
        reservoirThrustChanged.Invoke(reservoir, reservoirMax);
    }

    private void InvokeThrustForceChanged()
    {
        thrustForceChanged.Invoke(thrustForce, thrustForceMax);
    }
}
