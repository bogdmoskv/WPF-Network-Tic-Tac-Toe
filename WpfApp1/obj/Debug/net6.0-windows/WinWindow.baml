   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.WinWindow   5   	   W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  Вы выиграли!��5   	   $	��300��6   $	��500��6+    � WindowStartupLocation$ CenterOwner=�67    � 
ResizeMode$ NoResize=�6[    � WindowStyle$
 None=�6q   .��5	      � ���	#FF328832 � RowDefinitions 5
   
   �� 5      �� 5      .+�5   
   �� $��0��6   $��50(�6!   
�������$��Center=�6B   $��Center=�6]   Вы выиграли!6�   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      �� 5      .+�5      �� -   5         ��Name$ playAgainBtn��5      $��0��6F   $��20q�6V   .��5      �� $��30(�6   $
��Wrap=�6*    �� TextAlignment$ Center=�6>   $��Italic$�6U   
���   �Сыграть ещё раз6�   5      �� 5      -   5        $ exitBtn��5      $��1��6<   $��20q�6L   .��5      �� $��30(�6   $
��Wrap=�6*   $ Center=�6>   $��Italic$�6U   
���   �
Выйти6�   5      5      5      5      