   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      =    7Sobiens.Connectors.Studio.UI.Controls.CompareWizardForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5      $	��500��5        � 	Resources  5       �� MergedDictionaries �� Source�� 5	   
    5
      �� 5      $a [pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml?�6%   5      %    5      .��5      � $��Stretch=�6   $��Stretch=�6'     5   
   *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value)  #System.Windows.Controls.DataGridRow �� Triggers �� Binding �� Value��%  '        '  �     �� 5      "  6   .�5      ��  � ��$ 0,3,0,0q�64   �� 5       � ��$ 0q�6+   �� 5       � ��$ 0q�6,   5      �� 5      "  6   .�5      ��  � ��$ 0,3,0,0q�64   �� 5       � ��$ 0q�6+   �� 5       � ��$ 0q�6,    5      �� 5       6"   �� *DifferenceType+ Missing6E   .a�5      ��  � ��
 �  ��67   �� 5       � ��
 �����67   5      �� 5        6"   �� *DifferenceType+
 Update6E   .a�5!      ��  � ��
 � ���67   �� 5"       � ��
 �����67   5#      �� 5$       6"   �� *DifferenceType+ 
Additional6E   .a�5%      ��  � ��
 ��  �67   �� 5&       � ��
 �����67   5'      5)      5*      	 � RowDefinitions	 5+   
   �� 5,      $
��100* �6   �� 5-      $��30 �6   
 � ColumnDefinitions
 5/   
   �� 50      $
��100* �6   6.   .+�52   
   &   System.Windows.Controls.DataGrid  -   5        $��CompareGrid��52   w   $��0,50,0,0q�6   $��Stretch=�6&   $��Stretch=�6B      CanUserAddRows .     AutoGenerateColumns .  $��Stretch=�6�   $��Stretch=�6�    �� 
GroupStyle 53      (  "System.Windows.Controls.GroupStyle  54         HeaderTemplate 55      �� 56      .R�57      �� .+�58   "   ��    GenericControlStyle#���   6G   ��6,   ��  �� Path$
 Name�6G   59      5:         ContainerStyle 5<      �� 5=      " ��6    .�5>      ��  � �� 5?   "   �� 5@   &    �� 
TargetType" ��66   .R�5A   *   9� ��5B   .   �� 5C   2    �� Orientation$ 
Horizontal=�6=   .+�5D   6   �� $��Calibri)�6[   $��12(�6p   ��6@   �� $
 Name�6p   �� 5E   6   $�� ��6@   �� 5F   6   $��Calibri)�6a   $��12(�6v   ��6@   �� $ 	ItemCount�6v   �� 5G   6   $�� Items��6@   $��Calibri)�6O   $��12(�6d   5H   3   .��5J   .   �� 5K   +   5L   '   5N      5O      5Q         Columns 5T      0  *System.Windows.Controls.DataGridTextColumn  5W      ,  &System.Windows.Controls.DataGridColumn   Header$ Object Type��6%      
IsReadOnly .   GenericDataGridCellStyle1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# �  6k     ControlBaseStyle   HeaderStyle# �  6�      	CellStyle# �  6�      Binding 6L   �� *
ObjectType+6�     5X      $
 Name��6%    . # �  6^   # �  6�   # �  6�    6E   �� *Name+6�     5Y      $ Difference Type��6%   # �  6a   # �  6�   # �  6�    6>   �� *DifferenceType+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5Z         CellTemplate 5[      �� 5\      .R�5]      � .+�5^   "   �� $��ApplyChangeButton��6)   -   5          GenericButtonStyle#���  5^   �   $��25��6�   $��75��6�   y�6b   �� *ApplyButtonVisibility+.��6�   Apply6�   �� 5_   "   $��ShowDifferencesButton��6)   -   5        #���  5_   �   $��25��6�   $��75��6�   y�6j   �� *!ShowDifferencesButtonVisibility+.��6�   Update6�   5`      5a      5c      5e      �� 5f   
   -   5         ��Name$ label��5f      $��Source Object:��6   $
��Left=�68   $	��Top=�6S     GenericLabelStyle#���  6k   �� 5g   
   -   5        $ SourceObjectLabel��5g      $	��***��6+   $
��Left=�69   $	��Top=�6T   $��	100,0,0,0q�6l   #���  6   �� 5h   
   -   5        $ label1��5h      $��Destination Object:��6    $
��Left=�6>   $	��Top=�6Y   $��	200,0,0,0q�6q   #���  6�   �� 5i   
   -   5        $ DestinationObjectLabel��5i      $	��***��60   $
��Left=�6>   $	��Top=�6Y   $��	310,0,0,0q�6q   #���  6�   5j      5l      