using System;
using System.Collections.Generic;
using Flow.Core.Models;
using Flow.Core.Plugins;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;

namespace Flow.Tests.TestHelpers;

public static class MockFactory
{
    public static Mock<IGraphManager> CreateGraphManager()
    {
        var mock = new Mock<IGraphManager>();
        return mock;
    }

    public static Mock<INodeFactory> CreateNodeFactory()
    {
        var mock = new Mock<INodeFactory>();
        return mock;
    }

    public static Mock<IGameRegistry> CreateGameRegistry()
    {
        var mock = new Mock<IGameRegistry>();
        mock.Setup(r => r.AvailableGames)
            .Returns(new List<GameInfo> { new GameInfo("Test Factory Game", new Version(1, 0)) });
        return mock;
    }

    public static Recipe CreateRecipe()
    {
        var ironOre = new Item("iron-ore", "Iron Ore");
        var ironPlate = new Item("iron-plate", "Iron Plate");
        var furnace = new Machine("stone-furnace", "Stone Furnace", 50m);

        return new Recipe(
            "iron-smelting",
            "Test Recipe",
            new[] { new ItemStack(ironOre, 1) },
            new[] { new ItemStack(ironPlate, 1) },
            furnace,
            TimeSpan.FromSeconds(3.5)
        );
    }
} 