using Flow.Core.Models;
using Flow.Core.Plugins;

namespace Flow.Games.TestGame;

public class TestGamePlugin : IGamePlugin
{
    public string GameName => "Test Factory Game";
    public Version Version => new(1, 0);

    private readonly List<Item> _items;
    private readonly List<Machine> _machines;
    private readonly List<Recipe> _recipes;

    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
    public IReadOnlyCollection<Machine> Machines => _machines.AsReadOnly();
    public IReadOnlyCollection<Recipe> Recipes => _recipes.AsReadOnly();

    public TestGamePlugin()
    {
        // Create items
        _items = new List<Item>
        {
            // Raw resources
            new("iron-ore", "Iron Ore"),
            new("copper-ore", "Copper Ore"),
            new("coal", "Coal"),
            new("stone", "Stone"),
            
            // Basic materials
            new("iron-plate", "Iron Plate"),
            new("copper-plate", "Copper Plate"),
            new("stone-brick", "Stone Brick"),
            
            // Intermediate products
            new("iron-gear", "Iron Gear"),
            new("copper-wire", "Copper Wire"),
            new("steel-plate", "Steel Plate"),
            
            // Advanced products
            new("electronic-circuit", "Electronic Circuit"),
            new("engine-unit", "Engine Unit"),
            new("electric-motor", "Electric Motor")
        };

        // Create machines
        _machines = new List<Machine>
        {
            new("stone-furnace", "Stone Furnace", 5.0m),
            new("steel-furnace", "Steel Furnace", 15.0m),
            new("assembling-machine-1", "Assembling Machine 1", 20.0m),
            new("assembling-machine-2", "Assembling Machine 2", 35.0m)
        };

        // Create recipes
        _recipes = new List<Recipe>
        {
            // Basic smelting
            new Recipe(
                "iron-smelting",
                "Iron Smelting",
                new[] { new ItemStack(GetItem("iron-ore"), 1.0m) },
                new[] { new ItemStack(GetItem("iron-plate"), 1.0m) },
                GetMachine("stone-furnace"),
                TimeSpan.FromSeconds(3)
            ),
            new Recipe(
                "copper-smelting",
                "Copper Smelting",
                new[] { new ItemStack(GetItem("copper-ore"), 1.0m) },
                new[] { new ItemStack(GetItem("copper-plate"), 1.0m) },
                GetMachine("stone-furnace"),
                TimeSpan.FromSeconds(3)
            ),
            new Recipe(
                "stone-brick",
                "Stone Brick",
                new[] { new ItemStack(GetItem("stone"), 2.0m) },
                new[] { new ItemStack(GetItem("stone-brick"), 1.0m) },
                GetMachine("stone-furnace"),
                TimeSpan.FromSeconds(3)
            ),

            // Advanced smelting
            new Recipe(
                "steel-smelting",
                "Steel Smelting",
                new[] { new ItemStack(GetItem("iron-plate"), 5.0m) },
                new[] { new ItemStack(GetItem("steel-plate"), 1.0m) },
                GetMachine("steel-furnace"),
                TimeSpan.FromSeconds(16)
            ),

            // Basic assembly
            new Recipe(
                "iron-gear",
                "Iron Gear",
                new[] { new ItemStack(GetItem("iron-plate"), 2.0m) },
                new[] { new ItemStack(GetItem("iron-gear"), 1.0m) },
                GetMachine("assembling-machine-1"),
                TimeSpan.FromSeconds(1)
            ),
            new Recipe(
                "copper-wire",
                "Copper Wire",
                new[] { new ItemStack(GetItem("copper-plate"), 1.0m) },
                new[] { new ItemStack(GetItem("copper-wire"), 2.0m) },
                GetMachine("assembling-machine-1"),
                TimeSpan.FromSeconds(1)
            ),

            // Advanced assembly
            new Recipe(
                "electronic-circuit",
                "Electronic Circuit",
                new[] 
                { 
                    new ItemStack(GetItem("iron-plate"), 1.0m),
                    new ItemStack(GetItem("copper-wire"), 3.0m)
                },
                new[] { new ItemStack(GetItem("electronic-circuit"), 1.0m) },
                GetMachine("assembling-machine-2"),
                TimeSpan.FromSeconds(1)
            ),
            new Recipe(
                "electric-motor",
                "Electric Motor",
                new[]
                {
                    new ItemStack(GetItem("iron-gear"), 1.0m),
                    new ItemStack(GetItem("copper-wire"), 4.0m)
                },
                new[] { new ItemStack(GetItem("electric-motor"), 1.0m) },
                GetMachine("assembling-machine-2"),
                TimeSpan.FromSeconds(2)
            ),
            new Recipe(
                "engine-unit",
                "Engine Unit",
                new[]
                {
                    new ItemStack(GetItem("steel-plate"), 1.0m),
                    new ItemStack(GetItem("iron-gear"), 1.0m),
                    new ItemStack(GetItem("electric-motor"), 2.0m)
                },
                new[] { new ItemStack(GetItem("engine-unit"), 1.0m) },
                GetMachine("assembling-machine-2"),
                TimeSpan.FromSeconds(4)
            )
        };
    }

    private Item GetItem(string identifier) => _items.First(i => i.Identifier == identifier);
    private Machine GetMachine(string identifier) => _machines.First(m => m.Identifier == identifier);
} 