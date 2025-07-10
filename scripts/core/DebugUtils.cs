using Godot;

namespace Game.Scripts.Core;

public static class DebugUtils
{
    private static SceneTree GetTree() => (SceneTree)Engine.GetMainLoop();
    public static void PrintSceneTree(Node node = null, int depth = 0)
    {
        // Start from root if no node specified
        node ??= GetTree().Root;
        
        string indent = new string(' ', depth * 2);
        GD.Print($"{indent}- {node.Name} ({node.GetType().Name})");

        foreach (Node child in node.GetChildren())
        {
            PrintSceneTree(child, depth + 1);
        }
    }

    // Usage: DebugUtils.FindNode("Player")
    public static Node FindNode(string nodeName, Node root = null)
    {
        root ??= GetTree().Root;
        if (root.Name == nodeName) return root;

        foreach (Node child in root.GetChildren())
        {
            var found = FindNode(nodeName, child);
            if (found != null) return found;
        }
        return null;
    }

    // Usage: DebugUtils.PrintGroup("enemies")
    public static void PrintGroup(string groupName)
    {
        GD.Print($"GROUP: {groupName}");
        foreach (Node node in GetTree().GetNodesInGroup(groupName))
        {
            GD.Print($"- {node.GetPath()}");
        }
    }

    // Usage: DebugUtils.GetDistance(player, enemy)
    public static float GetDistance(Node2D a, Node2D b)
    {
        return a.GlobalPosition.DistanceTo(b.GlobalPosition);
    }

    // Usage: DebugUtils.ToggleVisibility(GetNode("Sprite2D"))
    public static void ToggleVisibility(Node2D node)
    {
        if (node == null)
        {
            GD.PrintErr("Cannot toggle visibility - node is null!");
            return;
        }
        node.Visible = !node.Visible;
        GD.Print($"{node.Name} visibility: {node.Visible}");
    }

    public static void ShowCollisions(bool enable)
    {
        GetTree().DebugCollisionsHint = enable;


    }

    // Toggle navigation debug (blue navigation mesh visuals)
    public static void ShowNavigation(bool enable)
    {
        GetTree().DebugNavigationHint = enable; // shows Area2D
    }

}