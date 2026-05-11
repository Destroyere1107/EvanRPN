namespace EvanRPN;

// The RPN Engine stores the stack in Engine._stack. This is private, so you must use the provided methods to 
// interact with the stack.

/// <summary>
///     This is the primary Engine of evanRPN. It is a wrapper around C#'s System classes, primarily
///     System.Math and Stack in System.Collections. It is designed to
///     follow the architecture of traditional HP calculators and Postfix Notation.
///     Note: in summaries for the class' methods, please note that:
///     X (Value 1) = value at the top of the stack when an operation is performed
///     Y (Value 2) = value second from the top of the stack when an operation is performed
///     Z (Value 3) = value third from the top of the stack when an operation is performed
///     T (Value 4) = the fourth value from the top of the stack when an operation is performed
///     All values afterward are referred to as 5, 6, etc.
/// </summary>
public class Engine
{
    /// <summary>
    /// This private stack of doubles is where the Engine stores all its values.
    /// All other methods manipulate or read the stack.
    /// </summary>
    private readonly Stack<double> _stack = new();
    
    #region --- Stack Info ---

    /// <summary>
    ///     Returns the number of values on the stack.
    /// </summary>
    public int Count => _stack.Count;

    /// <summary>
    ///     Returns true when the stack is empty.
    /// </summary>
    public bool StackIsEmpty => _stack.Count == 0;
    
    #endregion
    
    #region --- Stack Management ---
    /// <summary>
    ///     Returns a copy of the current stack as an IEnumerable<double> array.
    /// </summary>
    public IEnumerable<double> GetStack()
    {
        return _stack.ToArray();
    }

