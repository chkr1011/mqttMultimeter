using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mqttMultimeter.Pages.Inflight.Export.Model;
using MQTTnet;
using Newtonsoft.Json;

namespace mqttMultimeter.Pages.Inflight.Export;

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
            Messages = new List<InflightPageExportMessage>(inflightPage.Items.Count)
        };

        foreach (var item in inflightPage.Items)
        {
            export.Messages.Add(CreateMessageModel(item.Message));
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
            await inflightPage.AppendMessage(CreateMessage(message));
        }
    }

    static MqttApplicationMessage CreateMessage(InflightPageExportMessage model)
    {
        var message = new MqttApplicationMessage
        {
            Topic = model.Topic,
            ResponseTopic = model.ResponseTopic,
            QualityOfServiceLevel = model.QualityOfServiceLevel,
            Retain = model.Retain,
            Dup = model.Dup,
            ContentType = model.ContentType,
            CorrelationData = model.CorrelationData,
            SubscriptionIdentifiers = model.SubscriptionIdentifiers,
            TopicAlias = model.TopicAlias,
            MessageExpiryInterval = model.MessageExpiryInterval,
            PayloadFormatIndicator = model.PayloadFormatIndicator,
            UserProperties = model.UserProperties,
            PayloadSegment = new ArraySegment<byte>(model.Payload ?? Array.Empty<byte>())
        };

        return message;
    }

    static InflightPageExportMessage CreateMessageModel(MqttApplicationMessage message)
    {
        var exportMessage = new InflightPageExportMessage
        {
            Topic = message.Topic,
            ResponseTopic = message.ResponseTopic,
            QualityOfServiceLevel = message.QualityOfServiceLevel,
            Retain = message.Retain,
            Dup = message.Dup,
            ContentType = message.ContentType,
            CorrelationData = message.CorrelationData,
            SubscriptionIdentifiers = message.SubscriptionIdentifiers,
            TopicAlias = message.TopicAlias,
            MessageExpiryInterval = message.MessageExpiryInterval,
            PayloadFormatIndicator = message.PayloadFormatIndicator,
            UserProperties = message.UserProperties,
            Payload = null // Will be set later!
        };

        if (message.Payload.Length > 0)
        {
            exportMessage.Payload = message.Payload.ToArray();
        }
        else
        {
            exportMessage.Payload = [];
        }

        return exportMessage;
    }
}