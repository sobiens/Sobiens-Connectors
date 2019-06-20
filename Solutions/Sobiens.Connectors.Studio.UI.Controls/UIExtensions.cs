using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    public static class UIExtensions
    {
        #region Methods
        /// <summary>
        /// Uses tags on the child controls to bind to properties on the source entity object
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="sourceEntity">The entity to bind to</param>
        public static void FillItemsByEnum(this ComboBox uc, Type enumType)
        {
            List<object> list = new List<object>();
            foreach (object enumValue in Enum.GetValues(enumType))
            {
                list.Add(enumValue);
            }
            uc.ItemsSource = list;
        }

        /// <summary>
        /// Uses tags on the child controls to bind to properties on the source entity object
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="sourceEntity">The entity to bind to</param>
        public static void BindControls(this FrameworkElement uc, object sourceEntity)
        {
            uc.Tag = sourceEntity;
            IEnumerable elements = LogicalTreeHelper.GetChildren(uc);
            foreach (object child in elements)
            {
                FrameworkElement currentUserControl = child as FrameworkElement;
                if (currentUserControl != null)
                {
                    object tagValue = currentUserControl.Tag;
                    if (tagValue != null && tagValue.ToString() != String.Empty)
                    {
                        System.Windows.Data.Binding binding = new System.Windows.Data.Binding(tagValue.ToString());
                        binding.Source = sourceEntity;

                        if (currentUserControl as CheckBox != null)
                        {
                            currentUserControl.SetBinding(CheckBox.IsCheckedProperty, binding);
                        }
                        else if (currentUserControl as ComboBox != null)
                        {
                            if (((ComboBox)currentUserControl).IsEditable == true)
                            {
                                currentUserControl.SetBinding(ComboBox.TextProperty, binding);
                            }
                            else
                            {
                                currentUserControl.SetBinding(ComboBox.SelectedItemProperty, binding);
                            }
                        }
                        else if (currentUserControl as ListBox != null)
                        {
                            currentUserControl.SetBinding(ListBox.SelectedItemProperty, binding);
                        }
                        else if (currentUserControl as TextBox != null)
                        {
                            currentUserControl.SetBinding(TextBox.TextProperty, binding);
                        }
                        else if (currentUserControl as BindablePasswordBox != null)
                        {
                            currentUserControl.SetBinding(BindablePasswordBox.PasswordProperty, binding);
                        }
                        else if (currentUserControl as RadioButton != null)
                        {
                            currentUserControl.SetBinding(RadioButton.IsCheckedProperty, binding);
                        }
                    }
                    else
                    {
                    }

                    BindControls(currentUserControl, sourceEntity);
                }
            }
        }

        /// <summary>
        /// Returns the first child control within the current control with the matchin tag
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="tag">The Tag Value</param>
        /// <returns>A FrameworkElement</returns>
        public static FrameworkElement GetChildByTag(this FrameworkElement uc, string tag)
        {
            IEnumerable elements = LogicalTreeHelper.GetChildren(uc);
            foreach (object child in elements)
            {
                FrameworkElement currentUserControl = child as FrameworkElement;
                if (currentUserControl != null)
                {
                    object tagValue = currentUserControl.Tag;
                    if (tagValue != null && tagValue.ToString() != String.Empty)
                    {
                        if (tagValue.ToString() == tag)
                        {
                            return currentUserControl;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first child control of the specified type within the current control with the matching tag
        /// </summary>
        /// <typeparam name="T">The Type of the child control</typeparam>
        /// <param name="uc"></param>
        /// <param name="tag">The Tag Value</param>
        /// <returns>A FrameworkElement</returns>
        public static T GetChildByTag<T>(this FrameworkElement uc, string tag) where T : FrameworkElement
        {
            IEnumerable elements = LogicalTreeHelper.GetChildren(uc);
            foreach (object child in elements)
            {
                FrameworkElement currentUserControl = child as FrameworkElement;
                if (currentUserControl != null)
                {
                    object tagValue = currentUserControl.Tag;
                    if (tagValue != null && tagValue.ToString() != String.Empty)
                    {
                        if (tagValue.ToString() == tag && (currentUserControl as T != null))
                        {
                            return (T)currentUserControl;
                        }
                    }
                }
            }
            return null;
        }

        public static void DoWhenLoaded<T>(this T element, Action<T> action)
where T : FrameworkElement
        {
            if (element.IsLoaded)
            {
                action(element);
            }
            else
            {
                RoutedEventHandler handler = null;
                handler = (sender, e) =>
                {
                    element.Loaded -= handler;
                    action(element);
                };
                element.Loaded += handler;
            }
        }
        #endregion
    }
}
