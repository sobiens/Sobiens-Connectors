   M S B A M L   `   `   `  ����Z  VSobiens.Connectors.WPF.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Sobiens.Connectors.WPF.ControlsSobiens.Connectors.WPF.Controls  5      ;    5Sobiens.Connectors.WPF.Controls.Settings.SettingsForm   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      >wpfc-clr-namespace:Sobiens.Connectors.WPF.Controls     5      Msdk=http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk     5   .   1   +Sobiens.Connectors.WPF.Controls.HostControl    Loaded$  UserControl_Loaded��6x   .��5
      � -   5         �Name$ 
LayoutRoot��5
      
�������.+�5   
   �� -   5        $��tabControl1��5   P   $	��422��6   $
��Left=�6"   $��	12,12,0,0q�6=   $	��Top=�6c   $	��427��6{   .@�5      �� -   5        $��SitesTabItem��5   ?      Site Settings#���   6   .��5      � .+�5      �� -   5        $��SitesListBox��5   Y   $	��133��6   $
��Left=�6+   $��	17,14,0,0q�6F   $	��Top=�6m   $	��227��6�   
 � Tag$ SiteSettings��6�   �� 5      -   5        $��	NewButton��5   x     Add#���  6   $��23��6=   $
��Left=�6I   $��
250,32,0,0q�6d   $	��Top=�6�   $��75��6�   6�   �� 5      -   5        $��DeleteButton��5   |    
 Delete#���  6   $��23��6@   $
��Left=�6L   $��250,0,0,307q�6g   $��Bottom=�6�   $��75��6�   6�   �� 5      -   5        $��
LogsButton��5   z     Logs#���  6   $��23��6>   $
��Left=�6J   $��159,200,0,0q�6e   $	��Top=�6�   $��75��6�   6�   �� 5      -   5        $��DetailedLogCheckBox��5   �     Detailed Log#���  6   $��16��6H   $
��Left=�6T   $��268,207,0,0q�6o   $	��Top=�6�   $ DetailedLogMode��6�   6�   �� 5      -	   5        $��ConfigurationImportButton��5   �     Import Configuration#���  6   $��23��6N   $
��Left=�6Z   $��
250,90,0,0q�6u   $	��Top=�6�   $	��161��6�   6�   5      5      �� 5      -
   5        $��ExplorerTabItem��5   :     Explorer#���  6   .��5      � .+�5      �� -   5        $��ExplorerLocationListBox��5   Y   $	��133��6   $
��Left=�6+   $��	10,10,0,0q�6F   $ SiteSettings��6x   $	��Top=�6�   $	��227��6�   6�   �� 5      -   5        $��AddExplorerLocationButton��5   x   #���  6   $��23��6=   $
��Left=�6I   $��
257,10,0,0q�6d   $	��Top=�6�   $��75��6�   6�   �� 5      -   5        $"��DeleteExplorerLocationButton��5   {   #���  6   $��23��6@   $
��Left=�6L   $��
257,39,0,0q�6g   $	��Top=�6�   $��75��6�   6�   5      5      �� 5      -   5        $��TemplatesTabItem��5   ;     	Templates#���  6   .��5      � $��Stretch=�6   $��Stretch=�65   .+�5      �� -   5        $��TemplatesListBox��5   Y   $	��150��6   $
��Left=�6+   $��	72,35,0,0q�6F   $ SiteSettings��6q   $	��Top=�6�   $	��227��6�   6�   �� 5       -   5        $��NewTemplateButton��5    x   #���  6   $��23��6=   $
��Left=�6I   $��
305,35,0,0q�6d   $	��Top=�6�   $��75��6�   6�   �� 5!      -   5        $��DeleteTemplateButton��5!   {   #���  6   $��23��6@   $
��Left=�6L   $��
305,64,0,0q�6g   $	��Top=�6�   $��75��6�   6�   �� 5"      -   5        $��TemplateMappingsListBox��5"   t    �� DisplayMemberPath$ Title��6   $	��150��68   $
��Left=�6E   $��
72,224,0,0q�6`   $	��Top=�6�   $	��227��6�   6�   �� 5#      -   5        $��NewTemplateMappingButton��5#   y   #���  6   $��23��6=   $
��Left=�6I   $��305,224,0,0q�6d   $	��Top=�6�   $��75��6�   6�   �� 5$      -   5        $!��DeleteTemplateMappingButton��5$   |   #���  6   $��23��6@   $
��Left=�6L   $��305,253,0,0q�6g   $	��Top=�6�   $��75��6�   6�   �� 5%      -   5        $��TemplateMappingsLabel��5%   �     Template Mappings#���  6   $��28��6J   $
��Left=�6V   $��	8,197,0,0q�6q   $	��Top=�6�   �� 5&      -   5        $��label2��5&   |   #���  6   $��28��6B   $
��Left=�6N   $��	10,10,0,0q�6i   $	��Top=�6�   5'      5(      �� 5)      -   5        $��WorkflowTabItem��5)   :    	 Workflow#��� 	 6   .��5*      � .+�5+      �� -   5        $��TaskListLocationListBox��5+   Y   $	��133��6   $
��Left=�6+   $��	10,10,0,0q�6F   $ SiteSettings��6x   $	��Top=�6�   $	��227��6�   6�   �� 5,      -   5        $��AddTaskListLocationButton��5,   x   #���  6   $��23��6=   $
��Left=�6I   $��
257,10,0,0q�6d   $	��Top=�6�   $��75��6�   6�   �� 5-      -   5        $"��DeleteTaskListLocationButton��5-   {   #���  6   $��23��6@   $
��Left=�6L   $��
257,39,0,0q�6g   $	��Top=�6�   $��75��6�   6�   5.      5/      �� 50      -   5        $��OutlookTabItem��50   '   $��Outlook��6   .��51      � .+�52      	� -   5        $��EmailRadioButton��52   �    
 Email Item msg#��� 
 6"   $��21��6M   $
