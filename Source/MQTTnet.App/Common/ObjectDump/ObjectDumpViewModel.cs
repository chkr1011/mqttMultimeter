using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MQTTnet.App.Common.ObjectDump
{
    public sealed class ObjectDumpViewModel : BaseViewModel
    {
        public ObservableCollection<ObjectDumpPropertyViewModel> Properties { get; } = new();

        public void Dump(object? graph)
        {
            Properties.Clear();

            if (graph == null)
            {
                return;
            }

            var properties = graph.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(p => p.Name).ToList();

            foreach (var property in properties)
            {
                Properties.Add(new ObjectDumpPropertyViewModel
                {
                    Name = property.Name,
                    Value = Convert.ToString(property.GetValue(graph)) ?? string.Empty
                });
            }
        }
    }
}
