# Architektur
## Am beispiel von Sprachen

## Dependency Injection und Inversion of Control

``` csharp
public class Adder
{
    public int Calculate(int n1, int n2)
    {
        return n1 + n2;
    }
}
public class Subtractor
{
    public int Calculate(int n1, int n2)
    {
        return n1 - n2;
    }
}
```

``` csharp
public int DoMath(int n1, int n2, Operation operation)
{
    int result = 0;
    if(operation == Operation.Subtract)
    {
        var subtractor = new Subtractor();
        result = subtractor.Calculate(n1, n2);
    }
    
    if(operation == Operation.Add)
    {
        var subtractor = new Adder();
        result = subtractor.Calculate(n1, n2);
    }
    return result;
}
```

``` csharp
public interface IMathOperation
{
    int Calculate(int n1, int n2);
}
public class Adder: IMathOperation
{
    public int Calculate(int n1, int n2)
    {
        return n1 + n2;
    }
}
public class Subtractor: IMathOperation
{
    public int Calculate(int n1, int n2)
    {
        return n1 - n2;
    }
}
```

``` csharp
public int DoMath(int n1, int n2, Operation operation)
{
    IMathOperation service = GetMathService(operation);
    
    return service.Calculate(n1, n2);
}
public IMathOperation GetMathService(Operation operation)
{    
    if(operation == Operation.Subtract)
    {
        return new Subtractor();
    }
    
    if(operation == Operation.Add)
    {
        return new Adder();
    }
}
```