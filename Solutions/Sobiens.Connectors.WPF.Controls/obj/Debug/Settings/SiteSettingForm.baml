   M S B A M L   `   `   `  ����Z  VSobiens.Connectors.WPF.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Sobiens.Connectors.WPF.ControlsSobiens.Connectors.WPF.Controls  5      Q-clr-namespace:Sobiens.Connectors.WPF.ControlsSobiens.Connectors.WPF.Controls  5   7   >    8Sobiens.Connectors.WPF.Controls.Settings.SiteSettingForm   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >wpfc-clr-namespace:Sobiens.Connectors.WPF.Controls     5      <my-clr-namespace:Sobiens.Connectors.WPF.Controls     5   7   
���   �$��10q�6�   $��10q�6�   .��5	      � .+�5
   
   �� -   5        $��	TypeLabel��5
   k      Type#���   6   $��28��61   $
��Left=�6=   $��	31,12,0,0q�6X   $	��Top=�6|   �� 5   
   -   5        $��TypeComboBox��5   N   $��23��6   $
��Left=�6   $��
156,17,0,0q�6:   $	��Top=�6b   $	��120��6z   
  � Tag$  SiteSettingType��6�   $��1��6�   6�   �� 5   
   -   5        $��URLLabel��5   j     URL#���  6   $��23��60   $
��Left=�6<   $��	31,59,0,0q�6W   $	��Top=�6z   $	��119��6�   �� 5   
   -   5        $��
URLTextBox��5   M   $��23��6   $
��Left=�6   $��
156,59,0,0q�69   $	��Top=�6_   $	��326��6w   $	  Url��6�   $��2��6�   �� 5   
   -   5        $��DefaultCredentialCheckBox��5   |     Default credential#���  6   $��16��6B   $
��Left=�6N   $��	39,88,0,0q�6i   $	��Top=�6�   $  UseDefaultCredential��6�   $��3��6,  �� 5   
   -   5        $��UsernameLabel��5   s     
DomainUser#���  6   $��28��68   $
��Left=�6D   $��
52,110,0,0q�6_   $	��Top=�6�   $��98��6�   �� 5   
   -   5        $��UserTextBox��5   N   $��23��6   $
��Left=�6   $��156,115,0,0q�69   $	��Top=�6a   $	��326��6y   $  Username��6�   $��4��6�   �� 5   
   -   5        $��label2��5   p     Password#���  6   $��24��65   $
��Left=�6A   $��
52,144,0,0q�6\   $	��Top=�6~   $��98��6�   9   3Sobiens.Connectors.WPF.Controls.BindablePasswordBox  5   
   -	   5          Name$ PasswordTextBox��5   Q   $
��Left=�6!   $��156,148,0,0q�6<   $	��Top=�6j   $	��326��6�   $  Password��6�   �� 5   
   -
   5        $!��ClaimAuthenticationCheckBox��5   �     Claim Authentication#���  6   $��20��6D   $
��Left=�6P    d� 	IsChecked$ FalseY�6k   $��
39,178,0,0q�6}   $��6��6�   $  UseClaimAuthentication��6�   $	��Top=�6�   {�. $	��237��6  9� 5   
   -   5        $��AdvancedSharePointSetting��5        SharePoint advanced setting#���  64   $��Stretch=�6k   $��
20,204,0,0q�6�   $��Stretch=�6�   .��5      �� ���	#FFD1D1D1$��1,0,0,0q�6-   $��10,0,0,0q�6G   ._�5      � .+�5      ��   Check in object after#���  6   $��0q�6J   �� 5      -   5        $��CheckInAfterCopyCheckBox��5   .   $��	10,17,0,0q�6   $��7��6N   $  CheckInAfterCopy��6[   .��5      ��   move or copy#���  6   $��0q�6E   6T   �� 5      -   5        $(��"CheckInAfterEditPropertiesCheckBox��5   .   $��	10,34,0,0q�6   $��8��6X   $   CheckInAfterEditProperties��6e   .��5      ��  	 set of all properties#��� 	 6   $��0q�6N   5      �� 5      -   5        $&�� useMajorVersionAsDefaultCheckBox��5   -   $��5,55,0,0q�6   $��9��6U   $  useMajorVersionAsDefault��6b   .��5      ��  +
 'Use major version as default to checkin#��� 
 6   $��0q�6`   5      �� 5       -   5        $+��%limitFolderEditablePropertiesCheckBox��5    1   $��5,76,0,0q�6   $��10��6^   $#  limitFolderEditableProperties��6l   .��5!      ��  $  Limit folder editable properties#���  6    $��0q�6]   5"      5#      5$      5%      5&      5'      