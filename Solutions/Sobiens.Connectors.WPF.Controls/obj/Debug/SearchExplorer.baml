   M S B A M L   `   `   `  ����Z  VSobiens.Connectors.WPF.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Sobiens.Connectors.WPF.ControlsSobiens.Connectors.WPF.Controls  5      Q-clr-namespace:Sobiens.Connectors.WPF.ControlsSobiens.Connectors.WPF.Controls  5   7   4    .Sobiens.Connectors.WPF.Controls.SearchExplorer   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >wpfc-clr-namespace:Sobiens.Connectors.WPF.Controls     5      <my-clr-namespace:Sobiens.Connectors.WPF.Controls     5   7   =my1-http://schemas.microsoft.com/wpf/2008/toolkit     6p   .��5	      � .+�5
      	� -   5        $��radioButtonGeneral��5
   a   $��General��6   $��16��6&   $
��Left=�62   $��
152,12,0,0q�6M     �� 
IsSelected  . $	��Top=�6�    	� 	GroupName$ ContentPane��6�   {�. $y�Hidden=�6�   	� 5      -   5        $��radioButtonWord��5   \   $
��Word��6   $��16��6#   $
��Left=�6/   $��5,10,0,0q�6J   $	��Top=�6s   $ ContentPane��6�    d� 	IsChecked$
 TrueY�6�   	� 5      -   5        $��radioButtonExcel��5   ^   $��Excel��6   $��16��6$   $
��Left=�60   $��	60,10,0,0q�6K   $	��Top=�6v   $ ContentPane��6�   	� 5      -   5        $��radioButtonOutlook��5   a   $��Outlook��6   $��16��6&   $
��Left=�62   $��
110,10,0,0q�6M   $	��Top=�6{   $ ContentPane��6�   �� 5   
   -   5        $��TemplateComboBox��5      $��23��6   $	��120��67   $	��Top=�6C   $
��Left=�6[   $��5,32,0,0q�6v   6�   �� 5   
   -   5        $��SearchLocationComboBox��5   N   $��23��6   $
��Left=�6   $��
130,32,0,0q�6:   $	��Top=�6l   $	��120��6�    �� DisplayMemberPath$ Folder.Title��6�   �� 5   
   -   5        $��FiltersButton��5   t      Filters#���   6   $	��Top=�65   $
��Left=�6M   $��23��6h   $��75��6�   $��
255,32,0,0q�6�   {�.  6�   3   -Sobiens.Connectors.WPF.Controls.SearchTextBox  5   
   -   5          Name$ SearchTextBox��5   '   3   -Sobiens.Connectors.WPF.Controls.SearchTextBox   TextChanged$ SearchTextBox_TextChanged��6�   $��23��6   $	��120��6>   $	��Top=�6J   $
��Left=�6b   $��
130,60,0,0q�6}   6�   �� 5   
   -	   5        $��SearchButton��5   �    
 Search#���  6   $	��Top=�64   $
��Left=�6L   $��23��6g   $��
255,60,0,0q�6s   $��75��6�   {�.  6�   &   System.Windows.Controls.DataGrid  5   
   -
   5          Name$ SearchResultDataGridView��5   %   $��0,90,0,0q�6      AutoGenerateColumns .  $��Stretch=�6�   $��Stretch=�6�   ��6G   �� *SourceTable+5   	   5      5      