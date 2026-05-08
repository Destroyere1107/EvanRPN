namespace EvanRPN.Tests;

// ── Stack Management ─────────────────────────────────────────────
public class StackManagementTests
{
    [Fact]
    public void NewEngine_HasEmptyStack()
    {
        var e = new Engine();
        Assert.True(e.StackIsEmpty);
        Assert.Equal(0, e.Count);
    }

    [Fact]
    public void Push_Peek_Pop_RoundTrip()
    {
        var e = new Engine();

        e.Push(42d);
        Assert.False(e.StackIsEmpty);
        Assert.Equal(1, e.Count);
        Assert.Equal(42d, e.Peek());
        Assert.Equal(1, e.Count); // Peek does not remove
        Assert.Equal(42d, e.Pop());
        Assert.True(e.StackIsEmpty);
    }

    [Fact]
    public void GetStack_ReturnsTopFirstCopy()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        e.Push(3d);

        var snapshot = e.GetStack().ToArray();

        Assert.Equal(3, snapshot.Length);
        Assert.Equal(3d, snapshot[0]);
        Assert.Equal(2d, snapshot[1]);
        Assert.Equal(1d, snapshot[2]);
    }

    [Fact]
    public void Clear_EmptiesTheStack()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);

        e.Clear();

        Assert.True(e.StackIsEmpty);
        Assert.Equal(0, e.Count);
    }
}

// ── Swap / Duplicate / Drop ──────────────────────────────────────
public class SwapDuplicateDropTests
{
    [Fact]
    public void Swap_ReversesTopTwo()
    {
        var e = new Engine();
        e.Push(10d);
        e.Push(20d);

        e.Swap();

        Assert.Equal(10d, e.Pop());
        Assert.Equal(20d, e.Pop());
    }

    [Fact]
    public void Duplicate_CopiesTopValue()
    {
        var e = new Engine();
        e.Push(7d);

        e.Duplicate();

        Assert.Equal(2, e.Count);
        Assert.Equal(7d, e.Pop());
        Assert.Equal(7d, e.Pop());
    }

    [Fact]
    public void Drop_RemovesTopValue()
    {
        var e = new Engine();
        e.Push(100d);
        e.Push(200d);

        e.Drop();

        Assert.Equal(1, e.Count);
        Assert.Equal(100d, e.Peek());
    }
}

// ── Two-Operand Arithmetic ───────────────────────────────────────
public class TwoOperandArithmeticTests
{
    [Fact]
    public void Add_3Plus7_Equals10()
    {
        var e = new Engine();
        e.Push(3d);
        e.Push(7d);
        e.Add();
        Assert.Equal(10d, e.Pop());
    }

    [Fact]
    public void Subtract_10Minus4_Equals6()
    {
        var e = new Engine();
        e.Push(10d);
        e.Push(4d);
        e.Subtract();
        Assert.Equal(6d, e.Pop());
    }

    [Fact]
    public void Multiply_5Times6_Equals30()
    {
        var e = new Engine();
        e.Push(5d);
        e.Push(6d);
        e.Multiply();
        Assert.Equal(30d, e.Pop());
    }

    [Fact]
    public void Divide_20By4_Equals5()
    {
        var e = new Engine();
        e.Push(20d);
        e.Push(4d);
        e.Divide();
        Assert.Equal(5d, e.Pop());
    }

    [Fact]
    public void Mod_17Mod5_Equals2()
    {
        var e = new Engine();
        e.Push(17d);
        e.Push(5d);
        e.Mod();
        Assert.Equal(2d, e.Pop());
    }

    [Fact]
    public void Modulo_AliasWorksSameAsMod()
    {
        var e = new Engine();
        e.Push(17d);
        e.Push(5d);
        e.Modulo();
        Assert.Equal(2d, e.Pop());
    }

