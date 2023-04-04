using Microsoft.CodeAnalysis;

namespace Paradox.SpanExtensions.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class GenerateSpanOverloadGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {

            ctx.AddSource();
        });
    }
}
