   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      B    <Sobiens.Connectors.Studio.UI.Controls.SyncCopyListWizardForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5      7   1Sobiens.Connectors.Studio.UI.Controls.HostControl    Loaded$  HostControl_Loaded��5   �   $��Stretch=�67   $��Stretch=�6U   $��Stretch=�6q   $��Stretch=�6�    � 	Resources 5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $a [pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml?�6%   5      %    5      .��5      � $��0,0,0,0q�6   $��Stretch=�6   $��Stretch=�6:   .+�5   
   �� -   5        $��WizardTabControl��5   D   .@�5      �� $��Source��6      TabItemStyle#���   6&   .��5      �  � ColumnDefinitions 5      �� 5      $
��100* �6+    � RowDefinitions 5      �� 5      $
��100* �6(   68   �� 5      $��30 �6(   66   .+�5      A   ;Sobiens.Connectors.Studio.UI.Controls.QueryGeneratorControl  -   5          Name$! SourceQueryGeneratorControl��5   1   $��0,0,0,0q�6V   �� 5      -   5         ��Name$ SourceNextButton��5      $
��Next��67   $��Right=�6F   $��0,5,5,5q�6b   $��1��6s   $��1��6�     GenericButtonStyle#���  6�   6�   5       5!      �� 5"      $��Destination��6   #���   6+   .��5#      �  5$      �� 5%      $	��60* �6+   �� 5&      $
��187* �6+    5(      �� 5)      $
��100* �6(   68   �� 5*      $��30 �6(   66   .+�5-      �� -   5        $ DestinationBackButton��5-      $
��Back��6<   $��Right=�6K   $��0,5,90,5q�6g   $��75��6y   $��1��6�   $��2��6�   #���  6�   �� 5.      -   5        $ DestinationNextButton��5.      $
��Next��6<   $��Right=�6K   $��0,5,5,5q�6g   $��75��6x   $��1��6�   $��1��6�   #���  6�   �� 5/      -   5         ��Name$ DestionationLabel��5/      $��Destination:��67   $
��Left=�6N   $��	42,84,0,0q�6i   $	��Top=�6|   	 J� RenderTransformOrigin$	 -0.645,-0.212&�6�     GenericLabelStyle#���  6�   �� 50      -   5        $ DestinationSelectButton��50      $��Please select...��6>   $
��Left=�6Y   $��	14,87,0,0q�6t   $	��Top=�6�   $	��150��6�   $	 
0.601,-2.3&�6�   $��1��6�   #���  6  51      52      �� 53      $��Mapping��6   #���   6'   .��54      �  55      �� 56      $
��247* �6+   �� 57      $	��14* �6+   �� 58      $
��233* �6+    5:      �� 5;      $	��150 �6(   67   �� 5<      $	��50* �6(   67   �� 5=      $	��50* �6(   67   �� 5>      $��30 �6(   66   .+�5@      � $��12,5,0,0q�6   $
��Left=�6-   $��3��6H   $��0��6\   .+�5A      �� $	��450��6#   $��60��6/   $��0,0,0,0q�6;   $	��Top=�6L   ��5B      � 5C   "   $��25��6'   .+�5D   &   �� -   5        $��AllowUpdateCheckBox��5D   @   $��0,0,0,0q�6/   $!��Enable update field mapping��6[   $	��170��6�   $
��Left=�6�   $��Center=�6�   $��25��6�     GenericCheckBoxStyle#���  6   6Q  5E   #   .��5G      � $��Stretch=�6#   $��Stretch=�6C   .+�5H   "   �� $��0,0,0,0q�6(   $
��Left=�69   $	��Top=�6T   $��Source field:��6l   $��30��6�   #���  6�   6�   �� 5I   "   -	   5        $��UpdateSourceFieldComboBox��5I   +   $��	100,0,0,0q�6L   $	��Top=�6_   $
��Left=�6w   $	��100��6�   $��25��6�   {�.    GenericComboBoxStyle#���  6�   6�   �� 5J   "   $��	220,0,0,0q�6(   $
��Left=�6;   $	��Top=�6V   $��Destination field:��6n   $��30��6�   #���  6�   6�   �� 5K   "   -
   5        $$��UpdateDestinationFieldComboBox��5K   +   $��	320,0,0,0q�6Q   $	��Top=�6d   $
��Left=�6|   $	��100��6�   $��25��6�   {�.  #���  6�   6�   5L      5M      �� 5O      -   5        $��AllowDeltaUpdateGroupBox��5O   e   $	��450��6#   $��70��6/   $��0,60,0,0q�6;   $	��Top=�6M   ��5P      � 5Q   "   $��25��6'   .+�5R   &   �� -   5        $��AllowDeltaUpdateCheckBox��5R   @   $��0,0,0,0q�6/   $��Enable delta update��6`   $	��130��6~   $
��Left=�6�   $��Center=�6�   $��25��6�   #���  6'  6X  5S   #   ��5U      � 5V   "   .+�5W   &   �� $��0,0,0,0q�6,   $
��Left=�6=   $	��Top=�6X   $��Last Modified Field:��6p   $��30��6�   #���  6�   6�   �� 5X   &   -   5        $%��LastModifiedSourceFieldComboBox��5X   /   $��	130,0,0,0q�6V   $	��Top=�6i   $
��Left=�6�   $	��100��6�   $��25��6�   {�.  #���  6�   6�   5Y   #   5[      5^      �� 5_      $
��Auto=�6#   $��Disabled=�6F   $	��Top=�6o   $
��Left=�6�   $��12,0,0,0q�6�   $��3��6�   $��1��6�   
����� �.��5`      � -   5        $��FieldMappingsStackPanel��5`   3   $��
12,0,12,32q�6   $	��Top=�6R   $
��Left=�6j   5a      �� 5c      $
��Auto=�6#   $��Disabled=�6F   $	��Top=�6o   $
��Left=�6�   $��12,0,0,0q�6�   $��3��6�   $��2��6�   
����� �.��5d      &   System.Windows.Controls.DataGrid  -   5        $��ResultGridOnMapping��5d   �   
   CanUserAddRows
 .  $��0,0,0,0q�6;   $��Stretch=�6L   $��Stretch=�6h      AutoGenerateColumns .  
���� ��5e      5f      �� 5h      -   5        $ MappingBackButton��5h      $
��Back��68   $��Right=�6G   $��0,5,90,5q�6c   $��75��6u   $��3��6�   $��2��6�   #���  6�   �� 5i      -   5        $ MappingNextButton��5i      $
��Next��68   $��Right=�6G   $��0,5,5,5q�6c   $��75��6t   $��3��6   $��2��6�   #���  6�   5j      5k      �� 5l      $��Schedule��6   #���   6(   .��5m      �  5n      �� 5o      $
��100* �6(   68   �� 5p      $��30 �6(   66   .+�5s      �� -   5        $ ScheduleBackButton��5s      $
��Back��69   $��Right=�6H   $��0,5,90,5q�6d   $��75��6v   $��1��6�   $��1��6�   #���  6�   �� 5t      -   5        $ ScheduleNextButton��5t      $
��Next��69   $��Right=�6H   $��0,5,5,0q�6d   $��75��6u   $��1��6�   $��20��6�   $	��Top=�6�   #���  6�   �� 5u      -   5        $��SkipUpdateEventsCheckBox��5u   2   $��	10,10,0,0q�6   $��Skip update events��6R   $	��150��6o   $
��Left=�6{   $	��Top=�6�   $��30��6�   #���  6�   6�   �� 5w      -   5         ��Name$ RunOnceRadioButton��5w      $��Schedule��6;   $
��Left=�6M   $	��154��6h   $��
35,110,0,0q�6u   $	��Top=�6�   $	��314��6�   .��5x      � .+�5y      �� -   5        $ label��5y   $   $��	Frequency��63   $
��Left=�6G   $��	35,62,0,0q�6b   $	��Top=�6u   $	 -1.75,-1.135&�6�   #���  6�   �� 5z      -   5         ��Name$ FrequencyTextBox��5z   &   $
��Left=�6@   $��23��6[   $��
122,65,0,0q�6g    �� TextWrapping$
 Wrap=�6{   $��0��6�   $	��Top=�6�   $	��120��6�   {�.    GenericTextBoxStyle#���  6�   	� 5{      -   5         	�Name$ RunOnceRadioButtonNo��5{   *   $��No��6H   $
��Left=�6V   $��
178,36,0,0q�6q   $	��Top=�6�     GenericRadioButtonStyle#���  6�   	� 5|      -   5        $ RunOnceRadioButtonYes��5|   *   $	��Yes��6I   $
��Left=�6X   $��
122,36,0,0q�6s   $	��Top=�6�    d� 	IsChecked$
 TrueY�6�   $	 1.458,-4.067&�6�   #���  6�   �� 5}      -   5        $ 
label_Copy��5}   $   $��One time job?��68   $
��Left=�6P   $��	35,30,0,0q�6k   $	��Top=�6~   $	 -1.75,-1.135&�6�   #���  6�   5~      5      5�      5�      5�      5�      5�      