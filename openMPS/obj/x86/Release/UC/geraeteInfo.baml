   M S B A M L   `   `   `  ���� R  NZählerstandsermittlung, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:btg.Zaehlerstand.UCbtg.Zaehlerstand.UC  1clr-namespace:DesignInControlDesignInControl  %    btg.Zaehlerstand.UC.geraeteInfo   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:btg.Zaehlerstand.UC     9DesignInControlclr-namespace:DesignInControl     N JFluent, Version=5.0.1.10, Culture=neutral, PublicKeyToken=3e436e32a8c5546ffluenturn:fluent-ribbon  
�������.��� -     �Name$  
LayoutRoot��$��Center=�$
��Left=�$��60,0,0,0q� � RowDefinitions �� $��* ��� $
��Auto �.+�� $
��Left=�$��Stretch=�$	��200��.+��� -    ��Name$ 
lv_geraete����  �� Orientation$ Vertical=�$��	200,0,0,0q�$��Center=�$��Center=�.+�� $��Center=�$��Center=�.+�)   #DesignInControl.CircularProgressBar  $��Center=�$��Center=�   SegmentColor �	#FF878889   StrokeThickness$ 25��   
Percentage$	 100Q�  -     Name$ progressBlack��$��Center=�$��Center=�$ 0Q�
 �   �$ 25��A� 
�������$��30,30,30,30q��� -    ��Name$ lb_percentageBlack��$��Center=�$��Center=�� $��Center=�$��Center=�.+�  $��Center=�$��Center=� �	#FF878889$ 25��$	 100Q�  -   $ progressCyan��$��Center=�$��Center=�$ 0Q�
 ��� �$ 25��A� 
�������$��30,30,30,30q��� -   $ lb_percentageCyan��$��Center=�$��Center=�� $��Center=�$��Center=�.+�  $��Center=�$��Center=� �	#FF878889$ 25��$	 100Q�  -   $ progressMagenta��$��Center=�$��Center=�$ 0Q�
 �� ��$ 25��A� 
�������$��30,30,30,30q��� -   $ lb_percentageMagenta��$��Center=�$��Center=�� $��Center=�$��Center=�.+�  $��Center=�$��Center=� �	#FF878889$ 25��$	 100Q�  -	   $ progressYellow��$��Center=�$��Center=�$ 0Q�
 � ���$ 25��A� 
�������$��30,30,30,30q��� -
   $ lb_percentageYellow��$��Center=�$��Center=