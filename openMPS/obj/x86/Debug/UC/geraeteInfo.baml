   M S B A M L   `   `   `  ����B  >openMPS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:de.as1259.openMPS.UCde.as1259.openMPS.UC  5      1clr-namespace:DesignInControlDesignInControl  5      &     de.as1259.openMPS.UC.geraeteInfo   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:de.as1259.openMPS.UC     5      9DesignInControlclr-namespace:DesignInControl     5      N JFluent, Version=5.0.1.10, Culture=neutral, PublicKeyToken=3e436e32a8c5546ffluenturn:fluent-ribbon  5	      
�������.��5      � -   5          �Name$  
LayoutRoot��5      $��Center=�6   $
��Left=�6:   $��60,0,0,0q�6U    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $
��Left=�6   $��Stretch=�6*   $	��200��6F   .+�5      �� -   5         ��Name$ 
lv_geraete��5      6+   5      �� 5   
    �� Orientation$ Vertical=�6   $��	200,0,0,0q�6,   $��Center=�6?   $��Center=�6\   .+�5      � $��Center=�6   $��Center=�60   .+�5      )   #DesignInControl.CircularProgressBar  $��Center=�66   $��Center=�6S      SegmentColor �	#FF878889   StrokeThickness$ 25��5   O      
Percentage$	 100Q�6d     5      -   5          Name$ progressBlack��5   6   $��Center=�5   6   $��Center=�6S   $ 0Q�5   6   
 �   �$ 25��6Z   5   6   A� 5      
�������$��30,30,30,30q�6'   �� 5       -   5         ��Name$ lb_percentageBlack��5!      $��Center=�5       $��Center=�6<   5!      5"      � 5#      $��Center=�6   $��Center=�60   .+�5$        $��Center=�66   $��Center=�6S    �	#FF878889$ 25��5%   O   $	 100Q�6d     5&      -   5        $ progressCyan��5(   6   $��Center=�5&   6   $��Center=�6S   $ 0Q�5'   6   
 ��� �$ 25��6Y   5(   6   A� 5)      
�������$��30,30,30,30q�6'   �� 5*      -   5        $ lb_percentageCyan��5*   ^   $��Center=�6   $��Center=�6<   6^   5+      � 5,      $��Center=�6   $��Center=�60   .+�5-        $��Center=�66   $��Center=�6S    �	#FF878889$ 25��5.   O   $	 100Q�6d     5/      -   5        $ progressMagenta��51   6   $��Center=�5/   6   $��Center=�6S   $ 0Q�50   6   
 �� ��$ 25��6\   51   6   A� 52      
�������$��30,30,30,30q�6'   �� 53      -   5        $ lb_percentageMagenta��54      $��Center=�53      $��Center=�6<   54      55      � 56      $��Center=�6   $��Center=�60   .+�57        $��Center=�66   $��Center=�6S    �	#FF878889$ 25��58   O   $	 100Q�6d     59      -	   5        $ progressYellow��5;   6   $��Center=�59   6   $��Center=�6S   $ 0Q�5:   6   
 � ���$ 25��6[   5;   6   A� 5<      
�������$��30,30,30,30q�6'   �� 5=      -
   5        $ lb_percentageYellow��5>      $��Center=�5=      $��Center=�6<   5>      5?      5@      5A      5B      