// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkTools.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   WPF / .Net framework tools
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// WPF / .Net framework tools
    /// </summary>
    public static class FrameworkTools
    {
        /// <summary>
        /// Invokes evend handlers through reflection
        /// </summary>
        /// <param name="target">The target object to invoke handler on</param>
        /// <param name="routedEvent">The event to invoke</param>
        public static void InvokeEventHandlers(UIElement target, RoutedEvent routedEvent)
        {
            InvokeEventHandlers(target, routedEvent, new RoutedEventArgs());
        }

        /// <summary>
        /// Invokes evend handlers through reflection
        /// </summary>
        /// <param name="target">The target object to invoke handler on</param>
        /// <param name="routedEvent">The event to invoke</param>
        /// <param name="routedArgs">The event arguments</param>
        public static void InvokeEventHandlers(UIElement target, RoutedEvent routedEvent, RoutedEventArgs routedArgs)
        {
            var targetType = target.GetType();
            var storeProperty = targetType.GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            if(storeProperty == null)
            {
                throw new InvalidOperationException("Property EventHandlersStore not found in target element");
            }

            var store = storeProperty.GetValue(target, null);
            if(store == null)
            {
                throw new InvalidOperationException("EventHandlersStore not initialized in target element");
            }

            var storeType = store.GetType();            
            var containsMethod = storeType.GetMethod("Contains", BindingFlags.Instance | BindingFlags.Public);
            if (containsMethod == null)
            {
                throw new InvalidOperationException("Method Contains of EventHandlerStore in target element not found");
            }

            var containsResult = containsMethod.Invoke(store, new object[] { routedEvent });
            if(!(containsResult is Boolean) || !(Boolean)containsResult)
            {
                throw new InvalidOperationException("Event to invoke not found in EventHandlerStore of target element");
            }

            var getMethod = storeType.GetMethod("GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public);
            if(getMethod == null)
            {
                throw new InvalidOperationException("Method 'GetRoutedEventHandlers' not found in EventHandlerStore of target element");
            }

            var infos = getMethod.Invoke(store, new object[] { routedEvent }) as RoutedEventHandlerInfo[];
            if(infos == null)
            {
                throw new InvalidOperationException("No method info found in EventHandlerStore of target element for requested event");
            }

            foreach (var routedEventHandlerInfo in infos)
            {
                routedEventHandlerInfo.Handler.DynamicInvoke(new object[] { target, routedArgs });
            }
        }

        /// <summary>
        /// Does an action for each visual child
        /// </summary>
        /// <param name="dependencyObj">The container</param>
        /// <param name="childAction">The action to run</param>
        public static void ForEachVisualChild(this DependencyObject dependencyObj, Action<DependencyObject> childAction)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObj); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObj, i);
                child.ForEachVisualChild(childAction);
                childAction(child);
            }
        }
    }
}
