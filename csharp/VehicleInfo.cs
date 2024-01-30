using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VehicleInfo : BaseScript {
    private List<int> closestVehicles = new List<int>();

    public VehicleInfo() {
        Tick += RenderInfoText;
        Tick += UpdateVehicles;
    }

    private async Task RenderInfoText() {
        if (closestVehicles.Count == 0) return;

        foreach (int vehicle in closestVehicles) {
            Vector3 coords = GetEntityCoords(vehicle, false);
            int maxHealth = GetEntityMaxHealth(vehicle);
            int health = GetEntityHealth(vehicle);
            float percent = (float)health / maxHealth * 100;
            int[] color = percent > 50 ? new int[] { 255, 255, 255 } : percent > 25 ? new int[] { 255, 255, 0 } : new int[] { 255, 0, 0 };
            string displayName = GetDisplayNameFromVehicleModel((uint)GetEntityModel(vehicle));
            string label = GetLabelText(displayName);
            string name = label != "CARNOTFOUND" ? $"{label} ({displayName})" : displayName;

            BeginTextCommandDisplayText("STRING");
            AddTextComponentSubstringPlayerName($"{name}\n{percent:F2}%");
            SetTextColour(color[0], color[1], color[2], 255);
            SetTextCentre(true);
            SetTextFont(0);
            SetTextScale(0.3f, 0.3f);
            SetDrawOrigin(coords.X, coords.Y, coords.Z + 0.5f, 0);
            EndTextCommandDisplayText(0, 0);
            ClearDrawOrigin();
        }

        await Task.FromResult(0);
    }

    private async Task UpdateVehicles() {
        Vector3 playerCoords = GetEntityCoords(PlayerPedId(), false);

        closestVehicles = World.GetAllVehicles().Where(vehicle => {
            Vector3 vehicleCoords = GetEntityCoords(vehicle.Handle, false);

            return Math.Sqrt(Math.Pow(vehicleCoords.X - playerCoords.X, 2) + Math.Pow(vehicleCoords.Y - playerCoords.Y, 2) + Math.Pow(vehicleCoords.Z - playerCoords.Z, 2)) <= 100;
        }).Select(v => v.Handle).ToList();

        Debug.WriteLine($"Found {closestVehicles.Count} vehicles nearby");

        await Delay(2500);
    }
}
