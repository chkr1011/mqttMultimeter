using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using mqttMultimeter.Services.Data;

namespace mqttMultimeter.Services.State;

public sealed class StateService
{
    readonly JsonSerializerService _jsonSerializerService;
    readonly Dictionary<string, JsonNode?> _state = new();

    bool _isLoaded;

    public StateService(JsonSerializerService jsonSerializerService)
    {
        _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
    }

    public event EventHandler<SavingStateEventArgs>? Saving;

    public void Set<TData>(string key, TData? data)
    {
        if (_state == null)
        {
            throw new InvalidOperationException("The storage is not loaded.");
        }

        _state[key] = JsonNode.Parse(_jsonSerializerService.Serialize(data));
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
            // TODO: Use proper logging framework.
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

            var json = _jsonSerializerService.Serialize(state.Value);
            await File.WriteAllTextAsync(path, json).ConfigureAwait(false);
        }
    }

    static string GeneratePath()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        // We use ".MQTTnetApp" instead of ".mqttMultimeter" because the app name was changed
        // and the state should be still working when starting the app with the new name!
        path = Path.Combine(path, ".MQTTnetApp", "State");

        Directory.CreateDirectory(path);

        return path;
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

                _state[key] = _jsonSerializerService.Deserialize<JsonValue>(json);
            }
        }
        catch (FileNotFoundException)
        {
            // In this case we start with empty data.
        }

        _isLoaded = true;
    }
}