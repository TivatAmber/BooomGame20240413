using Mangers;
using Units;
using UnityEngine;

public class DistanceModule
{
    public void Update(Player entity)
    {
        float distance = (entity.transform.position - Global.Planet.PlanetTransform.position).magnitude;
        float deltaEnergy;
        if (distance < Global.Energy.R0)
        {
            deltaEnergy = GetR0Energy(distance);
        }
        else if (distance < Global.Energy.R1)
        {
            deltaEnergy = GetR1Energy(distance);
        }
        else if (distance < Global.Energy.R2)
        {
            deltaEnergy = GetR2Energy(distance);
        }
        else if (distance < Global.Energy.R3)
        {
            deltaEnergy = GetR3Energy(distance);
        }
        else if (distance < Global.Energy.R4)
        {
            deltaEnergy = GetR4Energy(distance);
        }
        else
        {
            deltaEnergy = GetTooFarEnergy(distance);
        }

        deltaEnergy *= Time.deltaTime;
        if (deltaEnergy + entity.energy > 0 && deltaEnergy + entity.energy < entity.energyLimit)
            entity.energy += deltaEnergy;
    }

    float GetR0Energy(float distance)
    {
        return 0;
    }

    float GetR1Energy(float distance)
    {
        return 1f;
    }

    float GetR2Energy(float distance)
    {
        return 0.5f;
    }

    float GetR3Energy(float distance)
    {
        return -0.1f;
    }

    float GetR4Energy(float distance)
    {
        return -1f;
    }

    float GetTooFarEnergy(float distance)
    {
        return -10f;
    }
}
