   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      >    8Sobiens.Connectors.Studio.UI.Controls.SyncDataWizardForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5      .��5	      � $��0,0,-213,-65q�6     � RowDefinitions  5
   
   �� 5      $
��100* �6   .+�5   
   �� $��Stretch=�6   $��0,0,0,0q�63   $��Stretch=�6D   $��0��6`   .@�5      �� $��	Sync Data��6   .��5      � ���	#FFE5E5E5  5      �� 5      $
��100* �6(   68    � ColumnDefinitions 5      �� 5      $	��23* �6+   69   �� 5      $
��124* �6+   .+�5      �� -   5         ��Name$ SyncTasksComboBox��5      $
��Left=�6:   $��	23,10,0,0q�6U   $	��Top=�6h   $	��224��6�   $��2��6�   �� 5      -   5         ��Name$ MigrateButton��5      $��	Sync Data��64   $
��Left=�6H   $��223,170,0,0q�6c   $	��Top=�6x   $��75��6�   $��1��6�   6�   � 5      -   5         �Name$ SyncDataProgressBar��5   "   $
��Left=�6?   $��33��6Z   $��
47,161,0,0q�6f   $	��Top=�6z   $	��224��6�   $��2��6�    � Value$ 10Q�6�   
���   � J� RenderTransformOrigin$ 0.518,2.293&�6�   �� 5      -   5         ��Name$ ExportListItemsCheckBox��5      $��Export List Items��6@   $
��Left=�6\   $��	23,37,0,0q�6w   $	��Top=�6�   $��2��6�   �� 5      -   5        $ ImportListItemsCheckBox��5      $��Import List Items��6@   $
��Left=�6\   $��	23,67,0,0q�6w   $	��Top=�6�   $��2��6�   �� 5      -   5        $ ExportDocumentsCheckBox��5      $��Export Documents��6@   $
��Left=�6[   $��	89,37,0,0q�6v   $	��Top=�6�   $��1��6�   �� 5      -   5        $ ImportDocumentsCheckBox��5      $��Import Documents��6@   $
��Left=�6[   $��	91,67,0,0q�6v   $	��Top=�6�   $��1��6�   �� 5      -   5         ��Name$ ProgressTextBox��5      $��2��67   $
��Left=�6K   $��23��6f   $��
47,171,0,0q�6r   	 �� TextWrapping$
	 Wrap=�6�   $��TextBox��6�   $	��Top=�6�   $	��224��6�   $ 0.514,-0.044&�6�   
�������$��SemiBold"�6  ��6�   Z� ��61  Z� �� 5      -	   5        $ SkipUpdateCheckBox��5      $��Skip Updates��6;   $
��Left=�6R   $��
223,67,0,0q�6m   $	��Top=�6�   $��1��6�   �� 5       -
   5        $ LimitVersionsTextBox��5       $��1��6<   $
��Left=�6L   $��23��6g   $��
48,109,0,0q�6s   $
	 Wrap=�6�   $	��Top=�6�   $	��120��6�   {�.  $��0��6�   �� 5!      -   5        
 ��Name$
 label��5!      $��Version limit��6+   $��1��6C   $
��Left=�6S   $��
237,31,0,0q�6n   $	��Top=�6�   �� 5"      -   5        $ LimitVersionsCheckBox��5"      $��Limit versions��6>   $
��Left=�6W   $��
23,111,0,0q�6r   $	��Top=�6�   $��2��6�   �� 5#      -   5        $ ExcludeFieldsTextBox��5#      $��1��6<   $
��Left=�6L   $��23��6g   $��217,112,0,0q�6s   $
	 Wrap=�6�   $	��Top=�6�   $	��120��6�   $�� ��6�   5$      5%      5&      5(      5)      