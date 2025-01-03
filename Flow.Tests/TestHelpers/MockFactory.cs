using System;
using Flow.Core.Models;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;

namespace Flow.Tests.TestHelpers;

public static class MockFactory
{
    public static Mock<INodeFactory> CreateNodeFactory()
    {
        var factory = new Mock<INodeFactory>();
        factory.Setup(f => f.CreateNode(It.IsAny<NodeType>(), It.IsAny<IGraphManager>()))
            .Returns((NodeType type, IGraphManager manager) => new NodeViewModel(manager)
            {
                Title = $"New {type} Node",
                NodeType = type,
                Position = new Avalonia.Point(100, 100)
            });
        factory.Setup(f => f.CreateRecipeNode(It.IsAny<Recipe>(), It.IsAny<IGraphManager>()))
            .Returns((Recipe recipe, IGraphManager manager) => new RecipeNodeViewModel(manager)
            {
                Title = "New Recipe Node",
                Recipe = recipe,
                Position = new Avalonia.Point(100, 100)
            });
        return factory;
    }

    public static Mock<IGraphManager> CreateGraphManager()
    {
        var manager = new Mock<IGraphManager>();
        manager.Setup(m => m.AddNode(It.IsAny<NodeViewModel>()));
        manager.Setup(m => m.RemoveNode(It.IsAny<NodeViewModel>()));
        manager.Setup(m => m.ClearGraph());
        manager.Setup(m => m.CurrentGraph).Returns(() => 
        {
            var nodeFactory = CreateNodeFactory();
            return new GraphCanvasViewModel(nodeFactory.Object, manager.Object);
        });
        return manager;
    }

    public static Mock<Recipe> CreateRecipe()
    {
        var ironOre = new Item("iron-ore", "Iron Ore");
        var ironPlate = new Item("iron-plate", "Iron Plate");
        var furnace = new Machine("stone-furnace", "Stone Furnace", 50m);

        var recipe = new Recipe(
            "iron-smelting",
            "Iron Smelting",
            new[] { new ItemStack(ironOre, 1) },
            new[] { new ItemStack(ironPlate, 1) },
            furnace,
            TimeSpan.FromSeconds(3.5)
        );

        var mock = new Mock<Recipe>();
        mock.Setup(r => r.Identifier).Returns(recipe.Identifier);
        mock.Setup(r => r.DisplayName).Returns(recipe.DisplayName);
        mock.Setup(r => r.Inputs).Returns(recipe.Inputs);
        mock.Setup(r => r.Outputs).Returns(recipe.Outputs);
        mock.Setup(r => r.Machine).Returns(recipe.Machine);
        mock.Setup(r => r.ProcessingTime).Returns(recipe.ProcessingTime);
        return mock;
    }

    public static Mock<IUIActionManager> CreateUIActionManager()
    {
        var mock = new Mock<IUIActionManager>();
        mock.Setup(m => m.RegisterAction(It.IsAny<string>(), It.IsAny<Func<Task<bool>>>(), It.IsAny<Func<Task<bool>>>()));
        mock.Setup(m => m.UnregisterAction(It.IsAny<string>()));
        mock.Setup(m => m.ExecuteActionAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.CanExecuteActionAsync(It.IsAny<string>())).ReturnsAsync(true);
        return mock;
    }
} 