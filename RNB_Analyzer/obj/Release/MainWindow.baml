   M S B A M L   `   `   `  ���� Q  MLiveCharts, Version=0.6.5.0, Culture=neutral, PublicKeyToken=d1901e6ed2eb0532;,clr-namespace:LiveCharts;assembly=LiveCharts
LiveCharts  K4clr-namespace:LiveCharts.Tooltip;assembly=LiveChartsLiveCharts.Tooltip   Control_theory   RNB_Analyzer.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     <lvc,clr-namespace:LiveCharts;assembly=LiveCharts     HTooltip4clr-namespace:LiveCharts.Tooltip;assembly=LiveCharts     -     �Name$  window�� � Title$ 
MainWindow��$��560.4��$��1360.702��.��� $��	0,0,4.2,0q� � ColumnDefinitions �� $
��530* ��� $
��821* � � RowDefinitions �� �� .+�   LiveCharts.LineChart  -     Name$ TransformChart��$��1��$��15,15.4,15.6,15.4q�%   LiveCharts.CoreComponents.Chart   LegendLocation$ Right=� J� RenderTransformOrigin$ 0.5,0.5&�   LineSmoothness$	 0.8Q�   Series �� *
PFSeries+z�V� .~��� �� 	 �� AngleY$	 0.069Q��� T� 
 T� Y$
 0.399Q�   Legend    LiveCharts.ChartLegend     AxisY    LiveCharts.Axis     Title$
 h(t)��   LiveCharts.DefaultAxes   CleanSeparator   	Separator# Z    LabelFormatter �� *
XFormatter+   AxisX   $ t,сек�� �� *
XFormatter+�� -    ��Name$ label��$��Тип звена:��$
��Left=�$��	10,10,0,0q�$	��Top=�$��26��$��67���� -    ��Name$ kSlider��$
��Left=�$��
229,41,0,0q�$	��Top=�$	��199��$��18�� � Maximum$	 100Q� �� Delay$	 100���� -   $ label1��$9��3Передаточный коэффициент (k)��$
��Left=�$��	10,36,0,0q�$	��Top=�$��26��$	��214���� -   $ 	psiSlider��$
��Left=�$��
229,93,0,0q�$	��Top=�$	��199��$��18��$ 1Q�$	 100�� � Minimum$ -1Q��� -   $ label1_Copy��$<��6Коэффициент демпфирования (ς)��$
��Left=�$��	10,88,0,0q�$	��Top=�$��26��$	��214���� -   $ tSlider��$
��Left=�$��
229,67,0,0q�$	��Top=�$	��199��$��18��$	 100���� -	   $ label1_Copy1��$-��'Постоянная времени (T)��$
��Left=�$��	10,62,0,0q�$	��Top=�$��26��$	��214���� -
    ��Name$ comboBox��$
��Left=�$��
229,10,0,0q�$	��Top=�$	��293��$��22���� -    ��Name$ kText��$
��Left=�$��20��$��
428,40,0,0q� �� TextWrapping$
 Wrap=�$��1��$	��Top=�$��94���� -   $ tText��$
��Left=�$��20��$��
428,66,0,0q�$
 Wrap=�$��1��$	��Top=�$��94���� -   $ psiText��$
��Left=�$��20��$��
428,94,0,0q�$
 Wrap=�$��0��$	��Top=�$��94��  -   $ AchChart��$��14.4,15,15.4,14.6q�$ Right=�$��0��$��1��$	 0.8Q� �� *	ACHSeries+      $
 h(t)�� �� *
XFormatter+   $ 
w,1/сек�� �� *
XFormatter+  -   $ PchChart��$��14.4,15.4,15.4,15.4q�$ Right=�$��1��$��1��$	 0.8Q� �� *	FCHSeries+      $
 h(t)�� �� *
XFormatter+   $ 
w,1/сек�� �� *
XFormatter+�� -   $ label1_Copy2��$9��3Количество точек на графике��$
��Left=�$��
10,119,0,0q�$	��Top=�$��26��$	��214���� -   $ pointsSlider��$
��Left=�$��229,122,0,0q�$	��Top=�$	��199��$��18��$	 200Q�$	 100�� � SmallChange$ 1Q�$ 10Q� � Value$ 20Q��� -   $ 
pointsText��$
��Left=�$��20��$��428,122,0,0q�$
 Wrap=�$��20��$	��Top=�$��94���� -   $ label1_Copy3��$%��Границы графиков��$
��Left=�$��
10,150,0,0q�$	��Top=�$��26��$	��214���� -   $ 
leftBorder��$
��Left=�$��23��$��229,154,0,0q�$
 Wrap=�$��0��$	��Top=�$	��133���� -   $ rightBorder��$
��Left=�$��23��$��367,153,0,0q�$
 Wrap=�$��10��$	��Top=�$	��155���� -   $ bottomGraphComboBox��$
��Left=�$��
10,233,0,0q�$	��Top=�$	��212�� �� SelectedIndex$ 0��.@��� $)��#Переходная функция���� $)��#Импульсная функция���� -   $ rightGraphComboBox��$
��Left=�$��232,233,0,0q�$	��Top=�$	��290��$ 0��.@��� $��АЧХ и ФЧХ���� $��ЛАЧХ и ЛФЧХ���� $��АФХ���� -   $ label1_Copy4��$��График снизу:��$
��Left=�$��
10,202,0,0q�$	��Top=�$��26��$	��212���� -   $ label1_Copy5��$ ��График справа:��$
��Left=�$��232,202,0,0q�$	��Top=�$��26��$	��290��  -   $ 	AFH_chart��$��10.4,10,10.4,0.4q�$ Right=�$��1��$	 0.8Q�$��2��$y�Hidden=� �� *	AFHSeries+   $
 Imag�� �� *
XFormatter+   $
 Real�� �� *
XFormatter+