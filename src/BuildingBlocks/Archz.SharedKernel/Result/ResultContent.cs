using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Archz.SharedKernel.Result;
public class ResultContent
{
    private Type InputType { get; }
    private object Value { get; set; }

    private ResultContent(object value)
    {
        Value = value;
        InputType = value.GetType();
    }

    /// <summary>
    /// Encapsulates the content by passing the content type through generics
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    /// <param name="contentData"></param>
    /// <returns></returns>
    public static ResultContent Create<TContent>(TContent contentData) where TContent : new()
    {
        return new ResultContent(contentData);
    }

    public ResultContent(){}
}
