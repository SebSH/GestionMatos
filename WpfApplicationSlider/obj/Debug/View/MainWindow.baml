   M S B A M L   `   `   `  ����O  KWpfApplicationSlider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:WpfApplicationSliderWpfApplicationSlider  5   	   Q-clr-namespace:WpfApplicationSlider.ViewModelsWpfApplicationSlider.ViewModels  5   	   U/clr-namespace:WpfApplicationSlider.UserControls!WpfApplicationSlider.UserControls  5   	   %    WpfApplicationSlider.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	   .local"clr-namespace:WpfApplicationSlider  5   	   >
ViewModels-clr-namespace:WpfApplicationSlider.ViewModels  5   	   BUserControls/clr-namespace:WpfApplicationSlider.UserControls  5   	     � Title$  
MainWindow��5   	   $	��650��6   $
��1200��6)    � 	Resources 5	       �� DataType5  /WpfApplicationSlider.ViewModels.ClientViewModel2   ,WpfApplicationSlider.UserControls.ClientView � DataContext7  1WpfApplicationSlider.ViewModels.MaterielViewModel4   .WpfApplicationSlider.UserControls.MaterielView3  -WpfApplicationSlider.ViewModels.SiteViewModel0   *WpfApplicationSlider.UserControls.SiteView;  5WpfApplicationSlider.ViewModels.InterventionViewModel8   2WpfApplicationSlider.UserControls.InterventionView��%�  (��       5
   3   *, +)(�� W     5   5   *, +)(�� �     5   1   *, +)(��     5   9   *, +)�� 5
   
   ClientViewTemplate��"  63   .R�5         6&   �� 5      �� 5   
   MaterielViewTemplate��"  65   .R�5         6(   �� 5      �� 5   
   SiteViewTemplate��"  61   .R�5         6$   �� 5      �� 5   
   InterventionViewTemplate��"  69   .R�5         6,   �� 5      5      .��5   
   � .+�5      ]� .+�5      �� ���#f4e1d2.+�5      �� ���#2b303b$	��150��63   $	��620��6?   $��
-101,0,0,0q�6L   ��5      �� 5       �� 
TargetType" ��6    �� Triggers 5      :� 5        :� RoutedEvent$ 
MouseEnter��6+   [�5!   &   �� 5"   &   .��5#   *   �� .��5$   .   t�  �� TargetProperty$ Margin�6A    t� From$ 
-101,0,0,0X�6d   		 t� To$	 0,0,0,0X�6v   
 g� DecelerationRatio$
 .9Q�6�    g� Duration$ 0:0:1D�6�   5%   +   5&   '   5)      :� 5*      $ 
MouseLeave��6+   [�5,   "   �� 5-   &   .��5.   *   �� .��5/   .   t� $ Margin�6A   $ 0,0,0,0X�6d   $	 
-101,0,0,0X�6s   $
 .9Q�6�   $ 0:0:1D�6�   50   +   51   '   54      56      .+�58      �� -   5         ��Name$ ClientButton��58      $	��150��6K   ���#2b303b$��0q�6l   .��59      ]� $��Center=�6    $��0,10,0.4,10q�6;   .+�5;      �� $��Clients��6    $	��100��62   
�������$��Center=�6Q   "	  FontAwesome.WPF.ImageAwesome	  5<       	  Icon$ Users=�6*   $��25��67   $��25��6C   $��5q�6N    	  
Foreground
 �����6Y   5?      5@      �� 5A      -   5        $ MaterielButton��5A      $	��150��6N   ���#2b303b$��0q�6o   .��5B      ]� $��Center=�6$   $��Center=�6?   $��0,0,0,10q�6\   .+�5D      �� $��	Materiels��6$   $	��100��68   
�������$��Center=�6W   	  5E      $ Desktop=�6.   $��25��6=   $��25��6I   $��5q�6T   
 �����6_   5F      5G      �� 5H      -   5        $ IntervButton��5H      $	��150��65   ���#2b303b$��0q�6V   .��5I      ]� $��Center=�6$   $��Center=�6?   $��0,0,0,10q�6\   .+�5J      �� $��Interventions��6$   $	��100��6<   
�������$��Center=�6[   	  5K      $ Gavel=�6.   $��25��6;   $��25��6G   $��5q�6R   
 �����6]   5L      5M      �� 5N      -   5        $ 
SiteButton��5N      $	��150��61   ���#2b303b$��0q�6R   .��5O      ]� $��Center=�6$   $��Center=�6?   $��0,0,0,10q�6\   .+�5P      �� $
��Site��6$   $	��100��63   
�������$��Center=�6R   	  5Q      $ Building=�6.   $��25��6>   $��25��6J   $��5q�6U   
 �����6`   5R      5S      5T      �� 5U      ���#FFFFFF$��0,0,0,0q�6.   �� 5V      $��50,0,0,0q�6!   ��63   �� 5W      5X      5Z      5[      