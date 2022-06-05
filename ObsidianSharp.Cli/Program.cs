using ObsidianSharp.Core.reflection;

if (args.Length != 2) throw new ArgumentException("Arguments must be: input path, output path");

ReflectionMarkdownGen gen = new(args[1], args[0]);
gen.Generate();