# A TypeScript compiler for ASP.NET Core

[![Build status](https://ci.appveyor.com/api/projects/status/h7vn1gsn7139r74e?svg=true)](https://ci.appveyor.com/project/madskristensen/weboptimizer-typescript)
[![NuGet](https://img.shields.io/nuget/v/LigerShark.WebOptimizer.TypeScript.svg)](https://nuget.org/packages/LigerShark.WebOptimizer.TypeScript/)

This package compiles markdown files into HTML by hooking into the [LigerShark.WebOptimizer](https://github.com/ligershark/WebOptimizer) pipeline.

## Usage

You can reference any TypeScript file directly in the browser and a compiled ES5 JavaScript document will be served. To set that up, do this:

```c#
services.AddWebOptimizer(pipeline =>
{
    pipeline.CompileTypeScirptFiles();
});
```

Or if you just want to parse specific TypeScript files, do this:

```c#
services.AddWebOptimizer(pipeline =>
{
    pipeline.CompileTypeScriptFiles("/path/file1.ts", "/path/file2.ts");
});
```