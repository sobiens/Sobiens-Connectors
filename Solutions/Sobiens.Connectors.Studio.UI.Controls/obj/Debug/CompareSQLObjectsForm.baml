   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5      A    ;Sobiens.Connectors.Studio.UI.Controls.CompareSQLObjectsForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpfc3clr-namespace:Sobiens.Connectors.Studio.UI.Controls     5      $��Stretch=�5      $��Stretch=�6=   .��5      � $��Stretch=�6   $��Stretch=�6'     � 	Resources  5	   
   )  #System.Windows.Controls.DataGridRow �� 
TargetType �� Triggers �� Binding �� Value �� Property �� Value��%�  '        �� 5
      "  6    5      �� 5       6"   �� *DifferenceType+ Missing6E   .a�5      ��  � ��
 �  ��67   �� 5       � ��
 �����67   5      �� 5       6"   �� *DifferenceType+
 Update6E   .a�5      ��  � ��
 � ���67   5      �� 5       6"   �� *DifferenceType+ 
Additional6E   .a�5      ��  � ��
 ��  �67   5      5      5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $
��100* �6   �� 5      $��30 �6    � ColumnDefinitions 5   
   �� 5      $	��50* �6   6-   �� 5       $��30 �6   6,   �� 5!      $	��50* �6   6-   .+�5#   
   �� -   5        	 ��Name$!	 SourceObjectSchemaTextBlock��5#   $   $��1��6   $��0,0,0,0q�6I   $��Stretch=�6Z   $��Stretch=�6x   
 �� AcceptsReturn
 . 6�   �� 5$   
   -   5        $&	  DestinationObjectSchemaTextBlock��5$   4   $��1��6   $��2��6$   $��0,0,0,0q�6^   $��Stretch=�6o   $��Stretch=�6�   
 . 6�   �� 5%   
   -   5         ��Name$ CopyToRightButton��5%   .   $��1��6   $��1��6   $��>>��6I   $��5,5,5,5q�6|   $	��Top=�6�   $��20��6�   �� 5&   
   -   5        $ CopyToLeftButton��5&   .   $��1��6   $��1��6   $��<<��6H   $��5,35,5,5q�6z   $	��Top=�6�   $��20��6�   �� 5'   
   -   5        $ ExecuteLeftSideButton��5'      $��2��6   $��Execute��6=   $��Right=�6s   $��5,5,5,5q�6�   $	��Top=�6�   $��80��6�   �� 5(   
   -   5        $ ExecuteRightSideButton��5(   .   $��2��6   $��3��6   $��Execute��6N   $��Right=�6�   $��5,5,5,5q�6�   $	��Top=�6�   $��80��6�   �� 5)   
   -   5         ��Name$ label��5)      $��Source Object:��6   $
��Left=�68   $	��Top=�6S   �� 5*   
   -   5        $ SourceObjectLabel��5*      $	��***��6+   $
��Left=�69   $	��Top=�6T   $��	100,0,0,0q�6l   �� 5+   
   -	   5        $ label1��5+       $��2��6   $��Destination Object:��60   $
��Left=�6N   $	��Top=�6i   $��0,0,0,0q�6�   �� 5,   
   -
   5        $ DestinationObjectLabel��5,       $��2��6   $	��***��6@   $
��Left=�6N   $	��Top=�6i   $��	110,0,0,0q�6�   5-      5/      