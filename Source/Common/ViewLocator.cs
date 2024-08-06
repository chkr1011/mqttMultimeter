using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace mqttMultimeter.Common;

sealed class ViewLocator : IDataTemplate
{
    readonly Dictionary<string, Type> _viewTypeCache = new();

    public Control Build(object? viewModel)
    {
        if (viewModel == null)
        {
            return new TextBlock();
        }

        var viewModelTypeName = viewModel.GetType().FullName!;

        if (!_viewTypeCache.TryGetValue(viewModelTypeName, out var viewType))
        {
            // This project expects that a view is located in the same namespace with "ViewModel"
            // replaced with "View"!
            var viewName = viewModel.GetType().FullName!.Replace("ViewModel", "View");
            viewType = Type.GetType(viewName);

            if (viewType != null)
            {
                _viewTypeCache[viewModelTypeName] = viewType;
            }
        }

        if (viewType != null)
        {
            if (Activator.CreateInstance(viewType) is not Control control)
            {
                throw new InvalidOperationException($"Unable to create view of type '{viewType.FullName}'.");
            }

            control.DataContext = viewModel;
            return control;
        }

        return new TextBlock
        {
            Text = $"View not found for view model {viewModelTypeName}."
        };
    }

    public bool Match(object? data)
    {
        return data is BaseViewModel;
    }
}