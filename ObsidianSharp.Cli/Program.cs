using Microsoft.CodeAnalysis.CSharp;
using ObsidianSharp.Core.markdown;
using ObsidianSharp.Core.reflection;

string inputPath = "../../../../../StockScreener3/";
string outputPath = "../../../../../StockScreener3/Obsidian/";

ReflectionMarkdownGen gen = new(outputPath, inputPath);
gen.Generate();

// ObsidianMdGenerator generator = new();
// generator.AddHeading("Hello world", "hello_world");
// generator.Add("hello ");
// generator.AddBold("world");
//
// generator.AddFootnote("hw", "hello world note");
// generator.AddFootnoteRef("hw");
// generator.NextLine();
//
// generator.AddCode("Console.WriteLine");
//
// generator.BeginCallout(CalloutType.info);
// generator.AddLine("info text");
// generator.AddLine("info text");
// generator.AddLine("info text");
// generator.EndCallout();
//
// generator.AddInternalLink("lol");
//
// Console.WriteLine(generator.Generate());
//
// Console.WriteLine("Hello, World!");
// string path = "../../../../../StockScreener3/";
// // string path = "../../../../";
//
// CSharpParseOptions parseOptions = CSharpParseOptions.Default;
// IEnumerable<string> files = Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories);
// foreach (string file in files) {
//     FileDataExtractor extractor = new(file, parseOptions);
//     extractor.Extract();
// }
//
// Console.WriteLine("Hello, World!");