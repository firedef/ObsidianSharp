using Microsoft.CodeAnalysis.CSharp;
using ObsidianSharp.Core.markdown;
using ObsidianSharp.Core.reflection;

string inputPath = "../../../../../StockScreener3/";
string outputPath = "../../../../../StockScreener3/Obsidian/";

ReflectionMarkdownGen gen = new(outputPath, inputPath);
gen.Generate();