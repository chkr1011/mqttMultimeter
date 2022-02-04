﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MQTTnet.App.Services.Updates;

public sealed class AppUpdateService
{
    public AppUpdateService()
    {
        CurrentVersion = typeof(App).Assembly.GetName().Version ?? new Version(0, 0, 0, 0);
    }

    public Version CurrentVersion { get; }

    public bool IsUpdateAvailable { get; private set; }

    public Version? LatestVersion { get; private set; }

    public string UpdateUrl { get; private set; }

    public void EnableUpdateChecks()
    {
        Task.Run(DoUpdateChecks);
    }

    async Task DoUpdateChecks()
    {
        using (var httpClient = new HttpClient())
        {
            while (true)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://api.github.com/repos/chkr1011/MQTTnet.App/releases")
                    };

                    request.Headers.UserAgent.ParseAdd("MQTTnet.App");

                    var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                    var releasesJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var releases = JArray.Parse(releasesJson);
                    var latestReleaseName = releases.SelectToken("[0].name")?.Value<string>() ?? string.Empty;

                    // Remove the "v" from the version from GitHub. Then it can be safely parsed.
                    LatestVersion = Version.Parse(latestReleaseName.TrimStart('v'));

                    IsUpdateAvailable = LatestVersion > CurrentVersion;
                }
                catch
                {
                    // Do nothing for now.
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(5));
                }
            }
        }
    }
}