    [Fact]
    public void Exp_2ToThe10_Equals1024()
    {
        var e = new Engine();
        e.Push(2d);
        e.Push(10d);
        e.Exp();
        Assert.Equal(1024d, e.Pop());
    }
}

// ── Single-Operand Operations ────────────────────────────────────
public class SingleOperandTests
{
    [Fact]
    public void Negate_Positive_BecomesNegative()
    {
        var e = new Engine();
        e.Push(5d);
        e.Negate();
        Assert.Equal(-5d, e.Pop());
    }

    [Fact]
    public void Negate_Negative_BecomesPositive()
    {
        var e = new Engine();
        e.Push(-3d);
        e.Negate();
        Assert.Equal(3d, e.Pop());
    }

    [Fact]
    public void Sqrt_25_Equals5()
    {
        var e = new Engine();
        e.Push(25d);
        e.Sqrt();
        Assert.Equal(5d, e.Pop());
    }

    [Fact]
    public void SquareRoot_AliasWorksSameAsSqrt()
    {
        var e = new Engine();
        e.Push(16d);
        e.SquareRoot();
        Assert.Equal(4d, e.Pop());
    }

    [Fact]
    public void Absolute_Negative_ReturnsPositive()
    {
        var e = new Engine();
        e.Push(-42d);
        e.Absolute();
        Assert.Equal(42d, e.Pop());
    }

    [Fact]
    public void Absolute_Positive_NoOp()
    {
        var e = new Engine();
        e.Push(42d);
        e.Absolute();
        Assert.Equal(42d, e.Pop());
    }

    [Fact]
    public void Reciprocal_4_Equals025()
    {
        var e = new Engine();
        e.Push(4d);
        e.Reciprocal();
        Assert.Equal(0.25d, e.Pop());
    }
}

// ── Constants ────────────────────────────────────────────────────
public class ConstantTests
{
    [Fact]
    public void PushPi_IsAccurate()
    {
        var e = new Engine();
        e.PushPi();
        var pi = e.Pop();
        Assert.True(Math.Abs(pi - 3.14159265358979d) < 0.0001d);
    }

    [Fact]
    public void PushEuler_IsAccurate()
    {
        var e = new Engine();
        e.PushEuler();
        var euler = e.Pop();
        Assert.True(Math.Abs(euler - 2.71828182845905d) < 0.0001d);
    }
}

