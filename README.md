# CsvGraphPlotter
A simple c# application to read data from a .csv file and plot a graph of its data.

![image](https://github.com/RWilko31/CsvGraphPlotter/blob/main/csvplotter.PNG)

Features:
* Select columns in file for x and y data
* specify the start and end row for the selected columns
* choose between graph styles, currently bar and line
* user can enter a name to save the image of the graph

How to use:

Download the CsvGraphPlotterV1 folder, and then using CMD navigate to the folder and run the dll with dotnet.

E.g dotnet "userFolders"/CsvGraphPlotterV1/ReadCsvAndPlotGraph.dll



Code:

The code is provided in the code folder. The code contains 2 versions which can be changed out in the main function, a gui version as shown in the above image, and a console version using read and write commands in the terminal.

In order to implement the GUI in terminal, the terminal was used: https://www.nuget.org/packages/Terminal.Gui

To plot the graphs, scottplot was used: https://www.nuget.org/packages/scottplot
