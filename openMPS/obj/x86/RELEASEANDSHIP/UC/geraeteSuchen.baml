   M S B A M L   `   `   `  ���� R  NZählerstandsermittlung, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:btg.Zaehlerstand.UCbtg.Zaehlerstand.UC  '    !btg.Zaehlerstand.UC.geraeteSuchen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:btg.Zaehlerstand.UC     
�������.��� .+��� -     ��Name$  	bt_suchen��$��Geräte suchen��$��0��$
��Left=�$��
538,25,0,0q�$	��Top=�$	��135��$��25�� K� ZIndex$ 1���� $��IPv4 Adressbereich:��$
��Left=�$��	25,25,0,0q�$	��Top=��� $	��bis��$
��Left=�$��
310,25,0,0q�$	��Top=��� -    ��Name$ tb_ip_rangeStart1��$
��Left=�$��18��$��
144,29,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$��36��$��1��$ 1�� �� 	MaxLength$ 3���� -   $ tb_ip_rangeStart2��$
��Left=�$��18��$��
184,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��2��$ 1��$ 3���� -   $ tb_ip_rangeStart3��$
��Left=�$��18��$��
224,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��3��$ 1��$ 3���� -   $ tb_ip_rangeStart4��$
��Left=�$��18��$��
264,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��4��$ 1��$ 3���� -   $ tb_ip_rangeStop1��$
��Left=�$��18��$��
344,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��5��$ 1��$ 3���� -   $ tb_ip_rangeStop2��$
��Left=�$��18��$��
384,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��6��$ 1��$ 3���� -   $ tb_ip_rangeStop3��$
��Left=�$��18��$��
424,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��7��$ 1��$ 3���� -	   $ tb_ip_rangeStop4��$
��Left=�$��18��$��
464,29,0,0q�$
 Wrap=�$�� ��$	��Top=�$��36��$��8��$ 1��$ 3��&   System.Windows.Controls.DataGrid  -
     Name$ geraeteGrid��$
��Left=�$	��335��$��
25,100,0,0q�$	��Top=�$
��1127��   CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .  �� $��.��$
��Left=�$��
176,29,0,0q�$	��Top=��� $��.��$
��Left=�$��
216,29,0,0q�$	��Top=��� $��.��$
��Left=�$��
256,29,0,0q�$	��Top=��� $��.��$
��Left=�$��
376,29,0,0q�$	��Top=��� $��.��$
��Left=�$��
416,29,0,0q�$	��Top=��� $��.��$
��Left=�$��
456,29,0,0q�$	��Top=�� $
��Left=�$��25��$��
538,25,0,0q�$	��Top=�$	��135��$ 0��.+�� -    �Name$ progress��$
��Left=�$��25��$	��Top=�$	��135���� -    ��Name$ progressPercent��$��Center=�$��Center=�$��1q�
�������