   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      <    6Sobiens.Connectors.Studio.UI.Controls.SyncTaskListForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $a [pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml?�6%   5      %    5      .��5      � .+�5   
   �� -   5         ��Name$ OpenFolderButton��5      $��Open Folder��6+   $
��Left=�6A   $��10,5,0,0q�6\   $	��Top=�6n      GenericButtonStyle#���   6�   (  "System.Windows.Controls.DatePicker  5   
   -   5        $��HistoryDatePicker��5      $��30��6.   $
��Left=�6:   $��	450,5,0,0q�6U   $	��Top=�6h   6�   �� 5   
   -   5         ��Name$ label��5      $!��Completed task history date��6   $
��Left=�6E   $��	270,5,0,0q�6`   $	��Top=�6s    J� RenderTransformOrigin$ 0.658,0.346&�6�   �� 5   
   -   5        $ RefreshButton��5      $��Refresh��6(   $
��Left=�6:   $��
450,60,0,0q�6U   $	��Top=�6i   #���   6�   �� 5   
   -   5         ��Name$ 
tabControl��5      $
��Left=�6)   $	��196��6D   $��	10,90,0,0q�6Q   $	��Top=�6d   $	��540��6|   .@�5      �� $��Current��6     TabItemStyle#���  6'   .��5      � ���	#FFE5E5E5.+�5      &   System.Windows.Controls.DataGrid  -   5        $��TasksDataGrid��5         AutoGenerateColumns .  $	��500��5   <      
IsReadOnly . $
��Left=�5      $��0,0,0,0q�5      $	��Top=�5      	   Columns	 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Name��61      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6Q   �� *Name+  5#      $
 	Scheduled��61    .  6V   �� *	Scheduled+  5$      $
 Last Run��61    6D   �� *LastRunStartDate+  5%      $
 Interval��61    6D   �� *ScheduleIntervalAsMinute+4  .System.Windows.Controls.DataGridTemplateColumn  5&      $
 Actions��65      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6F      CellTemplate 5'   "   �� 5(   &   .R�5)   *   � .+�5*   .   �� $��
EditButton��6D   -   5        $
��Edit��5*   5   $��50��6�   $
��Left=�6�   $��0,0,0,0q�6�   
 � Tag 6V   ��  �� Path$ ID�6�   �� 5+   .   $��ProgressButton��6H   -   5        $��Progress��5+   5   $��50��6�   $
��Left=�6�   $��60,0,0,0q�6�    6^   �� $ ID�6�   5,   +   5-   '   5/      51      53      54      �� 55      $��	Completed��6   #���  6)   .��56      � ���	#FFE5E5E5.+�57        -	   5        $��PastTasksDataGrid��5<       .  $	��500��57   <    . $
��Left=�59      $��0,0,0,0q�5:      $	��Top=�5;      	 5>        5?      $

 Name��61    .  6Q   �� *Name+  5@      $
 	Scheduled��61    .  6V   �� *	Scheduled+  5A      $
 Last Run��61    6D   �� *LastRunStartDate+  5B      $
 Interval��61    6D   �� *ScheduleInterval+5D      5F      5G      5H      �� 5I   
   -
   5        $ DeleteTaskButton��5I      $��Delete��6+   $
��Left=�6<   $��450,290,0,0q�6W   $	��Top=�6l     DeleteButtonStyle#���  6�   5K      5L      