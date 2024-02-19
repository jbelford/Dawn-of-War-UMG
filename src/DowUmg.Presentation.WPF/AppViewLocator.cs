using System;
using System.Collections.Generic;
using ReactiveUI;
using Splat;

namespace DowUmg.Presentation.WPF
{
    public class AppViewLocator : IViewLocator
    {
        private readonly Dictionary<Type, Type> resolvedViews = [];

        public IViewFor ResolveView<T>(T viewModel, string contract = null)
        {
            Type viewModelType = viewModel.GetType();
            if (!resolvedViews.TryGetValue(viewModelType, out Type viewType))
            {
                var viewModelName = viewModel.GetType().Name;
                var viewTypeName = viewModelName.TrimEnd("Model".ToCharArray());
                viewType = Type.GetType($"DowUmg.Presentation.WPF.Views.{viewTypeName}");
                if (viewType == null)
                {
                    this.Log()
                        .Error(
                            $"Could not find the view {viewTypeName} for view model {viewModelName}."
                        );
                    return null;
                }
                resolvedViews[viewModelType] = viewType;
            }

            try
            {
                return Activator.CreateInstance(viewType) as IViewFor;
            }
            catch (Exception)
            {
                this.Log().Error($"Could not instantiate view {viewType.FullName}.");
                throw;
            }
        }
    }
}
