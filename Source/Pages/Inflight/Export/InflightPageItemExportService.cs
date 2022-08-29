using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnetApp.Pages.Inflight.Export.Model;
using Newtonsoft.Json;

namespace MQTTnetApp.Pages.Inflight.Export;

public sealed class InflightPageItemExportService
{
    public async Task Export(InflightPageViewModel inflightPage, string fileName)
    {
        if (inflightPage == null)
        {
            throw new ArgumentNullException(nameof(inflightPage));
        }

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var export = new InflightPageExport
        {
            Messages = new List<MqttApplicationMessage>(inflightPage.Items.Count)
        };

        foreach (var item in inflightPage.Items)
        {
            export.Messages.Add(item.Message);
        }

        var json = JsonConvert.SerializeObject(export, Formatting.Indented);

        await File.WriteAllTextAsync(fileName, json, Encoding.UTF8, CancellationToken.None);
    }

    public async Task Import(InflightPageViewModel inflightPage, string fileName)
    {
        if (inflightPage == null)
        {
            throw new ArgumentNullException(nameof(inflightPage));
        }

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var json = await File.ReadAllTextAsync(fileName, Encoding.UTF8, CancellationToken.None);
        var export = JsonConvert.DeserializeObject<InflightPageExport>(json);

        if (export == null)
        {
            return;
        }

        if (export.Messages == null)
        {
            return;
        }

        foreach (var message in export.Messages)
        {
            await inflightPage.AppendMessage(message);
        }
    }
}