   M S B A M L   `   `   `  ����B  >openMPS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:de.as1259.openMPS.UCde.as1259.openMPS.UC  5      )    #de.as1259.openMPS.UC.abraegeStarten   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:de.as1259.openMPS.UC     5      
�������.��5
      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5           Name$  geraeteGrid��5   .   $
��Left=�6   $	��335��6C   $��
25,100,0,0q�6P   $	��Top=�5      $
��1127��6+      CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .     CanUserReorderColumns .     CanUserResizeRows .     
IsReadOnly . 5   f   �� 5   
   -   5         ��Name$ bt_start��5      $��Werte abfragen��6:   $
��Left=�6S   $��	25,25,0,0q�5      $	��Top=�6$   $	��135��6<   $��25��6H   �� 5   
   -   5        $ bt_uebertragen��5      $��Daten direkt Übertragen��6)   $
��Left=�6K   $��
282,24,0,0q�5      $	��Top=�6%   $	��135��6=   $��26��6f   �� 5   
   -   5        $ 	bt_senden��5      $��E-Mail direkt senden��6$   $
��Left=�6C   $��
422,24,0,0q�6^   $	��Top=�5      $	��135��6)   $��26��6M   �� 5   
   -   5        $ 	bt_client��5      $��E-Mail Client Starten��6$   $
��Left=�6D   $��
562,25,0,0q�6_   $	��Top=�5      $	��135��6)   $��25��6M   � 5   
   $
��Left=�6   $��25��6*   $��	25,25,0,0q�66   $	��Top=�6I   $	��135��6a    K� ZIndex$ 0��5      .+�5      � -   5        	 �Name$	 progress��5      $
��Left=�6,   $��25��6G   $	��Top=�6S   $	��135��6k   $y�Hidden=�5      
��� ���$ 2��5   [   �� 5      -   5        
 ��Name$
 progressPercent��5      $��Center=�6-   $��Center=�6Q   $y�Hidden=�5      $��1q�6(   
�������$ 3��6S   5      5!      5"      