// ── Execute Dispatcher ───────────────────────────────────────────
public class ExecuteDispatcherTests
{
    // --- Addition aliases ---
    [Theory]
    [InlineData("+")]
    [InlineData("add")]
    public void Execute_AdditionAliases(string token)
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute(token));
        Assert.Equal(3d, e.Pop());
    }

    // --- Subtraction aliases ---
    [Theory]
    [InlineData("-")]
    [InlineData("sub")]
    public void Execute_SubtractionAliases(string token)
    {
        var e = new Engine();
        e.Push(10d);
        e.Push(3d);
        Assert.True(e.Execute(token));
        Assert.Equal(7d, e.Pop());
    }

    // --- Multiplication aliases ---
    [Theory]
    [InlineData("*")]
    [InlineData("mul")]
    public void Execute_MultiplicationAliases(string token)
    {
        var e = new Engine();
        e.Push(4d);
        e.Push(5d);
        Assert.True(e.Execute(token));
        Assert.Equal(20d, e.Pop());
    }

    // --- Division aliases ---
    [Theory]
    [InlineData("/")]
    [InlineData("div")]
    public void Execute_DivisionAliases(string token)
    {
        var e = new Engine();
        e.Push(20d);
        e.Push(4d);
        Assert.True(e.Execute(token));
        Assert.Equal(5d, e.Pop());
    }

    // --- Mod aliases ---
    [Theory]
    [InlineData("%")]
    [InlineData("mod")]
    public void Execute_ModAliases(string token)
    {
        var e = new Engine();
        e.Push(7d);
        e.Push(3d);
        Assert.True(e.Execute(token));
        Assert.Equal(1d, e.Pop());
    }

    // --- Power aliases ---
    [Theory]
    [InlineData("^")]
    [InlineData("pow")]
    public void Execute_PowerAliases(string token)
    {
        var e = new Engine();
        e.Push(3d);
        e.Push(2d);
        Assert.True(e.Execute(token));
        Assert.Equal(9d, e.Pop());
    }

    // --- Negate aliases ---
    [Theory]
    [InlineData("neg")]
    [InlineData("chs")]
    public void Execute_NegateAliases(string token)
    {
        var e = new Engine();
        e.Push(5d);
        Assert.True(e.Execute(token));
        Assert.Equal(-5d, e.Pop());
    }

    [Fact]
    public void Execute_Abs()
    {
        var e = new Engine();
        e.Push(-8d);
        Assert.True(e.Execute("abs"));
        Assert.Equal(8d, e.Pop());
    }

    [Fact]
    public void Execute_Sqrt()
    {
        var e = new Engine();
        e.Push(9d);
        Assert.True(e.Execute("sqrt"));
        Assert.Equal(3d, e.Pop());
    }

    // --- Reciprocal aliases ---
    [Theory]
    [InlineData("1/x")]
    [InlineData("inv")]
    public void Execute_ReciprocalAliases(string token)
    {
        var e = new Engine();
        e.Push(2d);
        Assert.True(e.Execute(token));
        Assert.Equal(0.5d, e.Pop());
    }

    [Fact]
    public void Execute_Swap()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute("swap"));
        Assert.Equal(1d, e.Pop());
    }

    [Fact]
    public void Execute_Dup()
    {
        var e = new Engine();
        e.Push(42d);
        Assert.True(e.Execute("dup"));
        Assert.Equal(2, e.Count);
    }

    [Fact]
    public void Execute_Drop()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute("drop"));
        Assert.Equal(1, e.Count);
    }

    // --- Clear aliases ---
    [Theory]
    [InlineData("clear")]
    [InlineData("clr")]
    public void Execute_ClearAliases(string token)
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute(token));
        Assert.True(e.StackIsEmpty);
    }

    [Fact]
    public void Execute_Pi_PushesValue()
    {
        var e = new Engine();
        Assert.True(e.Execute("pi"));
        Assert.Equal(1, e.Count);
    }

    [Fact]
    public void Execute_E_PushesValue()
    {
        var e = new Engine();
        Assert.True(e.Execute("e"));
        Assert.Equal(1, e.Count);
    }

    [Fact]
    public void Execute_IsCaseInsensitive()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute("ADD"));
    }

    [Fact]
    public void Execute_TrimsWhitespace()
    {
        var e = new Engine();
        e.Push(1d);
        e.Push(2d);
        Assert.True(e.Execute("  + "));
    }

    [Fact]
    public void Execute_UnknownToken_ReturnsFalse()
    {
        var e = new Engine();
        Assert.False(e.Execute("banana"));
    }
}

