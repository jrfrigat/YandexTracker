namespace YandexTracker.Http;

/// <summary>The addresses of the services the client talks to.</summary>
internal static class YandexTrackerHosts
{
    /// <summary>The Tracker API.</summary>
    public const string Api = "https://api.tracker.yandex.net/";
}

/// <summary>The organization headers the Tracker API expects.</summary>
internal static class YandexTrackerHeaders
{
    /// <summary>A Yandex 360 organization.</summary>
    public const string Organization = "X-Org-ID";

    /// <summary>A Yandex Cloud organization.</summary>
    public const string CloudOrganization = "X-Cloud-Org-ID";
}
