   M S B A M L   `   `   `  ���� R  NZählerstandsermittlung, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:btg.Zaehlerstand.UCbtg.Zaehlerstand.UC  +    %btg.Zaehlerstand.UC.geraeteBearbeiten   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:btg.Zaehlerstand.UC     
�������$
��1200��.��� .+�&   System.Windows.Controls.DataGrid  -      Name$  geraeteGrid��$
��Left=�$	��335��$��
25,100,0,0q�$	��Top=�$
��1127��   CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .     CanUserReorderColumns .     CanUserResizeRows .  �� -    ��Name$ tb_ip_block1��$
��Left=�$��17��$��
299,30,0,0q� �� TextWrapping$
 Wrap=�$	��255��$	��Top=�$��35��$��1�� K� ZIndex$ 1��{�.  $��Arial)�$��12(�	 �� 	MaxLength$	 3��
 �� TextAlignment$
 Center=��� -   $ tb_ip_block2��$
��Left=�$��17��$��
339,30,0,0q�$
 Wrap=�$	��255��$	��Top=�$��35��$��2��$ 1��{�.  $��Arial)�$��12(�$	 3��$
 Center=��� -   $ tb_ip_block3��$
��Left=�$��17��$��
379,30,0,0q�$
 Wrap=�$	��255��$	��Top=�$��35��$��3��$ 1��{�.  $��Arial)�$��12(�$	 3��$
 Center=��� -   $ tb_ip_block4��$
��Left=�$��17��$��
419,30,0,0q�$
 Wrap=�$	��255��$	��Top=�$��35��$��4��$ 1��{�.  $��Arial)�$��12(�$	 3��$
 Center=��� -   $ tb_SerialNumber��$
��Left=�$��17��$��
480,30,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��200��{�.  �� $��.��$
��Left=�$��
330,28,0,0q�$	��Top=�$ 0���� $��.��$
��Left=�$��
370,28,0,0q�$	��Top=�$ 0���� $��.��$
��Left=�$��
410,28,0,0q�$	��Top=�$ 0���� $\��VUm ein Gerät nachträglich zu ändern klicken Sie bitte auf ein Gerät in der Tabelle��$
��Left=�$��	21,77,0,0q�$	��Top=�$
��Left=� J� RenderTransformOrigin$ -1.738,3.181&�$	��454���� $	��IP:��$
��Left=�$��
276,25,0,0q�$	��Top=�$��32���� $��Aktiv:��$
��Left=�$��
194,25,0,0q�$	��Top=��� -    ��Name$ cb_aktiv��$
��Left=�$��
238,31,0,0q�$	��Top=�{�.  �� -    ��Name$ bt_save��$��	speichern��$
��Left=�$��
845,25,0,0q�$	��Top=�$	��135��{�.  $��25���� -	   $ 
bt_anlegen��$��Gerät neu anlegen��$
��Left=�$��	25,25,0,0q�$	��Top=�$	��135��$��25���� -
   $ bt_del��$��Eintrag löschen��$
��Left=�$��
705,25,0,0q�$	��Top=�$	��135��{�.  $��25���� -   $ bt_anleitung��$��?��$
��Left=�$��1132,25,0,0q�$	��Top=�$��20��$ 1��$��20��$ 0.867,-1.409&�� -    �Name$ progressGrid��$
��Left=�$��25��$��	25,25,0,0q�$	��Top=�$	��135��.+�� -    �Name$ progress��
��� ���$y�Hidden=�$ 1��$��1q�
��������� -    ��Name$ progresslabel��$��Center=�$��Center=�$y�Hidden=�$ 2��