   M S B A M L   `   `   `  ����^  ZMicrosoft.Lync.Controls, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Lync.Controls;assembly=Microsoft.Lync.ControlsMicrosoft.Lync.Controls  5       SharePointLyncConnector.WPF)   #SharePointLyncConnector.WPF.Window1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      [controlsFclr-namespace:Microsoft.Lync.Controls;assembly=Microsoft.Lync.Controls     5        � Title$  Window1��5      $	��300��6   $	��300��6"   .��5      � .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��Center=�6K   
 � Tag$ ss��6f   .+�5      /   )Microsoft.Lync.Controls.PresenceIndicator  -   5          Name$ Presence��5      )   #Microsoft.Lync.Controls.ContactBase   Source$ sip:john@contoso.com��5      .   (Microsoft.Lync.Controls.AvailabilityIcon   PhotoDisplayMode$ Large=�5      �� 5      $��4,0,0,0q�5      $��Center=�5      ��5      �� *DisplayName+ �� ElementName$ Presence��5      5      ,   &Microsoft.Lync.Controls.SendFileButton  5   
   -   5        $��sendFileButton1��5   P   $
��Left=�6"   $��	33,23,0,0q�6=   $	��Top=�6g   $��40��6   $��53��6�   �� 5   
   -   5        $��button1��5   [   $
��Test��6   $��23��6    $
��Left=�6,   $��
166,28,0,0q�6G   $	��Top=�6j   $��75��6�   6�   �� 5   
   -   5        $��button2��5   c   $��Send Message��6   $��23��6(   $
��Left=�64   $��
154,76,0,0q�6O   $	��Top=�6r   $��94��6�   6�   �� 5   
   -   5        $��button3��5   ^   $��SendFile��6   $��23��6$   $
��Left=�60   $��	33,52,0,0q�6K   $	��Top=�6m   $��75��6�   6�   �� 5   
   -   5        $��button4��5   _   $��ShareApp��6   $��23��6$   $
��Left=�60   $��
48,200,0,0q�6K   $	��Top=�6n   $��75��6�   6�   �� 5   
   -   5        $��HistoryButton��5   _   $��History��6   $��23��6#   $
��Left=�6/   $��168,208,0,0q�6J   $	��Top=�6t   $��75��6�   6�   5      5       