// ── Error Handling ───────────────────────────────────────────────
public class ErrorHandlingTests
{
    // --- Empty-stack errors ---
    [Fact]
    public void Peek_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Peek());
    }

    [Fact]
    public void Pop_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Pop());
    }

    [Fact]
    public void Add_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Add());
    }

    [Fact]
    public void Subtract_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Subtract());
    }

    [Fact]
    public void Multiply_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Multiply());
    }

    [Fact]
    public void Divide_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Divide());
    }

    [Fact]
    public void Mod_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Mod());
    }

    [Fact]
    public void Exp_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Exp());
    }

    [Fact]
    public void Negate_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Negate());
    }

    [Fact]
    public void Sqrt_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Sqrt());
    }

    [Fact]
    public void Absolute_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Absolute());
    }

    [Fact]
    public void Reciprocal_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Reciprocal());
    }

    [Fact]
    public void Swap_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Swap());
    }

    [Fact]
    public void Duplicate_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Duplicate());
    }

    [Fact]
    public void Drop_EmptyStack_Throws()
    {
        var e = new Engine();
        Assert.Throws<InvalidOperationException>(() => e.Drop());
    }

    // --- One-value errors for two-operand ops ---
    [Fact]
    public void Add_OneValue_Throws()
    {
        var e = new Engine();
        e.Push(5d);
        Assert.Throws<InvalidOperationException>(() => e.Add());
    }

    [Fact]
    public void Swap_OneValue_Throws()
    {
        var e = new Engine();
        e.Push(5d);
        Assert.Throws<InvalidOperationException>(() => e.Swap());
    }

    // --- Division by zero ---
    [Fact]
    public void Divide_ByZero_Throws()
    {
        var e = new Engine();
        e.Push(10d);
        e.Push(0d);
        Assert.Throws<DivideByZeroException>(() => e.Divide());
    }

    // --- Modulo by zero ---
    [Fact]
    public void Mod_ByZero_Throws()
    {
        var e = new Engine();
        e.Push(10d);
        e.Push(0d);
        Assert.Throws<DivideByZeroException>(() => e.Mod());
    }

    // --- Square root of negative ---
    [Fact]
    public void Sqrt_Negative_Throws()
    {
        var e = new Engine();
        e.Push(-4d);
        Assert.Throws<ArithmeticException>(() => e.Sqrt());
    }

    // --- Reciprocal of zero ---
    [Fact]
    public void Reciprocal_Zero_Throws()
    {
        var e = new Engine();
        e.Push(0d);
        Assert.Throws<DivideByZeroException>(() => e.Reciprocal());
    }
}

// ── Multi-Step Calculations ──────────────────────────────────────
public class MultiStepCalculationTests
{
    [Fact]
    public void RPN_3_4_Plus_2_Times_Equals14()
    {
        // (3 + 4) * 2 = 14 using RPN: 3 4 + 2 *
        var e = new Engine();
        e.Push(3d);
        e.Push(4d);
        e.Add();
        e.Push(2d);
        e.Multiply();
        Assert.Equal(14d, e.Pop());
    }

    [Fact]
    public void Pythagorean_5_12_Equals13()
    {
        // √(5² + 12²) = 13 — RPN: 5 dup * 12 dup * + sqrt
        var e = new Engine();
        e.Push(5d);
        e.Duplicate();
        e.Multiply(); // 25
        e.Push(12d);
        e.Duplicate();
        e.Multiply(); // 144
        e.Add(); // 169
        e.Sqrt(); // 13
        Assert.Equal(13d, e.Pop());
    }

    [Fact]
    public void ExecuteBased_10Minus3_Over_2Plus5_Equals1()
    {
        // (10 - 3) / (2 + 5) = 1
        var e = new Engine();
        e.Push(10d);
        e.Push(3d);
        e.Execute("-");
        e.Push(2d);
        e.Push(5d);
        e.Execute("+");
        e.Execute("/");
        Assert.Equal(1d, e.Pop());
    }

    [Fact]
    public void XRoot_3rdRootOf27_Equals3()
    {
        var e = new Engine();
        e.Push(27d);
        e.Push(3d);
        e.XRoot();
        Assert.Equal(3d, e.Pop());
    }

    [Fact]
    public void Execute_XRoot_4thRootOf16_Equals2()
    {
        var e = new Engine();
        e.Push(16d);
        e.Push(4d);
        e.Execute("xroot");
        Assert.Equal(2d, e.Pop());
    }

    [Fact]
    public void Stack_CleanAfterAllSteps()
    {
        var e = new Engine();
        e.Push(3d);
        e.Push(4d);
        e.Add();
        e.Push(2d);
        e.Multiply();
        e.Pop(); // consume the result
        Assert.True(e.StackIsEmpty);
    }
}