event = {
    ["greet_player"] = {
        condition = function() return player_has_enter_village end,
        action = function() PrintMessage("¡Bienvenido a la aldea te tu putisima madre hijo del la gran puta!") end
    },
    ["start_quest"] = {
        condition = function() return player_level >= 5 end,
        action = function() PrintMessage("O se follaron hoy a la vieja que te gusta :D") end
    }
}

function CheckEvents()
    for event_name, event in pairs(events) do
        if event.condition() then
            event.action()
        end
    end
end