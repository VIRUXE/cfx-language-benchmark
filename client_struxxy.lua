local closestVehicles = {}

function GetVehicles()
    closestVehicles = {}

    local playerCoords = GetEntityCoords(PlayerPedId())
    local vehiclePool = GetGamePool('CVehicle')
    for i = 1, #vehiclePool do
        if #(playerCoords - GetEntityCoords(vehiclePool[i])) < 30 then
            closestVehicles[#closestVehicles+1] = vehiclePool[i]
        end
    end

    -- print('Found ' .. #closestVehicles .. ' vehicles nearby')
end

Citizen.CreateThread(function()
    while true do
        Citizen.Wait(2500)
        GetVehicles()
    end
end)

Citizen.CreateThread(function()
    while true do
        Citizen.Wait(0)
        if #closestVehicles == 0 then goto continue end

        for i = 1, #closestVehicles do
            local vehicle     = closestVehicles[i]
            local coords      = GetEntityCoords(vehicle)
            local maxHealth   = GetEntityMaxHealth(vehicle)
            local health      = GetEntityHealth(vehicle)
            local percent     = (health / maxHealth) * 100
            local color       = percent > 50 and {255, 255, 255} or percent > 25 and {255, 255, 0} or {255, 0, 0}
            local displayName = GetDisplayNameFromVehicleModel(GetEntityModel(vehicle))
            local label       = GetLabelText(displayName)
            local name        = label ~= 'NULL' and label .. ' (' .. displayName .. ')' or displayName

            BeginTextCommandDisplayText('STRING')
            AddTextComponentSubstringPlayerName(name .. '~n~' .. string.format("%.2f", percent) .. '%')
            SetTextColour(color[1], color[2], color[3], 255)
            SetTextCentre(true)
            SetTextFont(0)
            SetTextScale(0.3, 0.3)
            SetDrawOrigin(coords.x, coords.y, coords.z + .5, 0)
            EndTextCommandDisplayText(0, 0)
            ClearDrawOrigin()
        end

        ::continue::
    end
end)

GetVehicles()