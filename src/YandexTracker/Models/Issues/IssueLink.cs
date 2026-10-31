using System.Text.Json.Serialization;

namespace YandexTracker.Models.Issues;

/// <summary>A link between two issues, as read from one of them.</summary>
/// <param name="Id">The link id.</param>
/// <param name="Type">The kind of link.</param>
/// <param name="Direction">
/// Which end of the link the issue it was read from stands at: <c>outward</c> or <c>inward</c>. For <c>depends</c>,
/// the outward end is the issue that depends on <paramref name="Object"/>.
/// </param>
/// <param name="Object">The issue at the other end.</param>
/// <param name="CreatedAt">When the link was created.</param>
public sealed record IssueLink(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("type")] LinkType? Type,
    [property: JsonPropertyName("direction")] string? Direction,
    [property: JsonPropertyName("object")] ObjectReference? Object,
    [property: JsonPropertyName("createdAt")] DateTimeOffset? CreatedAt = null);

/// <summary>The kind of an issue link.</summary>
/// <param name="Id">The kind id, e.g. <c>depends</c>, <c>relates</c> or <c>subtask</c>.</param>
/// <param name="Inward">The label of the inward end.</param>
/// <param name="Outward">The label of the outward end.</param>
public sealed record LinkType(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("inward")] string? Inward = null,
    [property: JsonPropertyName("outward")] string? Outward = null);

/// <summary>The relationship values accepted when creating a link (the <c>relationship</c> field).</summary>
public static class LinkRelationships
{
    /// <summary>The issues are related.</summary>
    public const string Relates = "relates";

    /// <summary>This issue depends on the other one.</summary>
    public const string DependsOn = "depends on";

    /// <summary>The other issue depends on this one.</summary>
    public const string IsDependentBy = "is dependent by";

    /// <summary>This issue is a subtask of the other one.</summary>
    public const string IsSubtaskFor = "is subtask for";

    /// <summary>This issue is the parent of the other one.</summary>
    public const string IsParentTaskFor = "is parent task for";

    /// <summary>This issue duplicates the other one.</summary>
    public const string Duplicates = "duplicates";

    /// <summary>The other issue duplicates this one.</summary>
    public const string IsDuplicatedBy = "is duplicated by";

    /// <summary>This issue is the epic of the other one.</summary>
    public const string IsEpicOf = "is epic of";

    /// <summary>This issue belongs to the other one as its epic.</summary>
    public const string HasEpic = "has epic";
}
