   M S B A M L   `   `   `  ����   Zählerstandsermittlung#    btg.Zaehlerstand.RibbonWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     N JFluent, Version=5.0.1.10, Culture=neutral, PublicKeyToken=3e436e32a8c5546fFluenturn:fluent-ribbon    � Title$O  IManaged Print Services    -    Gerätedatenerfassung               (2018)�� � Icon$ bg.ico��$
��1200��$	��600��  Fluent.RibbonWindow   IsCollapsed .   � 
ResizeMode$ NoResize=�.��� .+��  K� ZIndex$ 0�� � RowDefinitions �� $
��Auto ��� $��* �.+�  Fluent.Ribbon  -   $��Ribbon_control��$��0��   Menu   Fluent.Backstage  -     Name$ 	backstage��
���  ��   Content.    Fluent.BackstageTabControl  
�������.@�  Fluent.BackstageTabItem  -   	  Name$	 bsi_help��
   Header$
 Hilfe��.��� -    �Name$ 	grid_help��  -   $	 bsi_settings��$
 Einstellungen��.��� -   $ grid_settings��  Fluent.SeparatorTabItem    -   $	 	bsi_close��$
 Beenden��   Tabs.   Fluent.RibbonTabItem     Header$ Geräte��   Groups.   Fluent.RibbonGroupBox     Header$  ��   IsSeparatorVisible .  .@�	  Fluent.Button	  -    	 Name$ 	bt_search�� 	  Header$ Geräte Suchen��$��70��	  -	   $ bt_geraetEdit��$ Geräte bearbeiten��$��70��	  -
   $ 
bt_abfrage��$ Werte abfragen und senden��$	��100���� -    ��Name$ tbl_info��$��	230,0,0,0q�$��84��$	��530��$
��Wrap=��� -    ��Name$
 logo��$��20,0,0,0q�� -   $��MainGrid��$��1��$��Stretch=�$��Stretch=�� -   $ grid_additionalThings��