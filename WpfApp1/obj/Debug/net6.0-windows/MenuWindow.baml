   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MenuWindow   5   	   W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$�  �Сетевой режим игры "Крестики-нолики". Приложение разработано Б. Москвичёвым��5   	   $	��600��6g   $
��1000��6t   $	��384��6�   $	��640��6�    � WindowStartupLocation$ CenterScreen=�6�    � Icon$ /Images/bovo.ico��6�   .��5	      �  � RowDefinitions 5
   
   �� 5      6   �� 5      $
��0.6* �6   6,   �� 5      6   �� 5      6   �� 5      $��0.40* �6   6-   .+�5   
   �� $��5��6   $��Images/background.jpg��6!   $
��Fill=�6@    J� Opacity$
 0.85Q�6O   �� 5   
   $��0��6   $��Images/logo.png��6   $
��0,10q�66   �� 5   
   -   5         ��Name$ 	NameField��5      $��1��6   
�������$��5q�6E   $	��520��6Q   $	��800��6`   $��Center=�6o    �� TextWrapping$
 Wrap=�6�   $��Center=�6�   $��30(�6�   $B��<Введите своё имя для продолжения��6�   6�   �� 5   
   -   5         ��Name$ createGameBtn��5      $��2��6'   $��Images/createGame.png��64   6�   �� 5   
   -   5        $ connectToGameBtn��5      $��3��6*   $��Images/connect.png��67   6�   5      5      