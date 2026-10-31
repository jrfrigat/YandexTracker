using System.Text.Json.Serialization;

namespace YandexTracker.Models.Users;

/// <summary>A user of the organization.</summary>
/// <param name="Uid">The user id; the same value as <c>id</c> in assignee references.</param>
/// <param name="Login">The login.</param>
/// <param name="Display">The display name.</param>
/// <param name="Email">The email address.</param>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Dismissed">Whether the user has left the organization.</param>
/// <param name="CloudUid">The user id in Yandex Cloud.</param>
/// <param name="PassportUid">The Yandex Passport id.</param>
public sealed record User(
    [property: JsonPropertyName("uid")] long? Uid,
    [property: JsonPropertyName("login")] string? Login,
    [property: JsonPropertyName("display")] string? Display,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("firstName")] string? FirstName = null,
    [property: JsonPropertyName("lastName")] string? LastName = null,
    [property: JsonPropertyName("dismissed")] bool? Dismissed = null,
    [property: JsonPropertyName("cloudUid")] string? CloudUid = null,
    [property: JsonPropertyName("passportUid")] long? PassportUid = null);
