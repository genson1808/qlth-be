using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace QLTH.Common.Exceptions;

public class ExceptionModel
{
    public string Message { get; set; }
    public IEnumerable<StackFrameModel> StackFrames { get; set; }
    public ExceptionModel InnerException { get; set; }
    public static ExceptionModel Create(Exception ex = null)
    {
        var trace = ex == null ? new StackTrace(true) : new StackTrace(ex, true);
        var model = new ExceptionModel();
        model.Message = ex?.Message;
        model.StackFrames = trace.GetFrames().Reverse()
            .Select(x => new StackFrameModel()
        {
            LineNumber = x.GetFileLineNumber(),
            Method = GetMethodSignature(x.GetMethod()),
            Class = x.GetMethod()?.DeclaringType?.FullName,
            AssemblyName = x.GetMethod()?.DeclaringType?.Assembly?.FullName,
            AssemblyFile = x.GetMethod()?.DeclaringType?.Assembly?.CodeBase,
            CodeFile = x.GetFileName(),
        });
        if (ex?.InnerException != null)
            model.InnerException = ExceptionModel.Create(ex.InnerException);
        return model;
    }
    private static string GetTypeName(Type type)
    {
        return type?.FullName?.Replace('+', '.');
    }
    private static string GetMethodSignature(MethodBase mb)
    {
        var sb = new StringBuilder();
        sb.Append(mb.Name);

        // deal with the generic portion of the method
        if (mb is MethodInfo && ((MethodInfo)mb).IsGenericMethod)
        {
            Type[] typars = ((MethodInfo)mb).GetGenericArguments();
            sb.Append("[");
            int k = 0;
            bool fFirstTyParam = true;
            while (k < typars.Length)
            {
                if (fFirstTyParam == false)
                    sb.Append(",");
                else
                    fFirstTyParam = false;

                sb.Append(typars[k].Name);
                k++;
            }
            sb.Append("]");
        }

        // arguments printing
        sb.Append("(");
        ParameterInfo[] pi = mb.GetParameters();
        bool fFirstParam = true;
        for (int j = 0; j < pi.Length; j++)
        {
            if (fFirstParam == false)
                sb.Append(", ");
            else
                fFirstParam = false;

            String typeName = "<UnknownType>";
            if (pi[j].ParameterType != null)
                typeName = pi[j].ParameterType.Name;
            sb.Append(typeName + " " + pi[j].Name);
        }
        sb.Append(")");
        return sb.ToString();
    }
}
public class StackFrameModel
{
    public int LineNumber { get; set; }
    public string Method { get; set; }
    public string Class { get; set; }
    public string AssemblyName { get; set; }
    public string AssemblyFile { get; set; }
    public string CodeFile { get; set; }
}