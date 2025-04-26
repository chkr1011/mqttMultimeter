using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using mqttMultimeter.Services.Data;

namespace mqttMultimeter.Services.State;

public sealed class StateService(JsonSerializerService jsonSerializerService)
{
    readonly JsonSerializerService _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
    readonly Dictionary<string, JsonNode?> _state = new();

    bool _isLoaded;

    public event EventHandler<SavingStateEventArgs>? Saving;

    public void Set<TData>(string key, TData? data)
    {
        if (_state == null)
        {
            throw new InvalidOperationException("The storage is not loaded.");
        }

        _state[key] = _jsonSerializerService.SerializeToNode(data);
    }

    public bool TryGet<TData>(string key, out TData? data)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (!_isLoaded)
        {
            Load();
        }

        data = default;

        if (!_state.TryGetValue(key, out var value))
        {
            return false;
        }

        try
        {
            data = _jsonSerializerService.Deserialize<TData>(value?.ToString() ?? string.Empty);
            return true;
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }

        return false;
    }

    public async Task Write()
    {
        Saving?.Invoke(this, new SavingStateEventArgs(this));

        // The state is written into multiple files so that editing and debugging gets
        // easier. So the files can be opened with VSCode etc. for analysis.
        foreach (var state in _state)
        {
            var path = Path.Combine(GeneratePath(), state.Key + ".json");
            Debug.WriteLine("Writing state to \'{Path}\'", path);

            var json = _jsonSerializerService.SerializeToString(state.Value);
            await File.WriteAllTextAsync(path, json).ConfigureAwait(false);
        }
    }

    static string GeneratePath()
    {
        string path;

        // This will work for Linux and macOS.
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            path = GeneratePathForLinux();
        }
        else
        {
            path = GeneratePathForWindows();
        }

        Migrate(path);

        path = Path.Combine(path, "State");
        Directory.CreateDirectory(path);

        return path;
    }

    static string GeneratePathForLinux()
    {
        // We follow the XDG Base Directory Specification https://specifications.freedesktop.org/basedir-spec/basedir-spec-latest.html
        var path = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
        if (string.IsNullOrEmpty(path))
        {
            // From spec: If $XDG_CONFIG_HOME is either not set or empty, a default equal to $HOME/.config should be used.
            path = Environment.GetEnvironmentVariable("HOME");
            if (string.IsNullOrEmpty(path))
            {
                path = "~";
            }

            path = Path.Combine(path, ".config");
        }

        return Path.Combine(path, "mqtt-multimeter");
    }

    static string GeneratePathForWindows()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mqtt-multimeter");
    }

    void Load()
    {
        try
        {
            var statePath = GeneratePath();
            var stateFiles = Directory.GetFiles(statePath, "*.json");

            foreach (var stateFile in stateFiles)
            {
                var json = File.ReadAllText(stateFile);
                var key = Path.GetFileNameWithoutExtension(stateFile);

                _state[key] = _jsonSerializerService.Deserialize<JsonNode>(json);
            }
        }
        catch (FileNotFoundException)
        {
            // In this case we start with empty data.
        }

        _isLoaded = true;
    }

    static void Migrate(string destinationPath)
    {
        try
        {
            // The first version of the app was named "MQTTnetApp". That is the reason for the different name.
            var legacyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".MQTTnetApp");

            if (!Directory.Exists(legacyPath))
            {
                // There is nothing to migrate...
                return;
            }

            if (Directory.Exists(destinationPath))
            {
                // If the new directory exist we create a backup of the data so that the user can manually restore the data.
                Directory.Move(destinationPath, destinationPath + "-backup-" + Guid.NewGuid());
            }

            Directory.Move(legacyPath, destinationPath);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }
}