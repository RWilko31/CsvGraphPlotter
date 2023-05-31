//imports
using System;
using ScottPlot;
using Terminal.Gui;

//code
class ReadCSVAndPlotGraph
{    
    public static void Main()
    {
        //CreateGraphFromCsv();        
        StartGui();
    }
    static void CreateGraphFromCsv()
    {
        //ask for file location
        Console.WriteLine("Enter file location:");

        //read file
        string location = Console.ReadLine();
        List<string[]> data = ReadCSV(location);

        //inform user if file has been read 
        promptUser(data);

        //ask user which data to plot
        Console.WriteLine("Input the number for column of x data:");
        string x = Console.ReadLine();        
        Console.WriteLine("Input the number for column of y data:");
        string y = Console.ReadLine();
        
        //data column
        int xCol = int.Parse(x); int yCol = int.Parse(y);

        //ask user if they want to set the start and end points for the selected data
        string xStart, yStart, xEnd, yEnd;
        int xS = -1, xE = -1, yS = -1, yE = -1;;
        Console.WriteLine("Set start and end points for the data? y/n");
        if(Console.ReadLine() == "y")
        {
            Console.WriteLine("Input the start row for x:");
            xStart = Console.ReadLine();    
            Console.WriteLine("Input the end row for x:");
            xEnd = Console.ReadLine();           

            Console.WriteLine("Input the start row for y: ");
            yStart = Console.ReadLine();     
            Console.WriteLine("Input the end row for y:");
            yEnd = Console.ReadLine();
            
            //start points
            xS = int.Parse(xStart); yS = int.Parse(yStart);
            //End points
            xE = int.Parse(xEnd); yE = int.Parse(yEnd);
        } 


        List<String> xPlotData = selectData(data, xCol, xS, xE);
        List<String> yPlotData = selectData(data, yCol, yS, yE);

        //ask user for graph type
        Console.WriteLine("Select graph type: 1.BarChart 2.LineChart");
        string chartType = Console.ReadLine();
        int chart = int.Parse(chartType);
        ScottPlot.Plot plt = PlotData(xPlotData, yPlotData, chart);

        //save graph as png
        SavePlot(plt, location);

        #region Debug print All data in CSV
        // foreach(string[] line in data)
        // {
        //     foreach(string str in line)
        //     {
        //         Console.WriteLine(str);
        //     }
        // }
        #endregion
    }
    static void CreateGraphFromCsvWithGUI(string x, string y, string xStart, string yStart, string xEnd, string yEnd, string chartType, string name, string location)
    {        
        //ask for file location
        //read file
        List<string[]> data = ReadCSV(location);
        
        //data column
        int xCol = int.Parse(x); int yCol = int.Parse(y);

        //start points
        int xS = int.Parse(xStart); int yS = int.Parse(yStart);
        //End points
        int xE = int.Parse(xEnd); int yE = int.Parse(yEnd);
        //get data
        List<String> xPlotData = selectData(data, xCol, xS, xE);
        List<String> yPlotData = selectData(data, yCol, yS, yE);
        //create plot
        int chart = int.Parse(chartType);
        ScottPlot.Plot plt = PlotData(xPlotData, yPlotData, chart);

        //save graph as png
        
        int index = location.LastIndexOf("/");
        string dir = location.Remove(index);

        plt.SaveFig(dir + "/" + name + ".png");
    }
    static void StartGui()
    {
        Application.Init();
        var top = Application.Top;
        var win = new Window("CsvPlotter")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(win);
        
        //set up page
        var labelLoc = new Label("File location:") {X = 3, Y = 2};
        var inputLoc= new TextField("") {X = Pos.Right(labelLoc) + 2, Y = 2, Width = 40};
        var labelXcol = new Label("X column") {X = 3, Y = 4};
        var labelYcol = new Label("Y column") {X = 3, Y = 6};
        var inputXcol = new TextField("") {X = Pos.Right(labelXcol) + 2, Y = 4, Width = 5};
        var inputYcol = new TextField("") {X = Pos.Right(labelYcol) + 2, Y = 6, Width = 5};
        var labelXstart = new Label("X start point") {X = Pos.Right(inputXcol) + 2, Y = 4};
        var labelYstart = new Label("Y start point") {X = Pos.Right(inputYcol) + 2, Y = 6};
        var inputXstart = new TextField("") {X = Pos.Right(labelXstart) + 2, Y = 4, Width = 5};
        var inputYstart = new TextField("") {X = Pos.Right(labelYstart) + 2, Y = 6, Width = 5};
        var labelXend = new Label("X end point") {X = Pos.Right(inputXstart) + 2, Y = 4};
        var labelYend = new Label("Y end point") {X = Pos.Right(inputYstart) + 2, Y = 6};
        var inputXend = new TextField("") {X = Pos.Right(labelXend) + 2, Y = 4, Width = 5};
        var inputYend = new TextField("") {X = Pos.Right(labelYend) + 2, Y = 6, Width = 5};
        var labelGraphType = new Label("Graph Style (1.Bar chart, 2.Linechart)") {X = 3, Y = 8};
        var inputGraphType = new TextField("") {X = Pos.Right(labelGraphType) + 2, Y = 8, Width = 5};
        var labelSave = new Label("Save as:") {X = 3, Y = 10};
        var inputSave = new TextField("") {X = Pos.Right(labelSave) + 2, Y = 10, Width = 20};
        //var labelTest = new Label("is") {X = 3, Y = 15};

        var EnterButton = new Button(3,12,"Create Graph"); 
        EnterButton.Clicked += () => { CreateGraphFromCsvWithGUI(
            inputXcol.Text.ToString(),inputYcol.Text.ToString(),
            inputXstart.Text.ToString(),inputYstart.Text.ToString(),
            inputXend.Text.ToString(),inputYend.Text.ToString(),
            inputGraphType.Text.ToString(),
            inputSave.Text.ToString(),inputLoc.Text.ToString()
            );    
        };
        win.Add(
            labelLoc, inputLoc,
            labelXcol,labelYcol,inputXcol,inputYcol,
            labelXstart,labelYstart,inputXstart,inputYstart,
            labelXend,labelYend,inputXend,inputYend,
            labelGraphType,inputGraphType,
            labelSave,inputSave,
            EnterButton
            //,labelTest
        );

        Application.Run();
    }
    static List<string[]> ReadCSV(string file) //reads and seperates a csv file into a 2D array of strings
    {
        List<string[]> data = new List<string[]>();
        try
        {  
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader reader = new StreamReader(file))
            {
                // Read and display lines from the file until the end of the file is reached.
                string line;           
                while ((line = reader.ReadLine()) != null)
                {
                    //Console.WriteLine(line);                    
                    string[] splitLine = line.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
                    data.Add(splitLine);                 
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        return data;
    }
    static void promptUser(List<string[]> data) //returns a message to the user to indicate if the file has been read
    {
        if(data.Count <= 0) 
        {
            Console.Write("This file does not contain any data /n press any key to exit");
            Console.Read();
            Environment.Exit(1);
        }
        
        Console.Write("File read successfully!");
    }
    static List<string> selectData(List<string[]> data, int column, int startPoint, int endPoint) //gets data from 2d Array and provides a list of data for the axis
    {
        if(startPoint == -1) {startPoint = 0; endPoint = data.Count();}

        List<string> dataList = new List<string>();
        int pos = 1;
        foreach(string[] line in data)
        {
            if(pos >= startPoint)
            {
                if(pos >= endPoint) break;
                #region Debug prints user selected data     
                //Console.WriteLine(line[column]);
                #endregion
                dataList.Add(line[column]);
                pos++;      
            } 
        }
        return dataList;
    }
    static ScottPlot.Plot PlotData(List<string> xPlotData, List<string> yPlotData, int chart) //plots data in the graph type given
    {
        ScottPlot.Plot plt = new ScottPlot.Plot(1080, 720);
        
        List<double> xData = new List<double>(), yData = new List<double>();
        foreach(string value in xPlotData)
        { double point = double.Parse(value); xData.Add(point); }
        double[] xPlot = xData.ToArray();
        foreach(string value in yPlotData)
        { double point = double.Parse(value); yData.Add(point); }
        double[] yPlot = yData.ToArray();

        switch(chart)
        {
            case 1:
                Console.WriteLine("BarChart");
                plt.AddBar(xPlot, yPlot);
                break;
            
            case 2:
                Console.WriteLine("LineChart");
                plt.AddScatter(xPlot, yPlot);
                break;

            default:
                Console.WriteLine("Input not valid. Press any key to exit");
                Console.ReadKey();
                Environment.Exit(1);
                break;
        }
        return plt;        
    }
    static void SavePlot(ScottPlot.Plot plt, string location) //saves the given plot to the given directory
    {        
        Console.WriteLine("Enter file name to save graph as:");
        string name = Console.ReadLine();
        int index = location.LastIndexOf("/");
        string dir = location.Remove(index);

        plt.SaveFig(dir + "/" + name + ".png");
    }
}
