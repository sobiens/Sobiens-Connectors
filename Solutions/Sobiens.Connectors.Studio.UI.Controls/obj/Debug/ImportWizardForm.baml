   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      <    6Sobiens.Connectors.Studio.UI.Controls.ImportWizardForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5        � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $a [pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml?�6%   5      %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      $	��200 �6   6+   �� 5      $
��100* �6   6,   �� 5      $��30 �6   6*    � ColumnDefinitions 5   
   �� 5      $
��100* �6   6.   .+�5   
   � $��0��6   .+�5      �� -   5         ��Name$ SelectedEntityLabelLabel��5      $��Entity��66   $
��Left=�6G   $��18,2,0,0q�6b   $	��Top=�6t      GenericLabelStyle#���   6�   �� 5      -   5        $ SelectedEntityNameLabel��5      $��Entity��65   $
��Left=�6F   $��	100,2,0,0q�6a   $	��Top=�6t   #���   6�   �� 5      -   5         ��Name$ SelectImportFileButton��5   )   $��	Select...��6   $
��Left=�6I   $	��Top=�6e   $��75��6}   $��
200,30,0,0q�6�     GenericButtonStyle#���  6�   �� 5      -   5        $ SelectedFileLabel��5      $��No file selected��6/   $
��Left=�6J   $��	18,30,0,0q�6e   $	��Top=�6x   #���   6�   �� 5       -   5        $( "InActiveRecordsExcludedFieldsLabel��5       $"��Exclude inactive records on ��6@   $
��Left=�6g   $��
300,38,0,0q�6�   $	��Top=�6�    J� RenderTransformOrigin$ -0.645,-0.212&�6�   #���   6�   �� 5!      -   5        $0 *SelectedInActiveRecordsExcludedFieldsLabel��5!      $��Not Selected��6H   $
��Left=�6_   $��
470,38,0,0q�6z   $	��Top=�6�   $ -0.645,-0.212&�6�   #���   6  o� 5"      $��20��6   $��20��6   $��
450,38,0,0q�6*   $
��Left=�6>   $	��Top=�6Y   .@�5#      m� -   5        $+��%InActiveRecordsExcludedFieldsMenuItem��5#      $��0,0,0,0q�6H   ��5$      �� 5%      $��Images/ArrowDown.png��6    $��-7,0,0,0q�6>   6R   5'      5)      5+      �� 5,   
   $��1��6   $��Field Mappings��6    $��0,0,0,0q�68   $��Stretch=�6I   $��Stretch=�6g   $��3��6�   .��5-      �� $
��Auto=�6   $��Disabled=�6>   $	��Top=�6g   $��Stretch=�6   $��0,0,0,0q�6�   $	��150��6�   .��5.      � -   5        $��DynamicGrid��5.   b   $��0,0,0,0q�6   $��Stretch=�6(   $��Stretch=�6F   5/      50      51      � 52   
   $��2��6   .+�53      &   System.Windows.Controls.DataGrid  -	   5        $��
ResultGrid��53   z      CanUserAddRows .  $��0,0,0,0q�6/   $��Stretch=�6@   $��Stretch=�6\   	   AutoGenerateColumns	 .  
   	CellStyle
 54      �� 55       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�56      ��  �� Property �@�ToolTip �� Value 64   �� *SobiensFieldStatusMessage+ �� Triggers 57      �� 58       �� Binding 6*   �� *SobiensFieldStatus+ �� Value Imported6Q   .a�59   "   ��  � ��
 � � �6?   5:      �� 5;       6*   �� *SobiensFieldStatus+ Invalid6Q   .a�5<   "   ��  � ��
 �  ��6?   5=      �� 5>       6*   �� *SobiensFieldStatus+	 Valid6Q   .a�5?   "   ��  � ��
 ��  �6?   5@      5B      5D      5E      � 5G   
   $��3��6   .+�5H      � -
   5         �Name$ ImportDataProgressBar��5H      $��Right=�69   $��30��6U   $��	0,0,210,0q�6a   $	��Top=�6t   $	��224��6�    � Value$ 10Q�6�   
���   �6�   �� 5I      -   5        $ ImportDataProgressTextBox��5I      $��Right=�67   $��23��6S   $��	0,5,240,0q�6_   $
��Done��6r   $	��Top=�6�   $	��150��6�   $ 0.514,-0.044&�6�   
�������#���   6�   �� 5J      -   5        $ ImportButton��5J   (   $��Validate��6   $��Right=�6>   $	��Top=�6[   $��75��6s   $��0,0,20,0q�6~   #���  6�   5K      5L      5M      