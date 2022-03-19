using System;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;

namespace MQTTnetApp.Services.Scripting;

public sealed class ScriptEngineService
{
    public Task<object> Execute(string script, Action<string> outputHandler)
    {
        if (script == null)
        {
            throw new ArgumentNullException(nameof(script));
        }

        if (outputHandler == null)
        {
            throw new ArgumentNullException(nameof(outputHandler));
        }

        return Task.Run(() =>
        {
            var outputStream = new PythonTextOutputStream();
            outputStream.OutputWritten += outputHandler.Invoke;

            var scriptEngine = Python.CreateEngine();
            scriptEngine.Runtime.IO.SetOutput(outputStream, Encoding.UTF8);
            var pythonScript = scriptEngine.CreateScriptSourceFromString(script);

            return pythonScript.Execute();
        });
    }
}