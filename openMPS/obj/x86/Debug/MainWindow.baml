   M S B A M L   `   `   `  ����  openMPS"    de.as1259.openMPS.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      N JFluent, Version=5.0.1.10, Culture=neutral, PublicKeyToken=3e436e32a8c5546fFluenturn:fluent-ribbon  5        � Title$O  IManaged Print Services    -    Gerätedatenerfassung               (2018)��5       � Icon$ openmps_Nco_12.ico��5      $
��1200��5      $	��600��5        Fluent.RibbonWindow   IsCollapsed .   � 
ResizeMode$ NoResize=�5
      .��5      � .+�5   
   �  K� ZIndex$ 0��6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5        Fluent.Ribbon  -   5        $��Ribbon_control��5   )   $��0��6      Menu 5        Fluent.Backstage  5      -   5          Name$ 	backstage��5   9   
����  �   Content. 5         Fluent.BackstageTabControl  
�������.@�5        Fluent.BackstageTabItem  -   5        	  Name$	 bsi_help��5   E   
   Header$
 Hilfe��66   .��5   "   � -   5         �Name$ 	grid_help��5   '   5        5      -   5        $	 bsi_settings��5   M   $
 Einstellungen��66   .��5   "   � -   5        $ grid_settings��5   '   5        Fluent.SeparatorTabItem  5         5!      -   5        $	 	bsi_close��5!   G   $
 Beenden��66   5"   6   5%      5'         Tabs. 5,        Fluent.RibbonTabItem     Header$ Geräte��6'      Groups. 5-        Fluent.RibbonGroupBox     Header$  ��6,      IsSeparatorVisible .  .@�5.      	  Fluent.Button	  -   5         	 Name$ 	bt_search��5.   ?    	  Header$ Geräte Suchen��6(   $��70��50   (   	  51      -	   5        $ bt_geraetEdit��51   C   $ Geräte bearbeiten��6(   $��70��53   (   	  54      -
   5        $ 
bt_abfrage��54   K   $ Werte abfragen und senden��6(   $	��100��56   (   �� 58      -   5         ��Name$ tbl_info��58   7   $��	230,0,0,0q�6$   $��84��6I   $	��530��6U   $
��Wrap=�6a   �� 59      -   5         ��Name$
 logo��59       $��20,0,0,0q�6.   5;      5D      5N      � 5P      -   5        $��MainGrid��5P      $��1��6#   $��Stretch=�60   $��Stretch=�6N   5Q      � 5R   
   -   5        $ grid_additionalThings��5R      5U      5W      5X      