   M S B A M L   `   `   `  ����`  \Sobiens.Connectors.Studio.UI.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�bclr-namespace:Sobiens.Connectors.Studio.UI.Controls;assembly=Sobiens.Connectors.Studio.UI.Controls%Sobiens.Connectors.Studio.UI.Controls  5   
   W SSobiens.Connectors.Studio.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Sobiens.Connectors.Studio.UISobiens.Connectors.Studio.UI 5   	   -   'Sobiens.Connectors.Studio.UI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   rmy1bclr-namespace:Sobiens.Connectors.Studio.UI.Controls;assembly=Sobiens.Connectors.Studio.UI.Controls     5   
   <local*clr-namespace:Sobiens.Connectors.Studio.UI     5   	   -   5        ���#f3f2f1  � WindowStyle$
  None=�5	       � AllowsTransparency .  � 
ResizeMode$ CanResizeWithGrip=�5
   $    � Title$ Sobiens Studio��5   	   $��0,0,0,0q�6    $��0q�6:   $��0q�6E   $	��500��6Q   $	��500��6^    � Icon$ Images/Sobiens_20x20.gif��6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $a [pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml?�6%   5      %    5      .��5      � .+�5   
   � $��32��6   $��0,0,0,0q�6   $��Stretch=�6,   $	��Top=�6J   ���#6264a7 � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5       �� $��0��5!      $��22��5"      $��22��5#      $��4q�5$      $��Images/Sobiens_20x20.gif��5%      o� 5'      	 o� 
IsMainMenu	 . $��0��6&   $��1��63   ���#6264a7
�������$��0,5,0,0q�6k   $y�Visible=�6|   .@�5(      m� $
��File��6   ���#6264a7
�������.@�5)      m� -   5        $��OpenMenuItem��5)   -   $
��Open��6   ���#6264a76\   m� 5*      -   5        $��SaveMenuItem��5*   -   $
��Save��6   ���#6264a76\   m� 5+      -   5        $��ScheduledJobsMenuItem��5+   7   $��Scheduled Jobs��6   ���#6264a76x   m� 5,      -   5        $��SyncDataMenuItem��5,   2   $��	Sync Data��6   ���#6264a76i   m� 5-      -   5        $��ExitMenuItem��5-   -   $
��Exit��6   ���#6264a76\   5.      m� 5/      -   5        $��HelpMenuItem��5/   )   $
��Help��6   ���#6264a76o   50      � 51      -   5        $��2��51      $��Stretch=�6#   $��Stretch=�6A   .+�52      �� -	   5        
 ��Name$
 ApplicationVersionLabel��52         GenericLabelStyle#���   69   
�������$	��150��6x   $��Center=�6�   6�   53      �� 55      -
   5        $��MinimizeButton��56      $��3��57      
������� �� EdgeMode$ Aliased=�5:        TitleBarButtonStyle#��[ 5;      .��5<      F� $��46��5=      $��32��5>      ���Ѐ���� � ?���� 	$��1��5B      ��5@      ��  �� Path$ 
Foreground� �� RelativeSource ��  �� AncestorType" ��5B      5C      �� 5D      -   5        $��MaximizeButton��5E      $��4��5F      
�������#��[ 5I      .��5J      F� $��46��5K      $��32��5L      +����I�7� �����7� ������)ѠI��)	$��1��5P      ��5N      �� $ 
Foreground� �� " ��5P      5Q      �� 5R      -   5        $��RestoreButton��5S      
�������$��4��5U      #��[ 5W      .��5X      F� $��46��5Y      $��32��5Z      X����I �� �`� �� �`���)ѠI��)� �8 �� � �8�7� �����7� ����`�`�`	$��1��5^      ��5\      �� $ 
Foreground� �� " ��5^      5_      �� 5`      -   5        $��5��5a      
�������  TitleBarCloseButtonStyle#��[ 5d      .��5e      F� $��46��5f      $��32��5g      +���Ѐ��ا ���� -1Ѐ� -1�����ا 	$��1��5k      ��5i      �� $ 
Foreground� �� " ��5k      5l      5m      � 5n      $��Stretch=�6   $��Stretch=�6)   $��0,32,0,0q�6E    � RowDefinitions 5o   
   �� 5p      $��25 �6   �� 5q      $
��100* �6   �� 5r      $��25 �6    5t   
   �� 5u      $	��40* �6   �� 5v      $	��60* �6   .+�5y   
   =   7Sobiens.Connectors.Studio.UI.Controls.MainRibbonToolbar  -   5          Name$ queryDesignerToolbar��5y   f   ���#6264a7$��0��65   $��0��6B   $��2��6R   $��0,0,0,0q�6�   $��5,0,0,0q�6�   $��Stretch=�6�   $��Stretch=�6�   $��Stretch=�6�   $��30��6  � 5z   
   $��1��6   $��2��6   $��Stretch=�60   $��Stretch=�6L    5{      �� 5|      $
��244* �6#   �� 5}      $��5 �6#   �� 5~      $
��196* �6#   �� 5      $	��26* �6#   �� 5�      $	��21* �6#   .+�5�      @   :Sobiens.Connectors.Studio.UI.Controls.ServerObjectExplorer  -   5          Name$ serverObjectExplorer��5�   7   $��0��6'   $��0,0,0,0q�6U   $��Stretch=�6�   $��Stretch=�6�   $��Stretch=�6�      ShowCheckBoxes .  6�   �� 5�      $��1��6   $��5��6+   $��Stretch=�65   
�������6S   8   2Sobiens.Connectors.Studio.UI.Controls.QueriesPanel  5�      -   5          Name$ queriesPanel��5�   /   $��2��6   $��0,0,0,0q�6E   $��Stretch=�6V   $��Stretch=�6t   $��Stretch=�6�   $��3��6�   5�      �� 5�   
   $��Stretch=�6   $��35��62   $��0,0,0,0q�6>   $��2��6O   $	��Top=�6\   $��2��6t   $��Stretch=�6�   $��Stretch=�6�   ���#6264a7.@�5�      �� $��30��6   $	��Top=�6(   $��Right=�6@   .��5�      �� -   5        $!��StartBackgrounProcessButton��5�   5   $��Start service��6     GenericButtonStyle#���  6�   6�   5�      �� 5�      $��30��6   $	��300��6(   $	��Top=�64   $
��Left=�6L   .��5�      � $��0q�6   .+�5�      � -   5        $��StatusProgressBar��5�   J    � Value$	 100Q�6&   $	��200��62   $��18��6>   $��0,0,0,0q�6c   $��0q�6t   $��Center=�6�   
�������6�   �� 5�      -   5        $��StatusLabel��5�   �   $
��Done��6    $	��165��6/   $��18��6;   $��20,0,0,0q�6G   $��0q�6Y   
�������$
��Bold"�6x   $��Center=�6�   $
��Left=�6�   $��Center=�6�   
������   ClickableLabelStyle#���  6B  6r  �� 5�      -   5        $��LoadingImage��5�   �   9   3Sobiens.Connectors.Studio.UI.Controls.ImageBehavior   AnimatedSource$ Images/loader.GIF��6    $��18��6U   $��18��6`   $
��Left=�6l   $	��Top=�6�   $��0,0,0,0q�6�   $y�Hidden=�6�   5�      5�      5�      5�      5�      5�      