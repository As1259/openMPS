   M S B A M L   `   `   `  ����B  >openMPS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:de.as1259.openMPS.UCde.as1259.openMPS.UC  5      (    "de.as1259.openMPS.UC.geraeteSuchen   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:de.as1259.openMPS.UC     5      
�������.��5
      � .+�5   
   �� -   5          ��Name$  	bt_suchen��5   6   $��Geräte suchen��6   $��0��6)   $
��Left=�5      $��
538,25,0,0q�6,   $	��Top=�6@   $	��135��6X   $��25��6d    K� ZIndex$ 1��5      �� 5   
   $��IPv4 Adressbereich:��6   $
��Left=�6.   $��	25,25,0,0q�6I   $	��Top=�6\   �� 5   
   $	��bis��6   $
��Left=�6   $��
310,25,0,0q�69   $	��Top=�6M   �� 5   
   -   5         ��Name$ tb_ip_rangeStart1��5      $
��Left=�6-   $��18��6H   $��
144,29,0,0q�6T    �� TextWrapping$
 Wrap=�5      $�� ��6&   $	��Top=�6.   $��36��6F   $��1��6Q   $ 1��6^    �� 	MaxLength$ 3��5      6    �� 5   
   -   5        $ tb_ip_rangeStart2��5      $
��Left=�6-   $��18��6H   $��
184,29,0,0q�6T   $
 Wrap=�5      $�� ��6&   $	��Top=�6.   $��36��6F   $��2��6Q   $ 1��6^   $ 3��5      6    �� 5   
   -   5        $ tb_ip_rangeStart3��5      $
��Left=�6-   $��18��6H   $��
224,29,0,0q�6T   $
 Wrap=�5      $�� ��6&   $	��Top=�6.   $��36��6F   $��3��6Q   $ 1��6^   $ 3��5      6    �� 5   
   -   5        $ tb_ip_rangeStart4��5      $
��Left=�6-   $��18��6H   $��
264,29,0,0q�6T   $
 Wrap=�5      $�� ��6&   $	��Top=�6.   $��36��6F   $��4��6Q   $ 1��6^   $ 3��5      �� 5   
   -   5        $ tb_ip_rangeStop1��5      $
��Left=�6,   $��18��6G   $��
344,29,0,0q�6S   $
 Wrap=�5      $�� ��6&   $	��Top=�6.   $��36��6F   $��5��6Q   $ 1��6^   $ 3��5      6    �� 5    
   -   5        $ tb_ip_rangeStop2��5       $
��Left=�6,   $��18��6G   $��
384,29,0,0q�6S   $
 Wrap=�5!      $�� ��6&   $	��Top=�6.   $��36��6F   $��6��6Q   $ 1��6^   $ 3��5"      6    �� 5#   
   -   5        $ tb_ip_rangeStop3��5#      $
��Left=�6,   $��18��6G   $��
424,29,0,0q�6S   $
 Wrap=�5$      $�� ��6&   $	��Top=�6.   $��36��6F   $��7��6Q   $ 1��6^   $ 3��5%      6    �� 5&   
   -	   5        $ tb_ip_rangeStop4��5&      $
��Left=�6,   $��18��6G   $��
464,29,0,0q�6S   $
 Wrap=�5'      $�� ��6&   $	��Top=�6.   $��36��6F   $��8��6Q   $ 1��6^   $ 3��5(      &   System.Windows.Controls.DataGrid  5)   
   -
   5          Name$ geraeteGrid��5)   .   $
��Left=�6   $	��335��6C   $��
25,100,0,0q�6P   $	��Top=�5*      $
��1127��6+      CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .  5+   L   �� 5,   
   $��.��6   $
��Left=�6   $��
176,29,0,0q�67   $	��Top=�6K   �� 5-   
   $��.��6   $
��Left=�6   $��
216,29,0,0q�67   $	��Top=�6K   �� 5.   
   $��.��6   $
��Left=�6   $��
256,29,0,0q�67   $	��Top=�6K   �� 5/   
   $��.��6   $
��Left=�6   $��
376,29,0,0q�67   $	��Top=�6K   �� 50   
   $��.��6   $
��Left=�6   $��
416,29,0,0q�67   $	��Top=�6K   �� 51   
   $��.��6   $
��Left=�6   $��
456,29,0,0q�67   $	��Top=�6K   � 52   
   $
��Left=�6   $��25��6*   $��
538,25,0,0q�66   $	��Top=�6J   $	��135��6b   $ 0��53      .+�54      � -   5         �Name$ progress��54      $
��Left=�6,   $��25��6G   $	��Top=�6S   $	��135��6k   55      �� 56      -   5         ��Name$ progressPercent��56      $��Center=�6-   $��Center=�6Q   $��1q�57      
�������6(   59      5:      5<      