��Left=�6Y   $��	66,68,0,0q�6t   $	��Top=�6�   $	��345��6�   6�   	� 53      -   5        $$��WordPlusAttachmentsRadioButton��53   \   $��21��6"   $
��Left=�6.   $��	66,41,0,0q�6I   $	��Top=�6�   $	��345��6�    3 /Word document doc attachments as seperate files#���  6�   6�   �� 54      -   5        $!��UploadAutomaticallyCheckBox��54   �    % !Upload automatically on drag-drop#���  6   $��16��6]   $
��Left=�6i   $��	66,95,0,0q�6�   $	��Top=�6�   $	��337��62  �� 55      -   5        $��SaveEmailAsLabel��55   u     Save email as#���  6   $
��Left=�6F   $��
18,6,0,355q�6a   $	��393��6�   �� 56      -    5        $(��"DefaultAttachmentsFolderTitleLabel��56   �    # Default attachments save folder#���  6   $��33��6X   $
��Left=�6d   $��
18,127,0,0q�6   $	��Top=�6�   $	��393��6�   �� 57      -!   5        $��SaveLocationTextBox��57   k   $��23��6   $
��Left=�6*   {�.  $��
66,171,0,0q�6W   $ TemplateURL��6�   $	��Top=�6�   $	��161��6�   �� 58      -"   5        $��SelectLocationButton��58   g   $	��...��6   $��23��6+   $
��Left=�67   $��233,170,0,0q�6R   $	��Top=�6�   $��32��6�   6�   �� 59      -#   5        $+��%DontAskSaveAttachmentLocationCheckBox��59   Z   $��25��6   $
��Left=�6+   $��
66,200,0,0q�6F   $	��Top=�6�     Do not ask save location#���  6�   $	��345��6H  5:      5;      5<      5=      5>      