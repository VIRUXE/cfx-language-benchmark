let closestVehicles = [];

function GetVehicles() {
	const [x, y, z] = GetEntityCoords(PlayerPedId());
	
	closestVehicles = GetGamePool("CVehicle").filter((vehicle) => {
		const [vx, vy, vz] = GetEntityCoords(vehicle);
		
		// Use sqrt and subsctrion. no pow
		return Math.sqrt((vx - x) ** 2 + (vy - y) ** 2 + (vz - z) ** 2) <= 30;
	});

	console.log(`Found ${closestVehicles.length} vehicles nearby`);
}

setInterval(GetVehicles, 2500);

// Render vehcile info on screen
setInterval(() => {
	if (closestVehicles.length === 0) return;

	closestVehicles.forEach((vehicle) => {
		const [x, y, z]   = GetEntityCoords(vehicle);
		const maxHealth   = GetEntityMaxHealth(vehicle);
		const health      = GetEntityHealth(vehicle);
		const percent     = (health / maxHealth) * 100;
		const color       = percent > 50 ? [255, 255, 255] : percent > 25 ? [255, 255, 0] : [255, 0, 0];
		const displayName = GetDisplayNameFromVehicleModel(GetEntityModel(vehicle));
		const label       = GetLabelText(displayName);
		const name        = label !== 'NULL' ? `${label} (${displayName})` : displayName;

		BeginTextCommandDisplayText("STRING");
		AddTextComponentSubstringPlayerName(`${name}~n~${percent.toFixed(2)}%`);
		SetTextColour(color[0], color[1], color[2], 255);
		SetTextCentre(true);
		SetTextFont(0);
		SetTextScale(0.3, 0.3);
		SetDrawOrigin(x, y, z + .5, 0);
		EndTextCommandDisplayText(0, 0);
		ClearDrawOrigin();
	});
}, 0);

GetVehicles();