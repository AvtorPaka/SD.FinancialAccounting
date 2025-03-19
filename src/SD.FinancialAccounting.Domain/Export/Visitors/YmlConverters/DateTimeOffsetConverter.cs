using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;

namespace SD.FinancialAccounting.Domain.Export.Visitors.YmlConverters;

public class DateTimeOffsetConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?);
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer _)
    {
        var scalar = parser.Consume<Scalar>();
        if (string.IsNullOrEmpty(scalar.Value))
        {
            return null;
        }

        return DateTimeOffset.Parse(
            scalar.Value, 
            null, 
            DateTimeStyles.RoundtripKind
        );
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer __)
    {
        if (value == null)
        {
            emitter.Emit(new Scalar("null"));
            return;
        }

        var dateTimeOffset = (DateTimeOffset)value;
        emitter.Emit(new Scalar(
            dateTimeOffset.ToString("o")
        ));
    }
}
