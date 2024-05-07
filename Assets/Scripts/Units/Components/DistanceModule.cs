using GlobalSystem;
using Units;
using UnityEngine;

public class DistanceModule
{
    public void Update(Entity entity)
    {
        float distance = (entity.transform.position - GlobalConfigure.Planet.PlanetTransform.position).magnitude;
        float deltaEnergy;
        if (distance < GlobalConfigure.Energy.R0)
        {
            deltaEnergy = GetR0Energy(distance);
        }
        else if (distance < GlobalConfigure.Energy.R1)
        {
            deltaEnergy = GetR1Energy(distance);
        }
        else if (distance < GlobalConfigure.Energy.R2)
        {
            deltaEnergy = GetR2Energy(distance);
        }
        else if (distance < GlobalConfigure.Energy.R3)
        {
            deltaEnergy = GetR3Energy(distance);
        }
        else if (distance < GlobalConfigure.Energy.R4)
        {
            deltaEnergy = GetR4Energy(distance);
        }
        else
        {
            deltaEnergy = GetTooFarEnergy(distance);
        }

        deltaEnergy *= Time.deltaTime;
        // Debug.Log(deltaEnergy);
        if (deltaEnergy + entity.energy > 0)
        {
            entity.energy = Mathf.Min(entity.energyLimit, entity.energy + deltaEnergy);
        }
    }

    float GetR0Energy(float distance)
    {
        return GlobalConfigure.Energy.E0;
    }

    float GetR1Energy(float distance)
    {
        return GlobalConfigure.Energy.E1;
    }

    float GetR2Energy(float distance)
    {
        return GlobalConfigure.Energy.E2 * (distance - GlobalConfigure.Energy.R2) / (GlobalConfigure.Energy.R1 - GlobalConfigure.Energy.R2);
    }

    float GetR3Energy(float distance)
    {
        return GlobalConfigure.Energy.E3;
    }

    float GetR4Energy(float distance)
    {
        return GlobalConfigure.Energy.E4;
    }

    float GetTooFarEnergy(float distance)
    {
        return 0f;
    }
}
