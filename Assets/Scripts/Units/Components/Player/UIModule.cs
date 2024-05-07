using System;
using GlobalSystem;
using UnityEngine;

namespace Units.Components
{
    public class UIModule
    {
        private Player _target;
        void NowSpeedUpdate()
        {
            _target.nowSpeedUI.SetText(
                $"NowSpeed: {MathF.Round(_target.NowSpeedModule, 2, MidpointRounding.ToEven)}");
        }

        void OrbitalSpeedUpdate()
        {
            _target.orbitalSpeedUI.SetText(
                $"OrbitalSpeed: {MathF.Round(_target.orbitalSpeedModule, 2)}");
        }

        void HeightUpdate()
        {
            _target.heightUI.SetText(
                $"Height: {MathF.Round(Vector3.Distance(GlobalConfigure.Planet.PlanetTransform.position, _target.transform.position) / 5.0f, 0) * 5}");
        }

        void HealthUpdate()
        {
            _target.healthUI.fillAmount = (_target.Health / _target.HealthLimit) * 0.5f;
        }

        void EnergyUpdate()
        {
            _target.energyUI.fillAmount = (_target.Energy / _target.EnergyLimit) * 0.5f;
        }
        void ResourceUpdate()
        {
            _target.resourceUI.SetText($"Resource: {MathF.Round(GlobalLevelUp.AllDoneInfo.GetResources, 2)}");
        }
        public void UIUpdate(Player entity)
        {
            _target = entity;
            if (_target is null) return;
            NowSpeedUpdate();
            OrbitalSpeedUpdate();
            HeightUpdate();
            HealthUpdate();
            EnergyUpdate();
            ResourceUpdate();
        }
    }
}