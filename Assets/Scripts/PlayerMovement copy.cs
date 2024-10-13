using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ThrustEvent2 : UnityEvent<float, float> { }

public class PlayerMovement2 : MonoBehaviour
{
    public ThrustEvent reservoirThrustChanged;
    public ThrustEvent thrustForceChanged;

    private Rigidbody rb;


    public float smallThrustForce = 1f;
    public float thrustForce = 0f;
    public float thrustForceMax = 10f;

    private float thrustForceTimer = 0f;
    public float thrustForceTimerMax = 1f;

    public float reservoir = 10f;
    public float reservoirMax = 10f;


    public float gasConsumptionRate = 1f;
    public float gasRefillRate = 3f;
    public float gasRefillDelay = 3f;
    private float timeSinceLastGasUse = 0f;


    private bool isCharging = false;
    private bool isCancelingCharge = false;
    private float gasToReturn = 0f;
    public float gasCancelReturnRate = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            StartCharging();
        }
        else if (isCharging && !Input.GetMouseButton(0))
        {
            CancelCharging();
        }
        else if (isCharging && Input.GetMouseButtonUp(1))
        {
            ReleaseChargedThrust();
        }
        else if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            SmallThrust();
        }
        else if (isCancelingCharge)
        {
            ReturnGasToReservoir();
        }
        else
        {
            GasRefillOverTime();
        }
    }

    void SmallThrust()
    {
        if (!isCharging && reservoir > 0f)
        {
            ForceModeForce(smallThrustForce);
            GasConsumption(gasConsumptionRate);
        }
    }

    void StartCharging()
    {
        if (thrustForceTimer == thrustForceTimerMax) return;

        if (reservoir > 0f)
        {
            isCharging = true;

            thrustForceTimer += Time.deltaTime;
            thrustForceTimer = Mathf.Clamp(thrustForceTimer, 0f, thrustForceTimerMax);

            thrustForce = thrustForceMax * (thrustForceTimer / thrustForceTimerMax); // Force calculation for UI

            GasConsumption(gasConsumptionRate);
            InvokeThrustForceChanged();
        }
    }

    void ReleaseChargedThrust()
    {
        if (isCharging)
        {
            thrustForce = thrustForceMax * (thrustForceTimer / thrustForceTimerMax);

            ForceModeImpulse(thrustForce);
            InvokeThrustForceChanged();

            isCharging = false;
            thrustForceTimer = 0f;
            timeSinceLastGasUse = 0f;

            Debug.Log("Propulsion libérée : " + thrustForce);
        }
    }

    void CancelCharging()
    {
        if (isCharging)
        {
            gasToReturn = thrustForceTimer * gasConsumptionRate;

            thrustForceTimer = 0f;
            isCharging = false;
            isCancelingCharge = true;

            InvokeThrustForceChanged();

            Debug.Log("Annulation de la charge, retour du gaz en cours");
        }
    }

    void ReturnGasToReservoir()
    {
        if (gasToReturn > 0f)
        {
            float gasReturned = gasCancelReturnRate * Time.deltaTime;

            gasReturned = Mathf.Clamp(gasReturned, 0f, gasToReturn);

            reservoir += gasReturned;
            gasToReturn -= gasReturned;

            reservoir = Mathf.Clamp(reservoir, 0f, reservoirMax);

            timeSinceLastGasUse = 0;

            InvokeReservoirThrustChanged();
            InvokeThrustForceChanged();
        }
        else
        {
            isCancelingCharge = false;
            timeSinceLastGasUse = gasRefillDelay;
            Debug.Log("Retour du gaz terminé");
            InvokeThrustForceChanged();
        }
    }

    void GasRefillOverTime()
    {
        timeSinceLastGasUse += Time.deltaTime;

        if (timeSinceLastGasUse >= gasRefillDelay)
        {
            reservoir += gasRefillRate * Time.deltaTime;
            if (reservoir > reservoirMax)
            {
                reservoir = reservoirMax;
                return;
            }

            InvokeReservoirThrustChanged();
        }
    }

    void GasConsumption (float gasConsumRate){
        if (reservoir > 0f)
        {
            reservoir -= gasConsumRate * Time.deltaTime;

            if (reservoir < 0f)
            {
                reservoir = 0f;
                InvokeReservoirThrustChanged();
                return;
            }

            timeSinceLastGasUse = 0f;
            InvokeReservoirThrustChanged();
        }
    }

    // * Force and direction management for the player
    void ForceModeForce(float force)
    {
        rb.AddForce(GetThrustDirection() * force, ForceMode.Force);
    }

    void ForceModeImpulse(float force)
    {
        rb.AddForce(GetThrustDirection() * force, ForceMode.Impulse);
    }

    Vector3 GetThrustDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z));

        Vector3 thrustDirection = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0).normalized;

        return -thrustDirection;
    }


    // * UI update
    private void InvokeReservoirThrustChanged()
    {
        reservoirThrustChanged.Invoke(reservoir, reservoirMax);
    }

    private void InvokeThrustForceChanged()
    {
        thrustForceChanged.Invoke(thrustForce, thrustForceMax);
    }
}
