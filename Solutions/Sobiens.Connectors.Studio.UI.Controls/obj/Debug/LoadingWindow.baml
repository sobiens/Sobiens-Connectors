   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5   	   9    3Sobiens.Connectors.Studio.UI.Controls.LoadingWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Bmy3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5   	   -   5          � Icon$  Images/Sobiens_20x20.gif��5   
    � Title$ LoadingWindow��5   	   $	��150��6   $	��239��6,    � Topmost . .��5      � $	��120��6   $	��215��6    � RowDefinitions 5   
   �� 5	      $��30 �6   6*   �� 5
      $
��100* �6   6,    � ColumnDefinitions 5   
   �� 5      $
��100* �6   6.   .+�5   
   �� -   5        $��TopGrid��5      $��0��6$   .+�5      �� -   5        $��image1��5   ;   $��30��6   $
��Left=�6    $
��Fill=�6I   $	��200��6X   $F��@/Sobiens.Connectors.Studio.UI.Controls;component/Images/Head.png��6d   5      � 5   
   -   5        $��	MainPanel��5      $��1��6    $��Stretch=�6-   $��Stretch=�6K   .+�5      �� -   5        $��LoadingImage��5   �   9   3Sobiens.Connectors.Studio.UI.Controls.ImageBehavior   AnimatedSource$ Images/loader.GIF��6   $��25��6H   $��25��6S   $
��Left=�6_   $	��Top=�6z   $��00,0,0,0q�6�   6�   � 5      -   5         �Name$ StatusProgressBar��5   P    � Value$ 0Q�6k   $��30��6u   $	��Top=�6�   $��Stretch=�6�   $��30,0,0,0q�6�   �� 5      -   5        $��label1��5   p   $�� ��6   $��25��6   $	��Top=�6+   $
��Left=�6C   $��30,0,0,0q�6^   6p   5      5      5      