using System.Data;

namespace Calculator;

public partial class AdvancedCalc : ContentPage
{
    
    public AdvancedCalc()
    {
        InitializeComponent();
        OnClear(this, null);

    }

    string currentEntry = "";
    int currentState = 1;
    string mathOperator;
    double firstNumber, secondNumber;
    string decimalFormat = "N0";
    string expr = "";
    bool startListeningToNumber = false;




    void OnSelectNumber(object sender, EventArgs e)
    {
    
        Button button = (Button)sender;
        string pressed = button.Text;

        currentEntry += pressed;

     

        this.resultText.Text = pressed;
        
            expr += pressed;
        
        this.CurrentCalculation.Text = expr;
    }

    void OnSelectModOperator(object sender, EventArgs e) { 

    }

    void OnSelectOperator(object sender, EventArgs e)
    {
        LockNumberValue(resultText.Text);

        currentState = -2;
        Button button = (Button)sender;
        string pressed = button.Text;
        if (pressed == "MOD")
        {
            mathOperator = "%";
            expr += "%";
        }
        else if (pressed == "SQRT")
        {
            mathOperator = "SQRT";
            expr = Math.Sqrt(float.Parse(new DataTable().Compute(expr,null).ToString())).ToString();
        }
        else if (pressed == "×")
        {
            mathOperator = "*";
            expr += "*";
        }
        else if (pressed == "÷")
        {
            mathOperator = "/";
            expr += "/";
        }
        else if (pressed == "+/-")
        {
            mathOperator = "+/-";
            expr = "-(" + expr+ ")";
        }
        else if (pressed == "%")
        {
            mathOperator = "Percentage";
            expr = "(" + expr + ")/100";
        }
        else if (pressed == "(")
        {
            expr += "(";
        }
        else if (pressed == "(")
        {
            expr += ")";
        }
        else
        {

            mathOperator = pressed;
            expr += pressed;
        }
        this.CurrentCalculation.Text = expr;

    }

    private void LockNumberValue(string text)
    {
        double number;
        if (double.TryParse(text, out number))
        {
            if (currentState == 1)
            {
                firstNumber = number;
            }
            else
            {
                secondNumber = number;
            }

            currentEntry = string.Empty;
        }
    }

    void OnClear(object sender, EventArgs e)
    {
        firstNumber = 0;
        secondNumber = 0;
        currentState = 1;
        decimalFormat = "N0";
        this.resultText.Text = "0";
        currentEntry = string.Empty;
        expr = "";
        startListeningToNumber = false;
    }

    void OnCalculate(object sender, EventArgs e)
    {
      
            var dataTable = new DataTable();
            string theValue = dataTable.Compute(expr, null).ToString();
            this.CurrentCalculation.Text = expr;
            this.resultText.Text = theValue;
            startListeningToNumber = false;
            return;
        
        if (currentState == 2)
        {
            if(secondNumber == 0)
                LockNumberValue(resultText.Text);

            double result = Calculator.Calculate(firstNumber, secondNumber, mathOperator);

            this.CurrentCalculation.Text = $"{firstNumber} {mathOperator} {secondNumber}";

            this.resultText.Text = result.ToTrimmedString(decimalFormat);
            firstNumber = result;
            secondNumber = 0;
            currentState = -1;
            currentEntry = string.Empty;
        }
    }    

    void OnNegative(object sender, EventArgs e)
    {
        if (currentState == 1)
        {
            secondNumber = -1;
            mathOperator = "×";
            currentState = 2;
            OnCalculate(this, null);
        }
    }

    void OnPercentage(object sender, EventArgs e)
    {
        if (currentState == 1)
        {
            LockNumberValue(resultText.Text);
            decimalFormat = "N2";
            secondNumber = 0.01;
            mathOperator = "×";
            currentState = 2;
            OnCalculate(this, null);
        }
    }
}
