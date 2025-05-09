map_config = {
    size = 512,
    seed = os.time(),

    biomes = {
        ["forest"] = { probability = 40, splat_color = { r = 0, g = 1, b = 0 } },
        ["desert"] = { probability = 30, splat_color = { r = 1, g = 1, b = 0 } },
        ["mountains"] = { probability = 30, splat_color = { r = 0.3, g = 0.3, b = 0.3 } }
    },

    resources = {
        tree = { count = 50, min_distance = 5 },
        mines = { count = 10, min_distance = 10}
    },
    structures = {
        land_owner_house = {count = 1, min_distance = 50}
    }
}

function GetMapConfig()
    return map_config
end