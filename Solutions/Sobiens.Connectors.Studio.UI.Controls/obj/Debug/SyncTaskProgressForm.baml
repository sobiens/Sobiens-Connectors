   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      @    :Sobiens.Connectors.Studio.UI.Controls.SyncTaskProgressForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5      .��5	      � .+�5
   
   � -   5        $��StatusProgressBar��5
   H   $	��300��6   $��30��6"     � Minimum$  0Q�6.    � Maximum$	 100Q�6:   $
��Left=�6a   $	��Top=�6|   $��	30,10,0,0q�6�   �� 5   
   -   5        $��StatusTextBlock��5      $	��100��6+   $��30��67   $
��Left=�6C   $	��Top=�6^   $��	50,20,0,0q�6v   $��Not Started��6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��TasksDataGrid��5         AutoGenerateColumns .  $	��600��5   0   $
��Left=�5         
IsReadOnly . $��	30,50,0,0q�5      $	��Top=�5      ��. $
��Auto=�5      $
��Auto=�5      ��5      �� 5       �� 
TargetType"  6   .�5      ��  �� Property �@ RowBackground �� Value
 �����66   �� 5        �@ AlternatingRowBackground
 �����6A   5         RowStyle 5      �� 5      )  #System.Windows.Controls.DataGridRow"  6    �� Triggers 5      �� 5      	 �� Binding	 6&   �� *Status+
 �� Value
 	Completed6A   .a�5      ��  � ��
 � � �6;   �� 5       � ��
 �����6;   5       �� 5!      	 6&   �� *Status+

 Failed6A   .a�5"      ��  � ��
 �  ��6;   �� 5#       � ��
 �����6;   5$      5&         Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn   Header$
 Path��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *Path+  5*      $ Action��6%    .  6G   �� *Action+  5+      $ Status��6%    .  6G   �� *Status+  5,      $ Message��6%    .  6H   �� *	Message+5.      �� 5/   
   -   5         ��Name$ RefreshButton��5/      $��Refresh��6(   $
��Left=�6:   $��30,0,0,0q�6U   $	��Top=�6g   $��75��6   $y�Hidden=�6�   6�   51      52      