    /// <summary>
    ///     Reads X without removing it.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public double Peek()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack is empty.");
        return _stack.Peek();
    }

    /// <summary>
    ///     Pushes a given value onto the stack, making it the new X.
    ///     Existing values are pushed one step away from the top of the stack.
    ///     (existing Z becomes T, existing T becomes value 5, etc...)
    /// </summary>
    /// <param name="value">The desired value to push onto the stack.</param>
    public void Push(double value)
    {
        _stack.Push(value);
    }


    /// <summary>
    ///     Deletes X, then returns it.
    ///     (DOES NOT PUT IT BACK ON THE STACK.)
    /// </summary>
    /// <returns>The value removed from the top of the stack.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public double Pop()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack is empty.");
        return _stack.Pop();
    }

    /// <summary>
    ///     Fully clears the stack.
    /// </summary>
    public void Clear()
    {
        _stack.Clear();
    }

    /// <summary>
    ///     Swaps X and Y.
    /// </summary>
    public void Swap()
    {
        RequireStack(2, "swap");
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(a);
        _stack.Push(b);
    }

    /// <summary>
    ///     Duplicates X.
    /// </summary>
    public void Duplicate()
    {
        RequireStack(1, "dup");
        _stack.Push(_stack.Peek());
    }

    /// <summary>
    ///     Removes X from the stack without returning it.
    /// </summary>
    public void Drop()
    {
        RequireStack(1, "drop");
        _stack.Pop();
    }

    /// <summary>
    ///     Removes X from the stack without returning it.
    ///     This is an alias of Engine.Drop().
    /// </summary>
    public void Delete()
    {
        Drop();
    }

    #endregion
    
    #region --- Two-Value Arithmetic ---

    // Methods that manipulate two values:

    /// <summary>
    ///     Removes X and Y from the stack, adds them, then pushes the sum.
    /// </summary>
    public void Add()
    {
        var (a, b) = PopTwo("+");
        _stack.Push(b + a);
    }

    /// <summary>
    ///     Removes X and Y from the stack, subtracts them, then pushes the result.
    /// </summary>
    public void Subtract()
    {
        var (a, b) = PopTwo("-");
        _stack.Push(b - a);
    }

    /// <summary>
    ///     Removes X and Y from the stack, multiplies them, then pushes the product.
    /// </summary>
    public void Multiply()
    {
        var (a, b) = PopTwo("*");
        _stack.Push(b * a);
    }

    /// <summary>
    ///     Removes X and Y from the stack, divides them, then pushes the quotient.
    /// </summary>
    public void Divide()
    {
        var (a, b) = PopTwo("/");
        if (a == 0) throw new DivideByZeroException("Division by zero.");
        _stack.Push(b / a);
    }

    /// <summary>
    ///     Removes X and Y from the stack, divides them, then pushes the remainder.
    ///     Engine.Modulo() is an alias.
    /// </summary>
    /// <exception cref="DivideByZeroException"></exception>
    public void Mod()
    {
        var (a, b) = PopTwo("%");
        if (a == 0) throw new DivideByZeroException("Modulo by zero.");
        _stack.Push(b % a);
    }

    /// <summary>
    ///     Removes X and Y from the stack, divides them, then pushes the remainder.
    ///     This is an alias for Engine.Mod().
    /// </summary>
    /// <exception cref="DivideByZeroException"></exception>
    public void Modulo()
    {
        Mod();
    }
    
    #endregion
    
    #region --- Exponent Math ---

    /// <summary>
    ///     Removes X and Y from the stack, computes Y raised to the Xth power, then pushes the result onto the stack.
    ///     Engine.Power() is an alias.
    /// </summary>
    public void Exp()
    {
        RequireStack(2, "exp");
        var (a, b) = PopTwo("^");
        _stack.Push(Math.Pow(b, a));
    }

    /// <summary>
    ///     Removes X and Y from the stack, computes Y raised to the Xth power, then pushes the result onto the stack..
    ///     This is an alias for Engine.Exp().
    /// </summary>
    public void Power()
    {
        Exp();
    }
    
    #endregion 
    
    #region --- Root Math ---
    
    /// <summary>
    ///     Removes X from the stack, then pushes its square root.
    ///     Engine.SquareRoot() is an alias.
    /// </summary>
    /// <exception cref="ArithmeticException"></exception>
    public void Sqrt()
    {
        RequireStack(1, "sqrt");
        var v = _stack.Pop();
        if (v < 0)
            throw new ArithmeticException("Square root of a negative number. Complex numbers are not supported yet.");
        _stack.Push(Math.Sqrt(v));
    }

    /// <summary>
    ///     Removes X and Y from the stack, computes the xth root of y, then pushes the result.
    /// </summary>
    /// <exception cref="ArithmeticException"></exception>
    public void XRoot()
    {
        var (n, v) = PopTwo("xroot");
        if (v < 0 && n % 2 == 0)
            throw new ArithmeticException("Even root of a negative number. Complex numbers are not supported yet.");
        _stack.Push(Math.Pow(v, 1.0 / n));
    }
    #endregion
    
    #region --- Logarithms ---

    public void Ln()
    { 
        RequireStack(1, "log");
        var v = _stack.Pop();
        _stack.Push(Math.Log(v));
    }

    /// <summary>
    /// Computes the logarithm of Y to the specified base X.
    /// </summary>
    public void LogBase()
    {
        RequireStack(2, "logbase");
        var (a, b) = PopTwo("logbase");
        _stack.Push(Math.Log(b,a));
    }
    
    #endregion

    #region --- One-Value Functions ---

    /// <summary>
    ///     Negates X by multiplying it by -1.
    ///     Engine.Flip() is an alias.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void Negate()
    {
        RequireStack(1, "neg");
        _stack.Push(-_stack.Pop());
    }

    /// <summary>
    ///     Negates X by multiplying it by -1.
    ///     This is an alias of Engine.Negate().
    /// </summary>
    public void Flip()
    {
        Negate();
    }

    /// <summary>
    ///     Removes X from the stack, then pushes its square root.
    ///     This is an alias of Engine.Sqrt().
    /// </summary>
    public void SquareRoot()
    {
        Sqrt();
    }


    /// <summary>
    ///     Removes X from the stack, then pushes its absolute value.
    /// </summary>
    public void Absolute()
    {
        RequireStack(1, "abs");
        _stack.Push(Math.Abs(_stack.Pop()));
    }

    /// <summary>
    ///     Removes X from the stack, then pushes its reciprocal.
    ///     Engine.Inv() is an alias.
    /// </summary>
    public void Reciprocal()
    {
        RequireStack(1, "1/x");
        var v = _stack.Pop();
        if (v == 0) throw new DivideByZeroException("Reciprocal of zero.");
        _stack.Push(1d / v);
    }

    /// <summary>
    ///     Removes X from the stack, then pushes its reciprocal.
    ///     This is an alias for Engine.Reciprocal().
    /// </summary>
    public void Inv()
    {
        Reciprocal();
    }
    
    #endregion
    
    #region --- Trigonometric Functions ---
    
    #region Basic Trig

    public void Sin()
    {
        RequireStack(1, "sin");
        var v = _stack.Pop();
        _stack.Push(Math.Sin(v));
    }
    
    public void Cos()
    {
        RequireStack(1, "cos");
        var v = _stack.Pop();
        _stack.Push(Math.Cos(v));
    }
    
    public void Tan()
    {
        RequireStack(1, "tan");
        var v = _stack.Pop();
        _stack.Push(Math.Tan(v));
    }
    
    #endregion

    #region Inverse Trig

    public void ArcSin()
    {
        RequireStack(1, "asin");
        var v = _stack.Pop();
        _stack.Push(Math.Asin(v));
    }

    public void Asin() => ArcSin();
    
    public void ArcCos()
    {
        RequireStack(1, "acos");
        var v = _stack.Pop();
        _stack.Push(Math.Acos(v));
    }

    public void Acos() => ArcCos();
    
    public void ArcTan()
    {
        RequireStack(1, "atan");
        var v = _stack.Pop();
        _stack.Push(Math.Atan(v));
    }

    public void Atan() => ArcTan();
    
    #endregion
    
    #endregion
   
    #region --- Constants ---

    /// <summary>
    ///     Pushes the value of Pi (π) to the stack.
    /// </summary>
    public void PushPi()
    {
        _stack.Push(Math.PI);
    }

    /// <summary>
    ///     Pushes the value of Euler's Number (e) to the stack.
    /// </summary>
    public void PushEuler()
    {
        _stack.Push(Math.E);
    }

    /// <summary>
    ///     Pushes the value of the Avogadro Constant (NA, or 1 Mol) to the stack.
    /// </summary>
    public void PushNA()
    {
        const double Avogadro = 6.0221408e+23;
        _stack.Push(Avogadro);
    }
    
    #endregion

    #region --- Execute Function ---

    /// <summary>
    ///     Executes the specified operator token by performing the corresponding operation on the stack.
    ///     This is primarily meant for use in environments where functions are not exposed
    ///     as user-clickable buttons.
    /// </summary>
    /// <param name="op">
    ///     A string representing the operator token. Supported tokens include mathematical operators
    ///     ("+", "-", "*", "/", "^"), stack manipulation commands ("swap", "dup", "drop", "clear"),
    ///     and constants ("pi", "e").
    /// </param>
    /// <returns>
    ///     Returns true if the operator token was recognized and the operation was successfully executed.
    ///     Returns false if the token was not recognized.
    /// </returns>
    public bool Execute(string op)
    {
        switch (op.Trim().ToLowerInvariant())
        {
            case "+":
            case "add": Add(); break;
            case "-":
            case "sub": Subtract(); break;
            case "*":
            case "mul": Multiply(); break;
            case "/":
            case "div": Divide(); break;
            case "%":
            case "mod": Modulo(); break;
            case "^":
            case "pow": Exp(); break;
            case "neg":
            case "chs": Negate(); break;
            case "abs": Absolute(); break;
            case "sqrt": SquareRoot(); break;
            case "xroot": XRoot(); break;
            case "1/x":
            case "inv": Reciprocal(); break;
            case "swap": Swap(); break;
            case "dup": Duplicate(); break;
            case "drop": Drop(); break;
            case "clear":
            case "clr": Clear(); break;
            case "pi": PushPi(); break;
            case "e": PushEuler(); break;
            case "na": PushNA(); break;
            default: return false;
        }

        return true;
    }
    
    #endregion

    #region --- Private helpers ---

    /// <summary>
    ///     Pops the two top values from the stack for operations that require two values.
    ///     This is intended for internal use by the Engine. Call Pop() twice if you need this.
    /// </summary>
    /// <param name="opName">The name of the operation requesting the values (for error reporting)</param>
    /// <returns>A tuple of the two topmost values from the stack, where the first element is the former top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the stack contains fewer than two values.</exception>
    private (double top, double second) PopTwo(string opName)
    {
        RequireStack(2, opName);
        var a = _stack.Pop();
        var b = _stack.Pop();
        return (a, b);
    }
    
    /// <summary>
    ///     Ensures that the stack contains the required number of elements before executing an operation.
    ///     This is an internal function for use by the Engine.
    /// </summary>
    /// <param name="needed">The minimum number of elements required on the stack.</param>
    /// <param name="opName">The name of the operation that requires the stack check.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the stack contains fewer elements than required for the specified operation.
    /// </exception>
    private void RequireStack(int needed, string opName)
    {
        if (_stack.Count < needed)
            throw new InvalidOperationException(
                $"'{opName}' requires {needed} value(s) on the stack, but only {_stack.Count} present.");
    }
    #endregion
}
    